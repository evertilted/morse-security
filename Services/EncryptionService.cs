using Microsoft.IdentityModel.Tokens;
using morse_auth.DTO;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace morse_auth.Services
{
    public class EncryptionService
    {
        /// <summary> Returns the key to decrypt this server's responses </summary>
        /// <returns> The public key </returns>
        public string GetPublicEncryptionKey()
        {
            return EncryptionKeys.PublicKey;
        }

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
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: credentials
            );

            return new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                expires = token.ValidTo
            };
        }

        /// <summary>
        /// Decrypts an item encrypted by the client
        /// </summary>
        /// <param name="item">The item to be decrypted</param>
        /// <returns>A string with decrypted item</returns>
        /// <exception cref="CryptographicException">Throws if failed to decrypt</exception>
        public string DecryptItem(string item)
        {
            try
            {
                byte[] encryptedItem = Convert.FromBase64String(item);
                using (var rsa = RSA.Create())
                {
                    rsa.ImportFromPem(EncryptionKeys.PrivateKey);
                    var decryptedItem = rsa.Decrypt(encryptedItem, RSAEncryptionPadding.Pkcs1);
                    return Encoding.UTF8.GetString(decryptedItem);
                }
            }
            catch (CryptographicException)
            {
                throw new CryptographicException("The client encryption key is invalid");
            }
        }
    }
}
