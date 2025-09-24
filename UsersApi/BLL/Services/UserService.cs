using UsersApi.BLL.DTO;
using UsersApi.BLL.Mapper;
using UsersApi.BLL.Models;
using UsersApi.Repositories;

namespace UsersApi.BLL.Services
{
    public class UserService(IUserRepository userRepository,IUserMapper userMapper) : IUserService
    {
        private static (string? firstName, string? lastName) ParseName(string fullName)
        {
            if (string.IsNullOrWhiteSpace(fullName)) return (null, null);
            var parts = fullName.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            return parts.Length switch
            {
                0 => (null, null),
                1 => (parts[0], null),
                _ => (parts[0], string.Join(" ", parts[1..]))
            };
        }

        public async Task<UserDto> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            var user = await userRepository.GetByIdAsync(id, cancellationToken);
            if (user == null) return null;

            return userMapper.ToDto(user); 
        }

        public async Task<List<UserDto>> GetAllAsync(CancellationToken ct)
        {
            var users = await userRepository.GetAllAsync(ct);
            return users.Select(u => userMapper.ToDto(u))
                        .ToList();
        }

        public async Task<bool> CreateAsync(CreateUserDTO request, CancellationToken ct)
        {
            var (firstName, lastName) = ParseName(request.Name);

            var userDto = new UserDto
            {
                Name = firstName,
                Surname = lastName,
                Email = request.Email                
            };

            var userEntity = userMapper.ToEntity(userDto);

            await userRepository.CreatedAsync(userEntity, ct);

            return true;
        }

        public async Task<bool> UpdateAsync(UpdateUserDTO request, CancellationToken ct)
        {
            var (firstName, lastName) = ParseName(request.Name);

            var userDto = new UserDto
            {
                Id = request.Id,
                Name = firstName,
                Surname = lastName,
                Email = request.Email
            };

            var existingUser = await userRepository.GetByIdAsync(userDto.Id, ct);
            if (existingUser == null || existingUser.IsDeleted)
                return false;

            userMapper.UpdateEntity(userDto, existingUser);

            await userRepository.UpdateAsync(existingUser, ct);

            return true;
        }

        public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            await userRepository.DeleteAsync(id, cancellationToken);
        }
    }
}
