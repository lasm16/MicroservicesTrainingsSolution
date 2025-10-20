using DataAccess.Models;

namespace UsersApi.Repositories
{
    public interface IUserRepository
    {
        Task<User> GetByIdAsync(int id, CancellationToken cancellationToken = default); 
        Task<List<User>> GetAllAsync(CancellationToken cancellationToken = default); 
        Task CreatedAsync(User user, CancellationToken cancellationToken = default); 
        Task UpdateAsync(User user, CancellationToken cancellationToken = default); 
        Task <bool>DeleteAsync(int id, CancellationToken cancellationToken = default);
    }
}
