using DataAccess.Models;

namespace AchievementsApi.Repositores
{
    public interface IAchievementRepository
    {
        Task<List<Achievement>> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<List<Achievement>> GetAllAsync(CancellationToken cancellationToken = default);
        Task AddAsync(Achievement achievement, CancellationToken cancellationToken = default);
        Task UpdateAsync(Achievement achievement, CancellationToken cancellationToken = default);
        Task DeleteAsync(int id, CancellationToken cancellationToken = default);
    }
}
