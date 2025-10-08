namespace TrainingsApi.BLL.Services
{
    public interface ITrainingStateService
    {
        Task StartTrainingAsync(int id, CancellationToken cancellationToken = default);
        Task CompleteTrainingAsync(int id, CancellationToken cancellationToken = default);
        Task CancelTrainingAsync(int id, CancellationToken cancellationToken = default);
    }
}
