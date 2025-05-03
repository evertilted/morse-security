using System.Text;
using Microsoft.EntityFrameworkCore;
using morse_auth.Database;
using morse_auth.Database.Models;
using morse_auth.DTO;

namespace morse_auth.Services
{
    public class AuthenticationService
    {
        private IDbContextFactory<MSDBContext> _contextFactory;
        private EncryptionService _encryptionService;

        public AuthenticationService(IDbContextFactory<MSDBContext> contextFactory)
        {
            _contextFactory = contextFactory;
            _encryptionService = new EncryptionService();
        }

        public UserDTO Register(AuthUserDTO data)
        {
            UserModel newUser = new UserModel()
            {
                Login = _encryptionService.DecryptItem(data.Login),
                Password = BCrypt.Net.BCrypt.HashPassword(_encryptionService.DecryptItem(data.Password))
            };
            UserModel createdUser = new UserModel();

            using (MSDBContext context = _contextFactory.CreateDbContext())
            {
                createdUser = context.Users.Add(newUser).Entity;
                try
                {
                    context.SaveChanges();
                }
                catch (DbUpdateException)
                {
                    throw;
                }
            }

            return new UserDTO
            {
                AccessToken = _encryptionService.GetJWT(data),
                Id = createdUser.Id,
                Login = data.Login,
            };
        }

        public UserDTO Login(AuthUserDTO data)
        {
            UserDTO user = new UserDTO();

            using (MSDBContext context = _contextFactory.CreateDbContext())
            {
                UserModel? foundUser = context.Users.FirstOrDefault(u => u.Login == _encryptionService.DecryptItem(data.Login));
                if (foundUser == null)
                {
                    throw new InvalidDataException("User not found");
                }
                if (!BCrypt.Net.BCrypt.Verify(_encryptionService.DecryptItem(data.Password), foundUser.Password))
                {
                    throw new InvalidDataException("Invalid password");
                }

                user.AccessToken = _encryptionService.GetJWT(data);
                user.Id = foundUser.Id;
                user.Login = foundUser.Login;
                user.DisplayName = foundUser.DisplayName;
            }

            return user;
        }
    }
}
