using Commons.Config;
using Commons.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using NutritionsApi.Properties;

namespace NutritionsApi.Extensions
{
    public static class InfrastructureRegistrar
    {
        public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<DataAccess.AppContext>(x =>
            {
                var connectionString = configuration.GetConnectionString("Npgsql")
                                       ?? throw new InvalidOperationException("Connection string not found.");
                x.UseNpgsql(connectionString);
            });

            services.AddSingleton(provider =>
            {
                var config = provider.GetRequiredService<IOptions<AppSettingsConfig>>().Value;
                var postgresConfig = config.HealthCheckConfig!.PostgresHealthCheckConfig;
                var connectionString = configuration.GetConnectionString("Npgsql")
                    ?? throw new InvalidOperationException("Connection string 'Npgsql' not found.");
                var options = provider.GetRequiredService<IOptions<PostgresHealthCheckConfig>>();
                return new PostgresHealthCheck(connectionString, postgresConfig!);
            });

            services.AddHealthChecks().AddCommonHealthChecks();
        }
    }
}
