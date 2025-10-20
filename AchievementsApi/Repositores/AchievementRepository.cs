using AchievementsApi.Abstractions;
using DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace AchievementsApi.Repositores
{
    public class AchievementRepository(DataAccess.AppContext context) : IAchievementRepository
    {
        private readonly DataAccess.AppContext _context = context;

        public async Task<bool> AddAsync(Achievement achievement, CancellationToken cancellationToken = default)
        {
            achievement.CreatedAt = DateTime.UtcNow;
            await _context.Achievements.AddAsync(achievement, cancellationToken);
            var result = await _context.SaveChangesAsync(cancellationToken);
            return result > 0;
        }

        public async Task<bool> UpdateAsync(Achievement achievement, CancellationToken cancellationToken = default)
        {
            achievement.UpdatedAt = DateTime.UtcNow;
            _context.Achievements.Update(achievement);
            var result = await _context.SaveChangesAsync(cancellationToken);
            return result > 0;
        }

        public async Task<bool> DeleteAsync(Achievement achievement, CancellationToken cancellationToken = default)
        {
            DeleteEntity(achievement);
            _context.Achievements.Update(achievement);
            var result = await _context.SaveChangesAsync(cancellationToken);
            return result > 0;
        }

        public async Task<List<Achievement>> GetAllAsync(int userId, CancellationToken cancellationToken = default)
        {
            return await _context.Achievements
                .Where(x => x.UserId == userId && x.IsDeleted == false)
                .ToListAsync(cancellationToken: cancellationToken);
        }

        public async Task<Achievement?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.Achievements
                .FirstOrDefaultAsync(x => x.Id == id && x.IsDeleted == false, cancellationToken: cancellationToken);
        }

        private static void DeleteEntity(Achievement achievement)
        {
            achievement.IsDeleted = true;
            achievement.UpdatedAt = DateTime.UtcNow;
        }
    }
}
