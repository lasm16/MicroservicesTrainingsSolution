using UsersApi.Abstractions;
using UsersApi.BLL.Mapper;
using UsersApi.BLL.DTOs;
using UsersApi.Repositories;

namespace UsersApi.BLL.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IAchievementsService _achievementService;
        private readonly INutritionsService _nutritionService;
        private readonly ITrainingsService _trainingsService;

        public UserService(
            IUserRepository userRepository, 
            IDbListener dbListener, 
            IAchievementsService achievementService,
            INutritionsService nutritionsService,
            ITrainingsService trainingsService)
        {
            _userRepository = userRepository;
            _achievementService = achievementService;
            _nutritionService = nutritionsService;
            _trainingsService = trainingsService;
            dbListener.OnNotificationReceived += OnUserDeleted;
        }

        public async Task<UserResponse> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            var user = await _userRepository.GetByIdAsync(id, cancellationToken);
            if (user == null) return null;
            var response = UserMapper.GetUserResponse(user);
            response.Achievements = await GetAchievementList(id);
            response.Nutritions = await GetNutritionList(id);
            response.Trainings = await GetTrainingsList(id);
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
    }
}
