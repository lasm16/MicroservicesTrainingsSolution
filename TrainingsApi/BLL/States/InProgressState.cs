namespace TrainingsApi.BLL.States
{
    public class InProgressState
    {
        public void Start(TrainingContext context)
        {
            throw new InvalidOperationException("Training is already in progress.");
        }

        public void Complete(TrainingContext context)
        {
            context.Training.IsCompleted = true;
            context.State = new CompletedState();
        }

        public void Cancel(TrainingContext context)
        {
            context.State = new CancelledState();
        }
    }
}
