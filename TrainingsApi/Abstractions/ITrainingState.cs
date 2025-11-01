using TrainingsApi.BLL.States;

namespace TrainingsApi.Abstractions
{
    public interface ITrainingState
    {
        void Start(TrainingContext trainingContext);
        void Cancel(TrainingContext trainingContext);
        void Complete(TrainingContext trainingContext);
    }
}
