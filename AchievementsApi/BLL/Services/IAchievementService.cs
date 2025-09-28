using AchievementsApi.BLL.DTO;

namespace AchievementsApi.BLL.Services
{
    public interface IAchievementService
    {
        Task<AchievementDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<List<AchievementDto>> GetAllByUserIdAsync(int userId, CancellationToken cancellationToken = default);
        internal Task<bool> CreateAsync(AchievementRequest request, CancellationToken cancellationToken = default);
        internal Task<bool> UpdateAsync(AchievementDto achievement, CancellationToken cancellationToken = default);
        internal Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
    }
}
