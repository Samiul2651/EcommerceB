using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using EcommerceWebApi.Interfaces;
using EcommerceWebApi.Models;
using Microsoft.IdentityModel.Tokens;

namespace EcommerceWebApi.Services
{
    public class TokenService : ITokenService
    {
        private IConfiguration _configuration;

        public TokenService(IConfiguration configuration)
        {
            this._configuration = configuration;
        }
        public string GetToken(Customer customer)
        {
            var tokenKey = _configuration["TokenKey"]
                           ?? throw new Exception("cannot access TokenKey");

            if (tokenKey.Length < 64) throw new Exception("Token Key needs to be longer");

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey));

            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, customer.Name)
            };

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}
