using Commons.HealthChecks;
using UsersApi.Abstractions;
using UsersApi.Factories;
using UsersApi.Properties;
namespace UsersApi.Extensions
{
    public static class HealthChecksExtensions
    {
        private const string RequestAchievementApi = "request_achievement_api";
        private const string RequestTrainingApi = "request_training_api";
        private const string RequestNutritiongApi = "request_nutrition_api";

        public static IHealthChecksBuilder AddHealthChecks(this IHealthChecksBuilder builder, IHealthCheckFactory healthCheckFactory, AppSettingsConfig config)
        {
            var trainingServiceUrl = config.TrainingsService.Address;
            var achievementServiceUrl = config.AchievementsService.Address;
            var nutritionServiceUrl = config.NutritionsService.Address;

            return builder
                .AddCheck(RequestAchievementApi, healthCheckFactory.Create(achievementServiceUrl))
                .AddCheck(RequestTrainingApi, healthCheckFactory.Create(trainingServiceUrl))
                .AddCheck(RequestNutritiongApi, healthCheckFactory.Create(nutritionServiceUrl));
        }
    }
}
