using DataAccess.Models;
using NutritionsApi.BLL;

namespace NutritionsApi.Repositories
{
    public interface INutritionRepository
    {
        Task<Nutrition> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<List<Nutrition>> GetAllAsync(CancellationToken cancellationToken = default);
        Task AddAsync(NutritionDto user, CancellationToken cancellationToken = default);
        Task UpdateAsync(NutritionDto user, CancellationToken cancellationToken = default);
        Task DeleteAsync(int id, CancellationToken cancellationToken = default);
    }
}
