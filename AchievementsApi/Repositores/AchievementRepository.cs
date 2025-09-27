using DataAccess.Models;
using Microsoft.EntityFrameworkCore;

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

        public async Task<List<Achievement>> GetByIdAsync(int userId, CancellationToken cancellationToken = default)
        {
            return await context.Achievements
                .Where(x => x.UserId == userId)
                .ToListAsync(cancellationToken: cancellationToken);
        }

        public Task UpdateAsync(Achievement achievement, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
