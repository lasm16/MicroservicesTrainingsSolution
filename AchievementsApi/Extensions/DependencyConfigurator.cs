using Commons.HealthChecks;

namespace AchievementsApi.Extensions
{
    public static class DependencyConfigurator
    {
        public static void AddDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddControllers();
            services.AddOpenApi();
            services.AddConfigurations(configuration);
            services.AddBusinessComponents();
            services.AddInfrastructure(configuration);
        }
    }
}
