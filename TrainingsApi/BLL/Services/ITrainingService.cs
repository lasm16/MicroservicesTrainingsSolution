namespace TrainingsApi.BLL.Services
{
    public interface ITrainingService
    {
        Task<string> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<List<string>> GetAllAsync(CancellationToken cancellationToken = default);
        internal Task CreateAsync(int userId, string description, DateTime date, double duration, bool isCompleted, CancellationToken cancellationToken = default);
        internal Task UpdateAsync(int id, int userId, string description, DateTime date, double duration, bool isCompleted, CancellationToken cancellationToken = default);
        internal Task DeleteAsync(int id, CancellationToken cancellationToken = default);
    }
}
