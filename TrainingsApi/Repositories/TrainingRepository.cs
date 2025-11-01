using DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using TrainingsApi.Abstractions;
namespace TrainingsApi.Repositories
{
    public class TrainingRepository(DataAccess.AppContext context) : ITrainingRepository
    {
        public async Task<List<Training>> GetAllAsync(int userId, CancellationToken cancellationToken = default)
        {
            return await context.Trainings
                .Where(t => t.UserId == userId && !t.IsDeleted)
                .ToListAsync(cancellationToken);
        }

        public async Task<Training?> GetByIdAsync(int trainingId, CancellationToken cancellationToken = default)
        {
            return await context.Trainings
                .FirstOrDefaultAsync(t => t.Id == trainingId && !t.IsDeleted, cancellationToken);
        }

        public async Task<bool> AddAsync(Training training, CancellationToken cancellationToken = default)
        {
            training.Created = DateTime.UtcNow;
            await context.Trainings.AddAsync(training, cancellationToken);
            var result = await context.SaveChangesAsync(cancellationToken);
            return result > 0;
        }

        public async Task<bool> UpdateAsync(Training training, CancellationToken cancellationToken = default)
        {
            training.Updated = DateTime.UtcNow;
            context.Trainings.Update(training);
            var result = await context.SaveChangesAsync(cancellationToken);
            return result > 0;
        }

        public async Task<bool> DeleteAsync(Training training, CancellationToken cancellationToken = default)
        {
            context.Trainings.Update(training);
            var result = await context.SaveChangesAsync(cancellationToken);
            return result > 0;
        }
    }
}
