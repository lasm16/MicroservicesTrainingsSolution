using NutritionsApi.BLL;
using NutritionsApi.DAL.Models;

namespace NutritionsApi.DAL.Repositories
{
    public class NutritionRepository(AppContext context) : INutritionRepository
    {
        public Task AddAsync(NutritionDto user, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<List<Nutrition>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<Nutrition> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(NutritionDto user, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
