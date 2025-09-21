using NutritionsApi.Repositories;

namespace NutritionsApi.BLL.Services
{
    public class NutritionService(INutritionRepository nutritionRepository) : INutritionService
    {
        public Task<List<string>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task CreateAsync(int userId, string description, double calories, CancellationToken cancellationToken = default)
        {
            var nutrition = new NutritionDto
            {
                UserId = userId,
                Description = description,
                Calories = calories,
            };
            throw new NotImplementedException();
        }

        public Task DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(int id, int userId, string description, double calories, CancellationToken cancellationToken = default)
        {
            var user = new NutritionDto
            {
                UserId = userId,
                Description = description,
                Calories = calories
            };
            throw new NotImplementedException();
        }
    }
}
