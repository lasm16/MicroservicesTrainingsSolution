using DataAccess.Models;
using UsersApi.BLL;

namespace UsersApi.Repositories
{
    public interface IUserRepository
    {
        Task<User> GetByIdAsync(int id, CancellationToken cancellationToken = default); 
        Task<List<User>> GetAllAsync(CancellationToken cancellationToken = default); 
        Task AddAsync(UserDto user, CancellationToken cancellationToken = default); 
        Task UpdateAsync(UserDto user, CancellationToken cancellationToken = default); 
        Task DeleteAsync(int id, CancellationToken cancellationToken = default);
    }
}
