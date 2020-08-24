using Core.Entities.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WebApi.Models;

namespace WebApi.Shared
{
    public   class GenerateJWT
    {
        public readonly IConfiguration _configuration;

        public GenerateJWT(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public  string GenerateTokenUser(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();           
            var key = Encoding.ASCII.GetBytes(_configuration.GetValue<string>("JwtSecret"));
           
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                     new Claim(ClaimTypes.Name, user.Id.ToString()),
                     new Claim(ClaimTypes.Expiration, DateTime.UtcNow.AddDays(1).ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
        
    }
}
