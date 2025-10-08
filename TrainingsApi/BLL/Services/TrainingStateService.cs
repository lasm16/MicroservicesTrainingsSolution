
using TrainingsApi.BLL.States;
using TrainingsApi.Repositories;

namespace TrainingsApi.BLL.Services
{
    public class TrainingStateService(ITrainingRepository repository) : ITrainingStateService
    {
        public async Task StartTrainingAsync(int id, CancellationToken cancellationToken = default)
        {
            var training = await repository.GetByIdAsync(id, cancellationToken);
            if (training is null)
                throw new ArgumentException($"Training with id {id} not found");

            var context = new TrainingContext(training);
            context.Start();

            training.Status = GetStatusFromState(context.State);
            await repository.UpdateAsync(training, cancellationToken);
        }

        public async Task CompleteTrainingAsync(int id, CancellationToken cancellationToken = default)
        {
            var training = await repository.GetByIdAsync(id, cancellationToken);
            if (training is null)
                throw new ArgumentException($"Training with id {id} not found");

            var context = new TrainingContext(training);
            context.Complete();

            training.Status = GetStatusFromState(context.State);
            await repository.UpdateAsync(training, cancellationToken);
        }

        public async Task CancelTrainingAsync(int id, CancellationToken cancellationToken = default)
        {
            var training = await repository.GetByIdAsync(id, cancellationToken);
            if (training is null)
                throw new ArgumentException($"Training with id {id} not found");

            var context = new TrainingContext(training);
            context.Cancel();

            training.Status = GetStatusFromState(context.State);
            await repository.UpdateAsync(training, cancellationToken);
        }
        private string GetStatusFromState(ITrainingState state)
        {
            return state switch
            {
                TrainingPlannedState => "Planned",
                TrainingInProgressState => "InProgress",
                TrainingCompletedState => "Completed",
                TrainingCancelledState => "Cancelled",
                _ => throw new ArgumentException("Unknown state")
            };
        }
    }
}
