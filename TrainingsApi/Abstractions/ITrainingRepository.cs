using DataAccess.Models;

namespace TrainingsApi.Abstractions
{
    public interface ITrainingRepository
    {
        Task<Training?> GetByIdAsync(int trainingId, CancellationToken cancellationToken = default);
        Task<List<Training>> GetAllAsync(int userId, CancellationToken cancellationToken = default);
        Task<bool> AddAsync(Training training, CancellationToken cancellationToken = default);
        Task<bool> UpdateAsync(Training training, CancellationToken cancellationToken = default);
        Task<bool> DeleteAsync(Training training, CancellationToken cancellationToken = default);
    }
}
