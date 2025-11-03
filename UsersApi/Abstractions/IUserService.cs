using UsersApi.BLL.DTOs;

namespace UsersApi.Abstractions
{
    public interface IUserService
    {
        Task<UserResponse?> GetByIdAsync(int userId, CancellationToken cancellationToken = default);
        Task<List<UserDto>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<bool> CreateAsync(UserRequest userRequest, CancellationToken cancellationToken = default);
        Task<bool> UpdateAsync(UserRequest userRequest, CancellationToken cancellationToken = default);
        Task<bool> DeleteAsync(int userId, CancellationToken cancellationToken = default);
    }
}
