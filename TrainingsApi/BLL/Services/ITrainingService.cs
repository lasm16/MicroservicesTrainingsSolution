using TrainingsApi.BLL.Dtos;

namespace TrainingsApi.BLL.Services
{
    public interface ITrainingService
    {
        Task<TrainingDto> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<List<TrainingDto>> GetAllAsync(int userId, CancellationToken cancellationToken = default);
        Task CreateAsync(TrainingCreateDto dto, CancellationToken cancellationToken = default);
        Task UpdateAsync(TrainingUpdateDto dto, CancellationToken cancellationToken = default);
        Task UpdateStatusAsync(TrainingStatusUpdateDto dto, CancellationToken cancellationToken = default);
        Task DeleteAsync(TrainingDeleteDto dto, CancellationToken cancellationToken = default);
    }
}
