using DataAccess.Models;
using NutritionsApi.BLL.DTO;

namespace NutritionsApi.Abstractions
{
    public interface INutritionRepository
    {
        Task<Nutrition?> GetByIdAsync(int nutritionId, CancellationToken cancellationToken = default);
        Task<List<Nutrition>?> GetAllAsync(int userId, CancellationToken cancellationToken = default);
        Task<bool> CreateAsync(Nutrition model, CancellationToken cancellationToken = default);
        Task<bool> UpdateAsync(Nutrition model, CancellationToken cancellationToken = default);
        Task<bool> DeleteAsync(int nutritionId, CancellationToken cancellationToken = default);
    }
}
