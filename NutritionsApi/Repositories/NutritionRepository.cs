using DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using NutritionsApi.Abstractions;

namespace NutritionsApi.Repositories
{
    public class NutritionRepository(DataAccess.AppContext context) : INutritionRepository
    {
        public async Task<Nutrition?> GetByIdAsync(int nutritionId, CancellationToken cancellationToken = default)
        {
            return await context.Nutritions
                .Where(n => n.Id == nutritionId && !n.IsDeleted)
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<List<Nutrition>?> GetAllAsync(int userId, CancellationToken cancellationToken = default)
        {
            var nutritionList = await context.Nutritions
                .Where(nutrition => nutrition.UserId == userId)
                .Where(nutrition => nutrition.IsDeleted == false)
                .ToListAsync(cancellationToken);

            return nutritionList;
        }

        public async Task<bool> CreateAsync(Nutrition model, CancellationToken cancellationToken = default)
        {
            context.Nutritions.Add(model);
            var affectedRows = await context.SaveChangesAsync(cancellationToken);
            
            return affectedRows > 0;
        }

        public async Task<bool> UpdateAsync(Nutrition model, CancellationToken cancellationToken = default)
        {
            context.Nutritions.Update(model);
            var affectedRows = await context.SaveChangesAsync(cancellationToken);
            
            return affectedRows > 0;
        }
        
        public async Task<bool> DeleteAsync(int modelId, CancellationToken cancellationToken = default)
        {
            var model = await context.Nutritions.FindAsync([modelId], cancellationToken);
            if (model is not { IsDeleted: false }) return false;
            model.IsDeleted = true;
            model.Updated = DateTime.UtcNow;

            context.Update(model);
            var affectedRows = await context.SaveChangesAsync(cancellationToken);
                
            return affectedRows > 0;
        }

    }
}
