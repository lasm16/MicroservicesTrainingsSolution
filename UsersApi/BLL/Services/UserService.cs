using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using UsersApi.Abstractions;
using UsersApi.BLL.DTOs;
using UsersApi.BLL.Mapper;
using UsersApi.Properties;

namespace UsersApi.BLL.Services
{
    public class UserService(
        IUserRepository userRepository,
        IAchievementsService achievementService,
        INutritionsService nutritionsService,
        ITrainingsService trainingsService,
        IMemoryCache memoryCache,
        IOptions<AppSettingsConfig> settingsConfig) : IUserService
    {
        private readonly AppSettingsConfig _settingsConfig = settingsConfig.Value;

        public async Task<UserResponse?> GetByIdAsync(int userId, CancellationToken cancellationToken = default)
        {
            memoryCache.TryGetValue(userId, out UserResponse? response);
            if (response == null)
            {
                var user = await userRepository.GetByIdAsync(userId, cancellationToken);
                if (user == null) return null;

                response = UserMapper.GetUserResponse(user);
                response.Achievements = await achievementService.GetAllAchievements(userId);
                response.Nutritions = await nutritionsService.GetAllNutritions(userId);
                response.Trainings = await trainingsService.GetAllTrainings(userId);
                Console.WriteLine($"Пользователь с Id={user.Id} извлечен из базы данных");
                var options = GetMemoryCacheOptions();
                memoryCache.Set(response.Id, response, options);
            }
            else
            {
                Console.WriteLine($"Пользователь с Id={response!.Id} извлечен из кэша");
            }
            return response;
        }

        public async Task<List<UserDto>> GetAllAsync(CancellationToken cancellationToken)
        {
            var users = await userRepository.GetAllAsync(cancellationToken);
            return [.. users.Select(u => UserMapper.ToDto(u))];
        }

        public async Task<UserDto> CreateAsync(UserRequest request, CancellationToken cancellationToken)
        {

            var existingUser = await userRepository.GetByIdAsync(request.Id, cancellationToken);
            if (existingUser != null)
            {
                throw new InvalidOperationException($"Пользователь с Id = {request.Id} уже существует.");
            }
            var userDto = UserMapper.MapToUserDto(request);

            var userEntity = UserMapper.ToEntity(userDto);

            await userRepository.CreatedAsync(userEntity, cancellationToken);

            return userDto;
        }

        public async Task<bool> UpdateAsync(UserRequest request, CancellationToken cancellationToken)
        {
            var userDto = UserMapper.MapToUserDto(request);


            var existingUser = await userRepository.GetByIdAsync(userDto.Id, cancellationToken);
            if (existingUser == null || existingUser.IsDeleted)
                return false;

            UserMapper.UpdateEntity(userDto, existingUser);

            await userRepository.UpdateAsync(existingUser, cancellationToken);

            return true;
        }

        public async Task<bool> DeleteAsync(int userId, CancellationToken cancellationToken = default)
        {
            if (userId <= 0)
            {
                return false;
            }
            return await userRepository.DeleteAsync(userId, cancellationToken);
        }

        private MemoryCacheEntryOptions GetMemoryCacheOptions()
        {
            var expirationTime = TimeSpan.FromSeconds(_settingsConfig.CacheSettings!.AbsoluteExpirationFromSeconds);
            return new MemoryCacheEntryOptions().SetAbsoluteExpiration(expirationTime);
        }
    }
}
