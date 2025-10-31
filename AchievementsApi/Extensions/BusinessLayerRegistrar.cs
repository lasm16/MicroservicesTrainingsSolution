using AchievementsApi.Abstractions;
using AchievementsApi.BLL.Services;
using AchievementsApi.Repositores;

namespace AchievementsApi.Extensions
{
    public static class BusinessLayerRegistrar
    {
        public static void AddBusinessComponents(this IServiceCollection services)
        {
            services.AddScoped<IAchievementRepository, AchievementRepository>();
            services.AddScoped<IAchievementService, AchievementService>();
            services.AddSingleton<INotificationService, NotificationService>();
            services.AddHostedService<NotificationProcessingService>();
        }
    }
}
