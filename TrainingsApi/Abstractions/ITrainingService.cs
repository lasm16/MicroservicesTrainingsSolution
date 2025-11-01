using TrainingsApi.BLL.DTO;

namespace TrainingsApi.Abstractions
{
    public interface ITrainingService
    {
        Task<TrainingDto?> GetByIdAsync(int trainingId, CancellationToken cancellationToken = default);
        Task<List<TrainingDto>> GetAllAsync(int userId, CancellationToken cancellationToken = default);
        Task CreateAsync(TrainingCreateDto dto, CancellationToken cancellationToken = default);
        Task UpdateAsync(TrainingUpdateDto dto, CancellationToken cancellationToken = default);
        Task UpdateStatusAsync(TrainingStatusUpdateDto dto, CancellationToken cancellationToken = default);
        Task DeleteAsync(int trainingId, CancellationToken cancellationToken = default);
    }
}
