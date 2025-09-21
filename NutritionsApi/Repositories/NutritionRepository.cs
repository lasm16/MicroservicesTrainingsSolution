using DataAccess.Models;
using NutritionsApi.BLL;

namespace NutritionsApi.Repositories
{
    public class NutritionRepository(DataAccess.AppContext context) : INutritionRepository
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
