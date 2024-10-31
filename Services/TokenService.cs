using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using EcommerceWebApi.DTO;
using EcommerceWebApi.Interfaces;
using EcommerceWebApi.Models;
using Microsoft.IdentityModel.Tokens;

namespace EcommerceWebApi.Services
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
            var token = _mongoDbService.GetRefreshToken(tokenDto.email);
            if (token == tokenDto.token) return true;
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
                Expires = DateTime.UtcNow.AddSeconds(5),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            var refreshToken = new RefreshToken
            {
                UserId = email,
                Token = tokenHandler.WriteToken(token)
            };

            _mongoDbService.UpdateRefreshToken(refreshToken);

            return refreshToken.Token;
        }
        
    }
}
