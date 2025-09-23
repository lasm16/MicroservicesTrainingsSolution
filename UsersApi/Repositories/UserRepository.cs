using DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using UsersApi.BLL;

namespace UsersApi.Repositories
{
    public class UserRepository(DataAccess.AppContext context) : IUserRepository
    {
        public async Task<User> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            var user = await context.Users
                .FirstOrDefaultAsync(u => u.Id == id && !u.IsDeleted, cancellationToken);

            return user; 
        }

        public async Task<List<User>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await context.Users
                .Where(u => !u.IsDeleted)
                .ToListAsync(cancellationToken);
        }

        public async Task AddAsync(UserDto userDto, CancellationToken cancellationToken = default)
        {
            var now = DateTime.UtcNow;

            var user = new User
            {
                Name = userDto.Name,
                Surname = userDto.Surname,
                Email = userDto.Email,
                Created = now,
                Updated = now,
                IsDeleted = false
            };

            context.Users.Add(user);
            await context.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateAsync(UserDto userDto, CancellationToken cancellationToken = default)
        {
            var user = await context.Users.FindAsync([userDto.Id], cancellationToken);
            if (user == null || user.IsDeleted) return;

            user.Name = userDto.Name;
            user.Surname = userDto.Surname;
            user.Email = userDto.Email;
            user.Updated = DateTime.UtcNow;

            context.Users.Update(user);
            await context.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            var user = await context.Users.FindAsync([id], cancellationToken);
            if (user != null && !user.IsDeleted)
            {
                user.IsDeleted = true;
                user.Updated = DateTime.UtcNow;

                context.Users.Update(user);
                await context.SaveChangesAsync(cancellationToken);
            }
        }
    }
}
