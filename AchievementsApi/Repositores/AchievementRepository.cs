using DataAccess.Models;

namespace AchievementsApi.Repositores
{
    public class AchievementRepository(DataAccess.AppContext context) : IAchievementRepository
    {
        public Task AddAsync(Achievement achievement, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<List<Achievement>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<Achievement> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(Achievement achievement, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
