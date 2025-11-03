using UsersApi.Abstractions;
using UsersApi.BLL.DTOs;
using UsersApi.BLL.Mapper;

namespace UsersApi.BLL.Services
{
    public class UserService(
        IUserRepository userRepository,
        IAchievementsService achievementsService,
        INutritionsService nutritionsService,
        ITrainingsService trainingsService,
        IMemoryCacheService memoryCacheService) : IUserService
    {
        public async Task<UserResponse?> GetByIdAsync(int userId, CancellationToken cancellationToken = default)
        {
            memoryCacheService.TryGet(userId, out UserResponse? response);
            if (response == null)
            {
                var user = await userRepository.GetByIdAsync(userId, cancellationToken);
                if (user == null) return null;

                response = UserMapper.GetUserResponse(user);
                await GetInfoFromServices(userId, response);
                Console.WriteLine($"Пользователь с Id={user.Id} извлечен из базы данных");
                memoryCacheService.Set(response.Id, response);
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

        private async Task GetInfoFromServices(int userId, UserResponse response)
        {
            var achievementsTask = achievementsService.GetAllAchievements(userId);
            var nutritionsTask = nutritionsService.GetAllNutritions(userId);
            var trainingsTask = trainingsService.GetAllTrainings(userId);

            await Task.WhenAll(achievementsTask, nutritionsTask, trainingsTask);

            response.Achievements = await achievementsTask;
            response.Nutritions = await nutritionsTask;
            response.Trainings = await trainingsTask;
        }
    }
}
