using Microsoft.EntityFrameworkCore;
using morse_auth.Database;
using morse_auth.Database.Models;
using morse_auth.DTO;

namespace morse_auth.Services
{
    public class AuthenticationService
    {
        private IDbContextFactory<MSDBContext> _contextFactory;

        public AuthenticationService(IDbContextFactory<MSDBContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public UserDTO Register(AuthUserDTO data)
        {
            UserModel newUser = new UserModel()
            {
                Login = data.Login,
                Password = data.Password
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
                Id = createdUser.Id,
                Login = data.Login,
                Password = data.Password
            };
        }

        public UserDTO Login(AuthUserDTO data)
        {
            UserDTO user = new UserDTO();

            using (MSDBContext context = _contextFactory.CreateDbContext())
            {
                UserModel foundUser = context.Users.FirstOrDefault(u => u.Login == data.Login && u.Password == data.Password);
                if (foundUser == null)
                {
                    throw new InvalidDataException("User not found");
                }

                user.Id = foundUser.Id;
                user.Login = foundUser.Login;
                user.DisplayName = foundUser.DisplayName;
            }

            return user;
        }
    }
}
