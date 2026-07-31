using E_Commerce.Application.Contracts;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace E_Commerce.Infrastucture.Identity.Services
{
    internal class TokenService : ITokenService
    {
        private readonly JwtSettings _settings;

        public TokenService(IOptions<JwtSettings> options)
        {
            _settings = options.Value;
        }
        public string CreateToken(string userId, string email, string userName, IEnumerable<string> roles)
        {
            var tclaims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, userId),
                new Claim(ClaimTypes.Email, email),
                new Claim(ClaimTypes.Name, userName)
            };
            tclaims.AddRange(roles.Select(r => new Claim(ClaimTypes.Role, r)));

            if (string.IsNullOrEmpty(_settings.SecretKey))
                throw new InvalidOperationException("JWT Secret Key Is Missing !");

            if(_settings.SecretKey.Length < 32)
                throw new InvalidOperationException("JWT Secret Key Is Too Short !");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.SecretKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
            {

            } ;

            var token = new JwtSecurityToken(
                issuer: _settings.Issuer,
                audience: _settings.Audience,
                claims: tclaims,
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddMinutes(_settings.ExpirationMinutes),
                signingCredentials:credentials
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }


    }
}
