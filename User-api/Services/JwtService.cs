using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using User_api.Models;

namespace User_api.Services
{
    public class JwtService(IConfiguration configuration) : IJwtService
    {
        public string GenerateToken(UserModel user)
        {
            string key = configuration.GetValue("Jwt:Key", string.Empty) ?? throw new ArgumentNullException(nameof(configuration));
            int expiry = configuration.GetValue("Jwt:ExpiryInMinutes", 60);
            var securitykey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var credentils = new SigningCredentials(securitykey, SecurityAlgorithms.HmacSha256);
            Claim[] claims =
            {
                new (ClaimTypes.NameIdentifier, user.Name),
                new (ClaimTypes.Email, user.Email),
            };

            var token = new JwtSecurityToken(
                    issuer: configuration["Jwt:Issuer"],
                    audience: configuration["Jwt:Audience"],
                    expires: DateTime.Now.AddMinutes(expiry),
                    claims: claims,
                    signingCredentials: credentils
                );

            return new JwtSecurityTokenHandler().WriteToken(token);


        }
    }
}
