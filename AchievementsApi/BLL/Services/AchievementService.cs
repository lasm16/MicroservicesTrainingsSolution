using AchievementsApi.BLL.Abstractions;
using AchievementsApi.BLL.DTO;
using AchievementsApi.BLL.DTO.Requests;
using AchievementsApi.BLL.Helpers;
using AchievementsApi.BLL.RewardStrategies;

namespace AchievementsApi.BLL.Services
{
    public class AchievementService(IAchievementRepository achievementRepository) : IAchievementService
    {
        private readonly IAchievementRepository _achievementRepository = achievementRepository;
        private IAchievementRewardCalculator? _rewardCalculator;

        public async Task<List<AchievementDto>> GetAllByUserIdAsync(int userId, CancellationToken cancellationToken)
        {
            var achievements = await _achievementRepository.GetAllAsync(userId, cancellationToken);
            if (achievements.Count == 0)
            {
                return [];
            }
            return AchievementMapper.MapEntityCollectionToDto(achievements);
        }

        public async Task<AchievementDto?> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            var achievement = await _achievementRepository.GetByIdAsync(id, cancellationToken);
            if (achievement == null)
            {
                Console.WriteLine($"Не найдено достижение с id={id}!");
                return null;
            }
            return AchievementMapper.MapEntityToDto(achievement);
        }
        public async Task<bool> CreateAsync(AchievementRequest request, CancellationToken cancellationToken)
        {
            var achievement = AchievementMapper.MapDtoToEntity((AchievementCreateRequest)request);
            _rewardCalculator = AchievementsStrategyFactory.CreateStrategy(achievement.Type);
            _rewardCalculator!.CalculateReward(achievement);
            return await _achievementRepository.AddAsync(achievement, cancellationToken);
        }

        public Task<bool> DeleteAsync(int id, CancellationToken cancellationToken)
        {
            return _achievementRepository.DeleteAsync(id, cancellationToken);
        }

        public Task<bool> UpdateAsync(AchievementRequest request, CancellationToken cancellationToken)
        {
            var achievement = AchievementMapper.MapDtoToEntity((AchievementUpdateRequest)request);
            _rewardCalculator = AchievementsStrategyFactory.CreateStrategy(achievement.Type);
            _rewardCalculator!.CalculateReward(achievement);
            return _achievementRepository.UpdateAsync(achievement, cancellationToken);
        }
    }
}
