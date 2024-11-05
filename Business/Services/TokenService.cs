using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Business.Interfaces;
using Contracts.DTO;
using Contracts.Interfaces;
using Contracts.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Business.Services
{
    public class TokenService : ITokenService
    {
        private IConfiguration _configuration;
        private readonly IMongoDbService _mongoDbService;

        public TokenService(IConfiguration configuration, IMongoDbService mongoDbService)
        {
            this._configuration = configuration;
            _mongoDbService=mongoDbService;
        }
        public string GetToken(string email)
        {
            var tokenKey = _configuration["TokenKey"]
                           ?? throw new Exception("cannot access TokenKey");

            if (tokenKey.Length < 64) throw new Exception("Token Key needs to be longer");

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey));

            var claims = new List<Claim>
            {
                new(ClaimTypes.Email, email)
            };

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddSeconds(30),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        public bool CheckRefreshToken(TokenDTO tokenDto)
        {
            bool Filter(RefreshToken refreshToken) => refreshToken.Id == tokenDto.email;
            var token = _mongoDbService.GetObjectByFilter(nameof(RefreshToken), (Func<RefreshToken, bool>)Filter);
            if (token.Token == tokenDto.token) return true;
            return false;
        }

        public string GetRefreshToken(string email)
        {
            var refreshTokenKey = _configuration["RefreshTokenKey"]
                                  ?? throw new Exception("cannot access RefreshTokenKey");

            if (refreshTokenKey.Length < 64) throw new Exception("Token Key needs to be longer");

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(refreshTokenKey));

            var claims = new List<Claim>
            {
                new(ClaimTypes.Email, email)
            };

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(5),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            var refreshToken = new RefreshToken
            {
                Id = email,
                Token = tokenHandler.WriteToken(token)
            };
            bool Filter(RefreshToken refreshToken) => refreshToken.Id == email;
            var checkRefreshToken =
                _mongoDbService.GetObjectByFilter(nameof(RefreshToken), (Func<RefreshToken, bool>)Filter);
            if (checkRefreshToken != null && checkRefreshToken.Id == email)
            {
                _mongoDbService.UpdateObject(nameof(RefreshToken), refreshToken);
            }
            else
            {
                _mongoDbService.AddObject(nameof(RefreshToken), refreshToken);
            }

            return refreshToken.Token;
        }
        
    }
}
