using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using DeliveryService.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace DeliveryService.Domain.Service
{
    public class JwtAuthService : IJwtAuthService
    {
        private readonly IConfiguration _configuration;

        public JwtAuthService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string CreateJwtToken(User user)
        {
            var audienceConfig = _configuration.GetSection("Audience");
            var signingKey = Convert.FromBase64String(audienceConfig["Secret"]);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = null,              // Not required as no third-party is involved
                Audience = null,            // Not required as no third-party is involved
                IssuedAt = DateTime.UtcNow,
                NotBefore = DateTime.UtcNow,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(signingKey), SecurityAlgorithms.HmacSha256Signature),
                Subject = new ClaimsIdentity(new List<Claim> {
                    new Claim("userid", user.Id.ToString()),
                    new Claim("role", user.Role.ToString()),
                })
            };

            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = jwtTokenHandler.CreateJwtSecurityToken(tokenDescriptor);

            return jwtTokenHandler.WriteToken(jwtToken);
        }
    }
}
