using AchievementsApi.Abstractions;
using DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace AchievementsApi.Repositores
{
    public class AchievementRepository(DataAccess.AppContext context) : IAchievementRepository
    {
        public async Task<bool> AddAsync(Achievement achievement, CancellationToken cancellationToken = default)
        {
            achievement.CreatedAt = DateTime.UtcNow;
            await context.Achievements.AddAsync(achievement, cancellationToken);
            var result = await context.SaveChangesAsync(cancellationToken);
            return result > 0;
        }

        public async Task<bool> UpdateAsync(Achievement achievement, CancellationToken cancellationToken = default)
        {
            achievement.UpdatedAt = DateTime.UtcNow;
            context.Achievements.Update(achievement);
            var result = await context.SaveChangesAsync(cancellationToken);
            return result > 0;
        }

        public async Task<bool> DeleteAsync(Achievement achievement, CancellationToken cancellationToken = default)
        {
            DeleteEntity(achievement);
            context.Achievements.Update(achievement);
            var result = await context.SaveChangesAsync(cancellationToken);
            return result > 0;
        }

        public async Task<List<Achievement>> GetAllAsync(int userId, CancellationToken cancellationToken = default)
        {
            return await context.Achievements
                .Where(x => x.UserId == userId && x.IsDeleted == false)
                .ToListAsync(cancellationToken: cancellationToken);
        }

        public async Task<Achievement?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await context.Achievements
                .FirstOrDefaultAsync(x => x.Id == id && x.IsDeleted == false, cancellationToken: cancellationToken);
        }

        private static void DeleteEntity(Achievement achievement)
        {
            achievement.IsDeleted = true;
            achievement.UpdatedAt = DateTime.UtcNow;
        }
    }
}
