using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Tryitter.Application.Interfaces;
using Tryitter.Domain.Entity;

namespace Tryitter.Application.Services.Auth
{
    public class TokenGenerate : ITokenGenerator
    {
        public string TokenConstantesSecret = "alshdjkfhalsdjfhlasdjfhlasdj";

        public string GetToken(Student Student)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.ASCII.GetBytes(TokenConstantesSecret)),
                    SecurityAlgorithms.HmacSha256Signature
               
                ),
                Expires = DateTime.Now.AddDays(1),
                Claims = new Dictionary<string, Object>(),

                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, Student.Id.ToString()),
                })
            };

            tokenDescriptor.Claims.Add("Student", true);

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}
