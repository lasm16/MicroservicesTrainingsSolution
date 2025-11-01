using DataAccess.Enums;
using TrainingsApi.Abstractions;

namespace TrainingsApi.BLL.States
{
    public class TrainingContext(StatusType status)
    {
        public StatusType Status { get; set; }
        public ITrainingState State { get; set; } = MapStatusToState(status);

        private static ITrainingState MapStatusToState(StatusType status)
        {
            return status switch
            {
                StatusType.Planned => new TrainingPlannedState(),
                StatusType.InProgress => new TrainingInProgressState(),
                StatusType.Completed => new TrainingCompletedState(),
                StatusType.Cancelled => new TrainingCancelledState(),
                _ => throw new ArgumentException($"Unknown status: {status}")
            };
        }

        public void Start()
        {
            State.Start(this);
            Status = StatusType.InProgress;
        }

        public void Complete()
        {
            State.Complete(this);
            Status = StatusType.Completed;
        }

        public void Cancel()
        {
            State.Cancel(this);
            Status = StatusType.Cancelled;
        }
    }
}
