using UsersApi.BLL.DTO;
using UsersApi.BLL.Models;

namespace UsersApi.BLL.Services
{
    public interface IUserService
    {
        Task<UserDto> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<List<UserDto>> GetAllAsync(CancellationToken cancellationToken = default);
        internal Task<bool> CreateAsync(CreateUserDTO createUserDTO, CancellationToken cancellationToken = default);
        internal Task<bool> UpdateAsync(UpdateUserDTO updateUserDTO, CancellationToken cancellationToken = default);
        internal Task DeleteAsync(int id, CancellationToken cancellationToken = default);
    }
}
