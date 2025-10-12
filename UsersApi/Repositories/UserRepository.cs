using DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace UsersApi.Repositories
{
    public class UserRepository(DataAccess.AppContext context) : IUserRepository
    {
        public async Task<User?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await context.Users
                .FirstOrDefaultAsync(u => u.Id == id && !u.IsDeleted, cancellationToken);
        }

        public async Task<List<User>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await context.Users
                .Where(u => !u.IsDeleted)
                .ToListAsync(cancellationToken);
        }

        public async Task CreatedAsync(User user, CancellationToken cancellationToken = default)
        {            
            
            context.Users.Add(user);
            user.Created = DateTime.UtcNow;
            await context.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateAsync(User user, CancellationToken cancellationToken = default)
        {
            context.Users.Update(user);
            user.Updated=DateTime.UtcNow;
            await context.SaveChangesAsync(cancellationToken);
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            var user = await context.Users.FindAsync([id], cancellationToken);
            if (user != null && !user.IsDeleted)
            {
                user.IsDeleted = true;
                user.Updated = DateTime.UtcNow;

                context.Users.Update(user);
                await context.SaveChangesAsync(cancellationToken);
                return true;
            }
            return false;
        }
    }
}
