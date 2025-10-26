using TrainingsApi.BLL.Dtos;
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

        public async Task<TrainingDto> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            var training = await repository.GetByIdAsync(id, cancellationToken);
            return training is null ? null : TrainingMapper.ToDto(training);
        }

        public async Task CreateAsync(TrainingCreateDto dto, CancellationToken cancellationToken = default)
        {
            var training = TrainingMapper.FromCreateDto(dto);
            await repository.AddAsync(training, cancellationToken);
        }

        public async Task UpdateAsync(TrainingUpdateDto dto, CancellationToken cancellationToken = default)
        {
            var training = await repository.GetByIdAsync(dto.Id, cancellationToken);
            if (training is null)
                throw new KeyNotFoundException($"Training with id {dto.Id} not found");

            var context = new TrainingContext(training);

            // применяем паттерн State
            switch (dto.Status)
            {
                case "Planned":
                    training.Status = "Planned";
                    break;

                case "InProgress":
                    context.Start();
                    break;

                case "Completed":
                    context.Complete();
                    break;

                case "Cancelled":
                    context.Cancel();
                    break;

                default:
                    throw new ArgumentException($"Unknown status: {dto.Status}");
            }

            training.Status = context.Training.Status ?? dto.Status;
            training.Updated = dto.Updated;

            await repository.UpdateAsync(training, cancellationToken);
        }
        
        public async Task UpdateStatusAsync(TrainingStatusUpdateDto dto, CancellationToken cancellationToken = default)
        {
            var training = await repository.GetByIdAsync(dto.Id, cancellationToken);
            if (training is null)
                throw new ArgumentException($"Training with id {dto.Id} not found");

            var context = new TrainingContext(training);

            // применяем паттерн State
            switch (dto.Status)
            {
                case "Planned":
                    training.Status = "Planned";
                    break;

                case "InProgress":
                    context.Start();
                    break;

                case "Completed":
                    context.Complete();
                    break;

                case "Cancelled":
                    context.Cancel();
                    break;

                default:
                    throw new ArgumentException($"Unknown status: {dto.Status}");
            }

            training.Status = context.Training.Status ?? dto.Status;
            training.Updated = dto.Updated;

            await repository.UpdateAsync(training, cancellationToken);
        }

        public async Task DeleteAsync(TrainingDeleteDto dto, CancellationToken cancellationToken = default)
        {
            var entity = await repository.GetByIdAsync(dto.Id, cancellationToken)
                ?? throw new ArgumentException($"Training with id {dto.Id} not found");

            TrainingMapper.MarkDeleted(entity, dto);
            await repository.UpdateAsync(entity, cancellationToken);
        }
    }
}
