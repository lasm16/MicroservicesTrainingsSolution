﻿using DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using TrainingsApi.BLL;

namespace TrainingsApi.Repositories
{
    public class TrainingRepository(DataAccess.AppContext context) : ITrainingRepository
    {
        public async Task<List<Training>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await context.Trainings
                .Where(t => !t.IsDeleted)
                .ToListAsync(cancellationToken);
        }

        public async Task<Training?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await context.Trainings
                .FirstOrDefaultAsync(t => t.Id == id && !t.IsDeleted, cancellationToken);
        }

        public async Task AddAsync(Training training, CancellationToken cancellationToken = default)
        {
            await context.Trainings.AddAsync(training, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateAsync(Training training, CancellationToken cancellationToken = default)
        {
            context.Trainings.Update(training);
            await context.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteAsync(Training training, CancellationToken cancellationToken = default)
        {
            training.IsDeleted = true;
            training.Updated = DateTime.UtcNow;
            context.Trainings.Update(training);
            await context.SaveChangesAsync(cancellationToken);
        }
    }
}
