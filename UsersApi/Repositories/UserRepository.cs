using DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using UsersApi.BLL;

namespace UsersApi.Repositories
{
    public class UserRepository(DataAccess.AppContext context) : IUserRepository
    {
        public Task<List<User>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public async Task<User> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await context.Users.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

        public async Task AddAsync(UserDto userDto, CancellationToken cancellationToken = default)
        {
            var user = new User
            {
                Name = userDto.Name,
                Email = userDto.Email,
            };
            context.Users.Add(user);
            await context.SaveChangesAsync(cancellationToken);
        }

        public Task UpdateAsync(UserDto user, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
