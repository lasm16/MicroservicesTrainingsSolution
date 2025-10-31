using AchievementsApi.Abstractions;
using AchievementsApi.Properties;
using Microsoft.Extensions.Options;

namespace AchievementsApi.BLL.Services
{
    public sealed class NotificationProcessingService(
        INotificationService notificationService, 
        IOptions<NotificationServiceConfig> options) : BackgroundService
    {
        private readonly int _processingInterval = options.Value.ProcessingIntervalSeconds;

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                await notificationService.ProcessNotificationsAsync();
                var delay = TimeSpan.FromSeconds(_processingInterval);
                await Task.Delay(delay, cancellationToken);
            }
        }
    }
}
