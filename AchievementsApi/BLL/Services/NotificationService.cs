using AchievementsApi.Abstractions;
using AchievementsApi.BLL.DTO;
using AchievementsApi.Properties;
using Microsoft.Extensions.Options;
using System.Collections.Concurrent;

namespace AchievementsApi.BLL.Services
{
    public class NotificationService : INotificationService
    {
        private readonly int _handlerCount;
        private readonly ConcurrentQueue<Notification> _notificationQueue = new();
        private readonly SemaphoreSlim _semaphore;

        public NotificationService(IOptions<NotificationServiceConfig> options)
        {
            _handlerCount = options.Value.SemaphoreHandlerCount;
            _semaphore = new(_handlerCount);
        }

        public void AddNotification(string message)
        {
            var notification = new Notification
            {
                Message = message,
            };

            Console.WriteLine($"Добавлено уведомление: {message}");
            _notificationQueue.Enqueue(notification);
        }

        public async Task ProcessNotificationsAsync()
        {
            while (_notificationQueue.TryDequeue(out var notification))
            {
                await _semaphore.WaitAsync();

                try
                {
                    await SendNotificationAsync(notification.Message);
                }
                finally
                {
                    _semaphore.Release();
                }
            }
        }

        private static async Task SendNotificationAsync(string message)
        {
            var seconds = new Random().Next(1, 5);  // случайная задержка для симуляции нагрузки
            await Task.Delay(TimeSpan.FromSeconds(seconds));
            Console.WriteLine($"Отправлено уведомление: {message}");
        }
    }
}
