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
                "Planned" => new PlannedState(),
                "InProgress" => new InProgressState(),
                "Completed" => new CompletedState(),
                "Cancelled" => new CancelledState(),
                _ => throw new ArgumentException($"Unknown status: {status}")
            };
        }

        public void Start() => State.Start(this);
        public void Complete() => State.Complete(this);
        public void Cancel() => State.Cancel(this);
    }
}
