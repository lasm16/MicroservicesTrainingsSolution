using AchievementsApi.Properties;
using Commons.Config;
using Commons.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace AchievementsApi.Extensions
{
    public static class InfrastructureRegistrar
    {
        public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<DataAccess.AppContext>(x =>
            {
                var configurationString = configuration.GetConnectionString("DefaultConnection");
                x.UseNpgsql(configurationString);
                x.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            });

            services.AddSingleton(provider =>
            {
                var config = provider.GetRequiredService<IOptions<AppSettingsConfig>>().Value;
                var postgresConfig = config.HealthCheckConfig!.PostgresHealthCheckConfig;
                var connectionString = configuration.GetConnectionString("DefaultConnection")
                    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
                var options = provider.GetRequiredService<IOptions<PostgresHealthCheckConfig>>();
                return new PostgresHealthCheck(connectionString, postgresConfig!);
            });

            services.AddHealthChecks().AddCommonHealthChecks();
        }
    }
}
