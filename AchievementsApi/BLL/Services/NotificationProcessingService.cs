using AchievementsApi.Abstractions;

namespace AchievementsApi.BLL.Services
{
    public sealed class NotificationProcessingService(INotificationService notificationService) : BackgroundService
    {
        private readonly TimeSpan _processingInterval = TimeSpan.FromSeconds(10);
        private readonly INotificationService _notificationService = notificationService;

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                // Выполняем обработку уведомлений каждые N секунд
                await _notificationService.ProcessNotificationsAsync();
                await Task.Delay(_processingInterval, cancellationToken);
            }
        }
    }
}
