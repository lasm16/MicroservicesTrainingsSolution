using DataAccess.Models;
using TrainingsApi.BLL;

namespace TrainingsApi.Repositories
{
    public interface ITrainingRepository
    {
        Task<Training> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<List<Training>> GetAllAsync(CancellationToken cancellationToken = default);
        Task AddAsync(TrainingDto training, CancellationToken cancellationToken = default);
        Task UpdateAsync(TrainingDto training, CancellationToken cancellationToken = default);
        Task DeleteAsync(int id, CancellationToken cancellationToken = default);
    }
}
