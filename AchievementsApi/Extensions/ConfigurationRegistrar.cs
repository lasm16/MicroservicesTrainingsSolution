using AchievementsApi.Properties;

namespace AchievementsApi.Extensions
{
    public static class ConfigurationRegistrar
    {
        public static void AddConfigurations(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<AppSettingsConfig>(configuration.GetSection("AppSettingsConfig"));
            services.Configure<NotificationServiceConfig>(configuration.GetSection("AppSettingsConfig:NotificationServiceConfig"));
        }
    }
}
