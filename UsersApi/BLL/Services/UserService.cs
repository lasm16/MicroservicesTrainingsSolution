using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using UsersApi.Abstractions;
using UsersApi.BLL.DTOs;
using UsersApi.BLL.Mapper;
using UsersApi.Properties;

namespace UsersApi.BLL.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IAchievementsService _achievementService;
        private readonly INutritionsService _nutritionService;
        private readonly ITrainingsService _trainingsService;
        private readonly IMemoryCache _memoryCache;
        private readonly AppSettingsConfig _settingsConfig;

        public UserService(
            IUserRepository userRepository,
            IDbListener dbListener,
            IAchievementsService achievementService,
            INutritionsService nutritionsService,
            ITrainingsService trainingsService,
            IMemoryCache memoryCache,
            IOptions<AppSettingsConfig> settingsConfig)
        {
            _userRepository = userRepository;
            _achievementService = achievementService;
            _nutritionService = nutritionsService;
            _trainingsService = trainingsService;
            _memoryCache = memoryCache;
            _settingsConfig = settingsConfig.Value;
            dbListener.OnNotificationReceived += OnUserDeleted;
        }

        public async Task<UserResponse?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            _memoryCache.TryGetValue(id, out UserResponse? response);
            if (response == null)
            {
                var user = await _userRepository.GetByIdAsync(id, cancellationToken);
                if (user == null) return null;

                response = UserMapper.GetUserResponse(user);
                response.Achievements = await GetAchievementList(id);
                response.Nutritions = await GetNutritionList(id);
                response.Trainings = await GetTrainingsList(id);
                Console.WriteLine($"Пользователь с Id={user.Id} извлечен из базы данных");
                var options = GetMemoryCacheOptions();
                _memoryCache.Set(response.Id, response, options);
            }
            else
            {
                Console.WriteLine($"Пользователь с Id={response!.Id} извлечен из кэша");
            }
            return response;
        }

        public async Task<List<UserDto>> GetAllAsync(CancellationToken cancellationToken)
        {
            var users = await _userRepository.GetAllAsync(cancellationToken);
            return [.. users.Select(u => UserMapper.ToDto(u))];
        }

        public async Task<UserDto> CreateAsync(UserRequest request, CancellationToken cancellationToken)
        {

            var existingUser = await _userRepository.GetByIdAsync(request.Id, cancellationToken);
            if (existingUser != null)
            {
                throw new InvalidOperationException($"Пользователь с Id = {request.Id} уже существует.");
            }
            var userDto = UserMapper.MapToUserDto(request);

            var userEntity = UserMapper.ToEntity(userDto);

            await _userRepository.CreatedAsync(userEntity, cancellationToken);

            return userDto;
        }

        public async Task<bool> UpdateAsync(UserRequest request, CancellationToken cancellationToken)
        {
            var userDto = UserMapper.MapToUserDto(request);


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

        private async Task<List<AchievementDto>> GetAchievementList(int id)
        {
            return await _achievementService.GetAllAchievements(id);
        }

        private async Task<List<NutritionDto>> GetNutritionList(int id)
        {
            return await _nutritionService.GetAllNutritions(id);
        }

        private async Task<List<TrainingDto>> GetTrainingsList(int id)
        {
            return await _trainingsService.GetAllTrainings(id);
        }

        private MemoryCacheEntryOptions GetMemoryCacheOptions()
        {
            var expirationTime = TimeSpan.FromSeconds(_settingsConfig.CacheSettings!.AbsoluteExpirationFromSeconds);
            return new MemoryCacheEntryOptions().SetAbsoluteExpiration(expirationTime);
        }
    }
}
