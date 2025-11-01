using TrainingsApi.Abstractions;
using TrainingsApi.BLL.DTO;
using TrainingsApi.BLL.Helpers;
using TrainingsApi.BLL.States;

namespace TrainingsApi.BLL.Services
{
    public class TrainingService(ITrainingRepository repository) : ITrainingService
    {
        public async Task<List<TrainingDto>> GetAllAsync(int userId, CancellationToken cancellationToken = default)
        {
            var trainings = await repository.GetAllAsync(userId, cancellationToken);
            if (trainings.Count == 0)
            {
                return [];
            }
            return TrainingMapper.ToDtoList(trainings);
        }

        public async Task<TrainingDto?> GetByIdAsync(int trainingId, CancellationToken cancellationToken = default)
        {
            var training = await repository.GetByIdAsync(trainingId, cancellationToken);
            return training is null ? null : TrainingMapper.ToDto(training);
        }

        public async Task CreateAsync(TrainingCreateDto dto, CancellationToken cancellationToken = default)
        {
            var training = TrainingMapper.ToModel(dto);
            await repository.AddAsync(training, cancellationToken);
        }

        public async Task UpdateAsync(TrainingUpdateDto dto, CancellationToken cancellationToken = default)
        {
            var trainingFromRepo = await repository.GetByIdAsync(dto.Id, cancellationToken)
                ?? throw new KeyNotFoundException($"Training with id {dto.Id} not found");

            var training = TrainingMapper.ToModel(dto);
            trainingFromRepo = training;

            await repository.UpdateAsync(trainingFromRepo, cancellationToken);
        }

        public async Task UpdateStatusAsync(TrainingStatusUpdateDto dto, CancellationToken cancellationToken = default)
        {
            var training = await repository.GetByIdAsync(dto.Id, cancellationToken)
                ?? throw new ArgumentException($"Training with id {dto.Id} not found");
            var context = new TrainingContext(training.Status);
            ChangeStatus(dto, context);

            training.Status = context.Status;

            await repository.UpdateAsync(training, cancellationToken);
        }

        public async Task DeleteAsync(int trainingId, CancellationToken cancellationToken = default)
        {
            var trainig = await repository.GetByIdAsync(trainingId, cancellationToken)
                ?? throw new ArgumentException($"Training with id {trainingId} not found");

            await repository.DeleteAsync(trainig, cancellationToken);
        }

        private static void ChangeStatus(TrainingStatusUpdateDto dto, TrainingContext context)
        {
            switch (dto.Status)
            {
                case (int)DataAccess.Enums.StatusType.Planned:
                    break;
                case (int)DataAccess.Enums.StatusType.InProgress:
                    context.Start();
                    break;
                case (int)DataAccess.Enums.StatusType.Completed:
                    context.Complete();
                    break;
                case (int)DataAccess.Enums.StatusType.Cancelled:
                    context.Cancel();
                    break;

                default:
                    throw new ArgumentException($"Unknown status: {dto.Status}");
            }
        }
    }
}
