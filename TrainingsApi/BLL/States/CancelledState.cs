namespace TrainingsApi.BLL.States
{
    public class CancelledState: ITrainingState
    {
        public void Start(TrainingContext context)
        {
            throw new InvalidOperationException("Cannot start a cancelled training.");
        }

        public void Complete(TrainingContext context)
        {
            throw new InvalidOperationException("Cannot complete a cancelled training.");
        }

        public void Cancel(TrainingContext context)
        {
            throw new InvalidOperationException("Training is already cancelled.");
        }
    }
}
