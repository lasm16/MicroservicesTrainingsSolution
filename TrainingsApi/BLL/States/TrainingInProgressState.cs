using TrainingsApi.Abstractions;

namespace TrainingsApi.BLL.States
{
    public class TrainingInProgressState: ITrainingState
    {
        public void Start(TrainingContext context)
        {
            throw new InvalidOperationException("Training is already in progress.");
        }

        public void Complete(TrainingContext context)
        {
            context.State = new TrainingCompletedState();
        }

        public void Cancel(TrainingContext context)
        {
            context.State = new TrainingCancelledState();
        }
    }
}
