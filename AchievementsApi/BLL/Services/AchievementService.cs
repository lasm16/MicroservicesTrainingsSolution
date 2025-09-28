using AchievementsApi.BLL.DTO;
using AchievementsApi.BLL.Helpers;
using AchievementsApi.Repositores;

namespace AchievementsApi.BLL.Services
{
    public class AchievementService(IAchievementRepository achievementRepository) : IAchievementService
    {
        private readonly IAchievementRepository _achievementRepository = achievementRepository;

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
            var entity = AchievementMapper.MapDtoToEntity(request);
            return await _achievementRepository.AddAsync(entity, cancellationToken);
        }

        public Task<bool> DeleteAsync(int id, CancellationToken cancellationToken)
        {
            return _achievementRepository.DeleteAsync(id, cancellationToken);
        }

        public Task<bool> UpdateAsync(AchievementDto achievement, CancellationToken cancellationToken)
        {
            var entity = AchievementMapper.MapDtoToEntity(achievement);
            return _achievementRepository.UpdateAsync(entity, cancellationToken);
        }
    }
}
