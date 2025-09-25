using UsersApi.BLL.DTO;
using UsersApi.BLL.Mapper;
using UsersApi.BLL.Models;
using UsersApi.Repositories;

namespace UsersApi.BLL.Services
{
    public class UserService(IUserRepository userRepository) : IUserService
    {
        
        public async Task<UserDto> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            var user = await userRepository.GetByIdAsync(id, cancellationToken);
            if (user == null) return null;

            return UserMapper.ToDto(user); 
        }

        public async Task<List<UserDto>> GetAllAsync(CancellationToken cancellationToken)
        {
            var users = await userRepository.GetAllAsync(cancellationToken);
            return users.Select(u => UserMapper.ToDto(u))
                        .ToList();
        }

        public async Task<UserDto> CreateAsync(CreateUserDTO request, CancellationToken cancellationToken)
        {
            var userDto = MapToUserDto(request.Name, request.Surname, request.Email);

            var userEntity = UserMapper.ToEntity(userDto);

            await userRepository.CreatedAsync(userEntity, cancellationToken);

            return userDto;
        }

        public async Task<bool> UpdateAsync(UserRequest request, CancellationToken cancellationToken)
        {         
            var userDto = MapToUserDto(request.Name, request.Surname, request.Email);
            

            var existingUser = await userRepository.GetByIdAsync(userDto.Id, cancellationToken);
            if (existingUser == null || existingUser.IsDeleted)
                return false;

            UserMapper.UpdateEntity(userDto, existingUser);

            await userRepository.UpdateAsync(existingUser, cancellationToken);

            return true;
        }

        public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            await userRepository.DeleteAsync(id, cancellationToken);
        }
        private UserDto MapToUserDto(string name, string surname, string email)
        {
            return new UserDto
            {
                Name = name,
                Surname = surname,
                Email = email
            };
        }

    }
}
