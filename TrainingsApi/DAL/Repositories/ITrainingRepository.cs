using TrainingsApi.BLL;
using TrainingsApi.DAL.Models;

namespace TrainingsApi.DAL.Repositories
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
