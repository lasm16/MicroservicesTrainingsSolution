using TrainingsApi.Repositories;

namespace TrainingsApi.BLL.Services
{
    public class TrainingService(ITrainingRepository repository) : ITrainingService
    {
        public Task<List<string>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public async Task CreateAsync(int userId, string description, DateTime date, double duration, bool isCompleted, CancellationToken cancellationToken = default)
        {
            var training = new TrainingDto
            {
                UserId = userId,
                Description = description,
                Date = date,
                DurationInMinutes = duration,
                IsCompleted = isCompleted,
            };
            await repository.AddAsync(training);
        }

        public Task DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(int id, int userId, string description, DateTime date, double duration, bool isCompleted, CancellationToken cancellationToken = default)
        {
            var training = new TrainingDto
            {
                UserId = userId,
                Description = description,
                Date = date,
                DurationInMinutes = duration,
                IsCompleted = isCompleted,
            };
            throw new NotImplementedException();
        }
    }
}
