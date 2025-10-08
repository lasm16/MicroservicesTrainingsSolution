namespace TrainingsApi.BLL.States
{
    public class TrainingCompletedState : ITrainingState
    {
        public void Start(TrainingContext context)
        {
            throw new InvalidOperationException("Cannot start a completed training.");
        }

        public void Complete(TrainingContext context)
        {
            throw new InvalidOperationException("Training is already completed.");
        }

        public void Cancel(TrainingContext context)
        {
            throw new InvalidOperationException("Cannot cancel a completed training.");
        }
    }
}
