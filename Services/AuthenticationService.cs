using Microsoft.EntityFrameworkCore;
using morse_auth.Database;
using morse_auth.Database.Models;
using morse_auth.DTO;

namespace morse_auth.Services
{
    public class AuthenticationService
    {
        private IDbContextFactory<MSDBContext> _contextFactory;
        private SessionEnctyptionService _sessionEncryptionService;

        public AuthenticationService(IDbContextFactory<MSDBContext> contextFactory)
        {
            _contextFactory = contextFactory;
            _sessionEncryptionService = new SessionEnctyptionService();
        }

        public UserDTO Register(AuthUserDTO data)
        {
            UserModel newUser = new UserModel()
            {
                Login = data.Login,
                Password = BCrypt.Net.BCrypt.HashPassword(data.Password)
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
                AccessToken = _sessionEncryptionService.GetJWT(data),
                Id = createdUser.Id,
                Login = data.Login,
            };
        }

        public UserDTO Login(AuthUserDTO data)
        {
            UserDTO user = new UserDTO();

            using (MSDBContext context = _contextFactory.CreateDbContext())
            {
                UserModel? foundUser = context.Users.FirstOrDefault(u => u.Login == data.Login);
                if (foundUser == null)
                {
                    throw new InvalidDataException("User not found");
                }
                if (!BCrypt.Net.BCrypt.Verify(data.Password, foundUser.Password))
                {
                    throw new InvalidDataException("Invalid password");
                }

                user.AccessToken = _sessionEncryptionService.GetJWT(data);
                user.Id = foundUser.Id;
                user.Login = foundUser.Login;
                user.DisplayName = foundUser.DisplayName;
            }

            return user;
        }
    }
}
