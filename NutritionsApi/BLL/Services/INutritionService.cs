namespace NutritionsApi.BLL.Services
{
    public interface INutritionService
    {
        Task<string> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<List<string>> GetAllAsync(CancellationToken cancellationToken = default);
        internal Task CreateAsync(int userId, string description, double calories, CancellationToken cancellationToken = default);
        internal Task UpdateAsync(int id, int userId, string description, double calories, CancellationToken cancellationToken = default);
        internal Task DeleteAsync(int id, CancellationToken cancellationToken = default);
    }
}
