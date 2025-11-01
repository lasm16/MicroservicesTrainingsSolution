using TrainingsApi.Abstractions;

namespace TrainingsApi.BLL.States
{
    public class TrainingPlannedState: ITrainingState
    {
        public void Start(TrainingContext context)
        {
            context.State = new TrainingInProgressState();
        }

        public void Complete(TrainingContext context)
        {
            throw new InvalidOperationException("Cannot complete a planned training.");
        }

        public void Cancel(TrainingContext context)
        {
            context.State = new TrainingCancelledState();
        }
    }
}
