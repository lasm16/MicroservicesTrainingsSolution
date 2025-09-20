using UsersApi.BLL;
using UsersApi.DAL.Models;

namespace UsersApi.DAL.Repositories
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
