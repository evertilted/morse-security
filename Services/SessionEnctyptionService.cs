using Microsoft.IdentityModel.Tokens;
using morse_auth.DTO;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace morse_auth.Services
{
    public class SessionEnctyptionService
    {
        /// <summary> Returns the key to decrypt this server's responses </summary>
        /// <returns> The public key </returns>
        public string GetPublicEncryptionKey()
        {
            return EncryptionKeys.PublicKey;
        }

        /// <summary> Returns existing or creates a new JWT </summary>
        /// <returns> A JWT </returns>
        public object GetJWT(UserDTO user)
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
            new Claim(ClaimTypes.Role, "Admin")
            };

            var token = new JwtSecurityToken(
                issuer: TokenParams.Issuer,
                audience: TokenParams.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
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
