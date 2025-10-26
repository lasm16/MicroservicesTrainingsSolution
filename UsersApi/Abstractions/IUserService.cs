using UsersApi.BLL.DTOs;

namespace UsersApi.Abstractions
{
    public interface IUserService
    {
        Task<UserResponse> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<List<UserDto>> GetAllAsync(CancellationToken cancellationToken = default);
        internal Task<UserDto> CreateAsync(UserRequest createUserDTO, CancellationToken cancellationToken = default);
        internal Task <bool> UpdateAsync(UserRequest updateUserDTO, CancellationToken cancellationToken = default);
        internal Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
    }
}
