namespace TrainingsApi.BLL.States
{
    public class InProgressState: ITrainingState
    {
        public void Start(TrainingContext context)
        {
            throw new InvalidOperationException("Training is already in progress.");
        }

        public void Complete(TrainingContext context)
        {
            context.State = new CompletedState();
        }

        public void Cancel(TrainingContext context)
        {
            context.State = new CancelledState();
        }
    }
}
