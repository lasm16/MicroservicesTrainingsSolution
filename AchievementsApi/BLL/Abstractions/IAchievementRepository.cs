using DataAccess.Models;

namespace AchievementsApi.BLL.Abstractions
{
    public interface IAchievementRepository
    {
        Task<Achievement?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<List<Achievement>> GetAllAsync(int userId, CancellationToken cancellationToken = default);
        Task<bool> AddAsync(Achievement achievement, CancellationToken cancellationToken = default);
        Task<bool> UpdateAsync(Achievement achievement, CancellationToken cancellationToken = default);
        Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
    }
}
