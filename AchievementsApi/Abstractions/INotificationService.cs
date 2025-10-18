namespace AchievementsApi.Abstractions
{
    public interface INotificationService
    {
        void AddNotification(string message);
        Task ProcessNotificationsAsync();
    }
}
