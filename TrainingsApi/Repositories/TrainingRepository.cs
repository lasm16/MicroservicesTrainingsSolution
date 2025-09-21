using DataAccess.Models;
using TrainingsApi.BLL;

namespace TrainingsApi.Repositories
{
    public class TrainingRepository(DataAccess.AppContext context) : ITrainingRepository
    {
        public Task<List<Training>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<Training> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public async Task AddAsync(TrainingDto trainingDto, CancellationToken cancellationToken = default)
        {
            var training = new Training
            {
                UserId = trainingDto.UserId,
                Description = trainingDto.Description,
                DurationInMinutes = trainingDto.DurationInMinutes,
                IsCompleted = trainingDto.IsCompleted,
                Date = trainingDto.Date
            };
            context.Trainings.Add(training);
            await context.SaveChangesAsync();
        }

        public Task DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(TrainingDto training, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
