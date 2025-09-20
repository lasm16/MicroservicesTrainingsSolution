using UsersApi.BLL;
using UsersApi.DAL.Models;

namespace UsersApi.DAL.Repositories
{
    public class UserRepository(AppContext context) : IUserRepository
    {
        public Task<List<User>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<User> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task AddAsync(UserDto user, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
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
