using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using PasabuyAPI.Models;
using PasabuyAPI.Services.Implementations;

namespace PasabuyAPI.Configurations.Jwt
{
    public class TokenProvider(IConfiguration configuration)
    {
        public string Create(Users user)
        {
            string secretKey = configuration["Jwt:Secret"];
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(
                [
                    new Claim(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Sub, user.UserIdPK.ToString()),
                    new Claim(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Email, user.Email),
                    new Claim("Username", user.Username),
                    new Claim("Phone Number", user.Phone),
                    new Claim(ClaimTypes.Role, user.CurrentRole.ToString()),
                    new Claim("Verification Status", user.VerificationInfo.VerificationInfoStatus.ToString())
                ]),
                Expires = DateTime.UtcNow.AddMinutes(configuration.GetValue<int>("Jwt:ExpirationInMinutes")),
                SigningCredentials = credentials,
                Issuer = configuration["Jwt:Issuer"],
                Audience = configuration["Jwt:Audience"]
            };

            var handler = new JsonWebTokenHandler();

            string token = handler.CreateToken(tokenDescriptor);

            return token;
        }
    }
}