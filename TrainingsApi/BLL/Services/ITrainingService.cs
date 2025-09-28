namespace TrainingsApi.BLL.Services
{
    public interface ITrainingService
    {
        Task<TrainingDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<List<TrainingDto>> GetAllAsync(CancellationToken cancellationToken = default);
        Task CreateAsync(TrainingDto dto, CancellationToken cancellationToken = default);
        Task UpdateAsync(TrainingDto dto, CancellationToken cancellationToken = default);
        Task DeleteAsync(int id, CancellationToken cancellationToken = default);
    }
}
