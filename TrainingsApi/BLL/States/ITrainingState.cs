namespace TrainingsApi.BLL.States
{
    public interface ITrainingState
    {
        void Start(TrainingContext trainingContext);
        void Cancel(TrainingContext trainingContext);
        void Complete(TrainingContext trainingContext);
    }
}
