using AchievementsApi.BLL.Abstractions;
using DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace AchievementsApi.Repositores
{
    public class AchievementRepository(DataAccess.AppContext context) : IAchievementRepository
    {
        private readonly DataAccess.AppContext _context = context;

        public async Task<bool> AddAsync(Achievement achievement, CancellationToken cancellationToken = default)
        {
            var last = await _context.Achievements.OrderBy(x => x.Id).LastOrDefaultAsync(cancellationToken);
            CreateEntity(achievement, last);
            await _context.Achievements.AddAsync(achievement, cancellationToken);
            var result = await _context.SaveChangesAsync(cancellationToken);
            return result > 0;
        }

        public async Task<bool> UpdateAsync(Achievement achievement, CancellationToken cancellationToken = default)
        {
            var entity = await _context.Achievements.FirstOrDefaultAsync(x => x.Id == achievement.Id, cancellationToken);
            if (entity == null)
            {
                Console.WriteLine($"Не найдено достижение с id={achievement.Id}!");
                return false;
            }
            UpdateEntity(achievement, entity);
            _context.Achievements.Update(achievement);
            var result = await _context.SaveChangesAsync(cancellationToken);
            return result > 0;
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            var achievement = await _context.Achievements
                .FirstOrDefaultAsync(x => x.Id == id && x.IsDeleted == false, cancellationToken: cancellationToken);

            if (achievement == null)
            {
                Console.WriteLine($"Не найдено достижение с id={id}!");
                return false;
            }
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

        private static void CreateEntity(Achievement achievement, Achievement? last)
        {
            if (last != null)
            {
                achievement.Id = last.Id + 1;
            }
            else
            {
                achievement.Id = 1;
            }
            achievement.CreatedAt = DateTime.UtcNow;
        }

        private static void DeleteEntity(Achievement achievement)
        {
            achievement.IsDeleted = true;
            achievement.UpdatedAt = DateTime.UtcNow;
        }

        private static void UpdateEntity(Achievement achievement, Achievement entity)
        {
            achievement.UpdatedAt = DateTime.UtcNow;
            achievement.CreatedAt = entity.CreatedAt;
        }
    }
}
