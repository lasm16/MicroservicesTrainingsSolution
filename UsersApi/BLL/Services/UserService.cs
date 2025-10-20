using UsersApi.Abstractions;
using UsersApi.BLL.Mapper;
using UsersApi.BLL.DTOs;
using UsersApi.Repositories;

namespace UsersApi.BLL.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository, IDbListener dbListener)
        {
            _userRepository = userRepository;
            dbListener.OnNotificationReceived += OnUserDeleted;
        }

        public async Task<UserDto> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            var user = await _userRepository.GetByIdAsync(id, cancellationToken);
            if (user == null) return null;

            return UserMapper.ToDto(user); 
        }

        public async Task<List<UserDto>> GetAllAsync(CancellationToken cancellationToken)
        {
            var users = await _userRepository.GetAllAsync(cancellationToken);
            return users.Select(u => UserMapper.ToDto(u))
                        .ToList();
        }

        public async Task<UserDto> CreateAsync(UserRequest request, CancellationToken cancellationToken)
        {
            var userDto = UserMapper.MapToUserDto(request.Id,request.Name, request.Surname, request.Email);

            var userEntity = UserMapper.ToEntity(userDto);

            await _userRepository.CreatedAsync(userEntity, cancellationToken);

            return userDto;
        }

        public async Task<bool> UpdateAsync(UserRequest request, CancellationToken cancellationToken)
        {         
            var userDto = UserMapper.MapToUserDto(request.Id,request.Name, request.Surname, request.Email);
            

            var existingUser = await _userRepository.GetByIdAsync(userDto.Id, cancellationToken);
            if (existingUser == null || existingUser.IsDeleted)
                return false;

            UserMapper.UpdateEntity(userDto, existingUser);

            await _userRepository.UpdateAsync(existingUser, cancellationToken);

            return true;
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            if (id <= 0)
            {
                return false;
            }
            return await _userRepository.DeleteAsync(id, cancellationToken);            
        }
        
        private void OnUserDeleted(object? sender, string e)
        {
            throw new NotImplementedException();
        }
    }
}
