using DataAccess.Models;

namespace UsersApi.Abstractions
{
    public interface IUserRepository
    {
        Task<User?> GetByIdAsync(int userId, CancellationToken cancellationToken = default); 
        Task<List<User>> GetAllAsync(CancellationToken cancellationToken = default); 
        Task CreatedAsync(User user, CancellationToken cancellationToken = default); 
        Task UpdateAsync(User user, CancellationToken cancellationToken = default); 
        Task <bool>DeleteAsync(int userId, CancellationToken cancellationToken = default);
    }
}
