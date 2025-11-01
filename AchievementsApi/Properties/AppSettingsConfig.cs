using Commons.Config;

namespace AchievementsApi.Properties
{
    public class AppSettingsConfig
    {
        public HealthCheckConfig? HealthCheckConfig { get; set; }
    }

    public class NotificationServiceConfig
    {
        public int ProcessingIntervalSeconds { get; set; }
        public int SemaphoreHandlerCount { get; set; }
    }
}
