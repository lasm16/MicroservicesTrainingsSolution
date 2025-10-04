namespace TrainingsApi.BLL.States
{
    public class PlannedState
    {
        public void Start(TrainingContext context)
        {
            context.Training.IsCompleted = false;
            context.State = new InProgressState();
        }

        public void Complete(TrainingContext context)
        {
            throw new InvalidOperationException("Cannot complete a planned training.");
        }

        public void Cancel(TrainingContext context)
        {
            context.State = new CancelledState();
        }
    }
}
