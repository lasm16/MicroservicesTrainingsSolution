using AchievementsApi.Abstractions;
using AchievementsApi.BLL.DTO;
using AchievementsApi.BLL.DTO.Requests;
using AchievementsApi.BLL.Helpers;
using AchievementsApi.BLL.RewardStrategies;

namespace AchievementsApi.BLL.Services
{
    public class AchievementService(
        IAchievementRepository achievementRepository,
        INotificationService notificationService) : IAchievementService
    {
        private IAchievementRewardCalculator? _rewardCalculator;

        public async Task<List<AchievementDto>> GetAllByUserIdAsync(int userId, CancellationToken cancellationToken)
        {
            var achievements = await achievementRepository.GetAllAsync(userId, cancellationToken);
            if (achievements.Count == 0)
            {
                return [];
            }
            return AchievementMapper.MapEntityCollectionToDto(achievements);
        }

        public async Task<AchievementDto?> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            var achievement = await achievementRepository.GetByIdAsync(id, cancellationToken);
            if (achievement == null)
            {
                Console.WriteLine($"Не найдено достижение с id={id}!");
                return null;
            }
            return AchievementMapper.MapEntityToDto(achievement);
        }

        public async Task<bool> CreateAsync(AchievementRequest request, CancellationToken cancellationToken)
        {
            var achievementDto = AchievementMapper.MapRequestToDto((AchievementCreateRequest)request);
            CalculateReward(achievementDto);
            var achievement = AchievementMapper.MapDtoToEntity(achievementDto);
            var result = await achievementRepository.AddAsync(achievement, cancellationToken);
            if (result == true)
            {
                var message = $"Получено новое достижение с наградой: {achievement.Reward}";
                notificationService.AddNotification(message);
            }
            return result;
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken)
        {
            var achievement = await achievementRepository.GetByIdAsync(id, cancellationToken);
            if (achievement == null)
            {
                Console.WriteLine($"Не найдено достижение с id={id}!");
                return false;
            }
            return await achievementRepository.DeleteAsync(achievement, cancellationToken);
        }

        public async Task<bool> UpdateAsync(AchievementRequest request, CancellationToken cancellationToken)
        {
            var achievementFromRequestDto = AchievementMapper.MapRequestToDto((AchievementUpdateRequest)request);
            var achievementFromRepository = await achievementRepository.GetByIdAsync(achievementFromRequestDto.Id, cancellationToken);
            if (achievementFromRepository == null)
            {
                Console.WriteLine($"Не найдено достижение с id={achievementFromRequestDto.Id}!");
                return false;
            }
            CalculateReward(achievementFromRequestDto);
            var achievement = AchievementMapper.MapDtoToEntity(achievementFromRequestDto);
            return await achievementRepository.UpdateAsync(achievement, cancellationToken);
        }

        private void CalculateReward(AchievementDto achievement)
        {
            try
            {
                _rewardCalculator = RewardStrategyFactory.CreateStrategy(achievement.Type);
                _rewardCalculator!.CalculateReward(achievement);
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Не удалось расчитать награду! {ex.Message}");
            }
        }
    }
}
