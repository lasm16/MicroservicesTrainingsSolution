using AchievementsApi.BLL.DTO;
using AchievementsApi.Repositores;

namespace AchievementsApi.BLL.Services
{
    public class AchievementService(IAchievementRepository achievementRepository) : IAchievementService
    {
        public Task<List<AchievementDto>> GetAllAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<AchievementDto> GetByIdAsync(int userId, CancellationToken cancellationToken)
        {
            var achievement = await achievementRepository.GetByIdAsync(userId, cancellationToken);

        }
        public Task<AchievementDto> CreateAsync(AchievementDto achievement, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(int id, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateAsync(AchievementDto achievement, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
