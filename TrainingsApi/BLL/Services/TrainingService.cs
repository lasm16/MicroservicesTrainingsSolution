using TrainingsApi.BLL.Helpers;
using TrainingsApi.BLL.States;
using TrainingsApi.Repositories;

namespace TrainingsApi.BLL.Services
{
    public class TrainingService(ITrainingRepository repository) : ITrainingService
    {
        public async Task<List<TrainingDto>> GetAllAsync(int userId, CancellationToken cancellationToken = default)
        {
            var trainings = await repository.GetAllAsync(userId, cancellationToken);
            return TrainingMapper.ToDtoList(trainings);
        }

        public async Task<TrainingDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            var training = await repository.GetByIdAsync(id, cancellationToken);
            return training is null ? null : TrainingMapper.ToDto(training);
        }

        public async Task CreateAsync(TrainingDto dto, CancellationToken cancellationToken = default)
        {
            if (dto.UserId <= 0)
                throw new ArgumentException("UserId is required");
            if (dto.Date == default)
                throw new ArgumentException("Date is required");
            if (dto.DurationInMinutes <= 0)
                throw new ArgumentException("DurationInMinutes must be greater than 0");

            var training = TrainingMapper.ToEntity(dto);
            await repository.AddAsync(training, cancellationToken);
        }

        public async Task UpdateAsync(TrainingDto dto, CancellationToken cancellationToken = default)
        {
            var training = await repository.GetByIdAsync(dto.Id, cancellationToken);
            if (training is null)
                throw new ArgumentException($"Training with id {dto.Id} not found");

            TrainingMapper.UpdateEntity(training, dto);
            await repository.UpdateAsync(training, cancellationToken);
        }

        public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            var training = await repository.GetByIdAsync(id, cancellationToken);
            if (training is null)
                throw new ArgumentException($"Training with id {id} not found");

            await repository.DeleteAsync(training, cancellationToken);
        }
    }
}
