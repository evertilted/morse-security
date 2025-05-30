﻿using Microsoft.IdentityModel.Tokens;
using morse_auth.DTO;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace morse_auth.Services
{
    public class EncryptionService
    {
        /// <summary> Returns existing or creates a new JWT </summary>
        /// <returns> A JWT </returns>
        public object GetJWT(AuthUserDTO user)
        {
            var rsa = RSA.Create();
            rsa.ImportFromPem(TokenParams.PrivateKey);

            var credentials = new SigningCredentials(
                new RsaSecurityKey(rsa),
                SecurityAlgorithms.RsaSha256
            );

            var claims = new[]
            {
            new Claim(ClaimTypes.NameIdentifier, user.Login),
            new Claim(ClaimTypes.Name, user.Login),
            new Claim(ClaimTypes.Role, "User")
            };

            var token = new JwtSecurityToken(
                issuer: TokenParams.Issuer,
                audience: TokenParams.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(15),
                signingCredentials: credentials
            );

            return new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                expires = token.ValidTo
            };
        }
    }
}
