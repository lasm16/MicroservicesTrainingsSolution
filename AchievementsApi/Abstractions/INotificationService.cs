namespace AchievementsApi.Abstractions
{
    public interface INotificationService
    {
        Task AddNotification(string message);
    }
}
