using AchievementsApi.BLL.DTO;
using AchievementsApi.BLL.DTO.Requests;

namespace AchievementsApi.Abstractions
{
    public interface IAchievementService
    {
        Task<AchievementDto?> GetByIdAsync(int achievementId, CancellationToken cancellationToken = default);
        Task<List<AchievementDto>> GetAllByUserIdAsync(int userId, CancellationToken cancellationToken = default);
        Task<bool> CreateAsync(AchievementRequest request, CancellationToken cancellationToken = default);
        Task<bool> UpdateAsync(AchievementRequest request, CancellationToken cancellationToken = default);
        Task<bool> DeleteAsync(int achievementId, CancellationToken cancellationToken = default);
    }
}
