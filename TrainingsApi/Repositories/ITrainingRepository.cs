using DataAccess.Models;
using TrainingsApi.BLL;

namespace TrainingsApi.Repositories
{
    public interface ITrainingRepository
    {
        Task<Training?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<List<Training>> GetAllAsync(CancellationToken cancellationToken = default);
        Task AddAsync(Training training, CancellationToken cancellationToken = default);
        Task UpdateAsync(Training training, CancellationToken cancellationToken = default);
        Task DeleteAsync(Training training, CancellationToken cancellationToken = default);
    }
}
