using DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using UsersApi.Abstractions;

namespace UsersApi.Repositories
{
    public class UserRepository(DataAccess.AppContext context) : IUserRepository
    {
        public async Task<User?> GetByIdAsync(int userId, CancellationToken cancellationToken = default)
        {
            return await context.Users
                .FirstOrDefaultAsync(u => u.Id == userId && !u.IsDeleted, cancellationToken);
        }

        public async Task<List<User>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await context.Users
                .Where(u => !u.IsDeleted)
                .ToListAsync(cancellationToken);
        }

        public async Task<bool> CreatedAsync(User user, CancellationToken cancellationToken = default)
        {
            user.Created = DateTime.UtcNow;
            context.Users.Add(user);
            var result = await context.SaveChangesAsync(cancellationToken);
            return result > 0;
        }

        public async Task<bool> UpdateAsync(User user, CancellationToken cancellationToken = default)
        {
            user.Updated = DateTime.UtcNow;
            context.Users.Update(user);
            var result = await context.SaveChangesAsync(cancellationToken);
            return result > 0;
        }

        public async Task<bool> DeleteAsync(int userId, CancellationToken cancellationToken = default)
        {
            var user = await context.Users
                .FirstOrDefaultAsync(x => x.Id == userId && x.IsDeleted == false, cancellationToken);

            if (user != null)
            {
                user.IsDeleted = true;
                user.Updated = DateTime.UtcNow;

                context.Users.Update(user);
                var result = await context.SaveChangesAsync(cancellationToken);
                return result > 0;
            }
            return false;
        }
    }
}
