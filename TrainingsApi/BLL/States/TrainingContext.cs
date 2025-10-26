using DataAccess.Models;

namespace TrainingsApi.BLL.States
{
    public class TrainingContext
    {
        public Training Training { get; set; }
        public ITrainingState State { get; set; }

        public TrainingContext(Training training)
        {
            Training = training;
            State = MapStatusToState(training.Status);
        }
        private ITrainingState MapStatusToState(string status)
        {
            return status switch
            {
                "Planned" => new TrainingPlannedState(),
                "InProgress" => new TrainingInProgressState(),
                "Completed" => new TrainingCompletedState(),
                "Cancelled" => new TrainingCancelledState(),
                null => new TrainingPlannedState(),
                _ => throw new ArgumentException($"Unknown status: {status}")
            };
        }

        public void Start()
        {
            State.Start(this);
            Training.Status = "InProgress";
        }

        public void Complete()
        {
            State.Complete(this);
            Training.Status = "Completed";
        }

        public void Cancel()
        {
            State.Cancel(this);
            Training.Status = "Cancelled";
        }
    }
}
