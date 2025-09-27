using AchievementsApi.BLL.DTO;

namespace AchievementsApi.BLL.Services
{
    public interface IAchievementService
    {
        Task<AchievementDto> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<List<AchievementDto>> GetAllAsync(CancellationToken cancellationToken = default);
        internal Task<AchievementDto> CreateAsync(AchievementDto achievement, CancellationToken cancellationToken = default);
        internal Task<bool> UpdateAsync(AchievementDto achievement, CancellationToken cancellationToken = default);
        internal Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
    }
}
