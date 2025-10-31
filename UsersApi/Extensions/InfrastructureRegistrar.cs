using Commons.Config;
using Commons.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using UsersApi.Abstractions;
using UsersApi.Factories;
using UsersApi.Properties;

namespace UsersApi.Extensions
{
    public static class InfrastructureRegistrar
    {
        public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<DataAccess.AppContext>(options =>
                    options.UseNpgsql(configuration.GetConnectionString("Npgsql")));

            services.AddSingleton(provider =>
            {
                var config = provider.GetRequiredService<IOptions<AppSettingsConfig>>().Value;
                var postgresConfig = config.HealthCheckConfig!.PostgresHealthCheckConfig;
                var connectionString = configuration.GetConnectionString("Npgsql")
                    ?? throw new InvalidOperationException("Connection string 'Npgsql' not found.");
                var options = provider.GetRequiredService<IOptions<PostgresHealthCheckConfig>>();
                return new PostgresHealthCheck(connectionString, postgresConfig!);
            });

            RegisterHttpClients(services);
            services.AddSingleton<IHealthCheckFactory, RequestHealthCheckFactory>();
            var factory = services.BuildServiceProvider().GetRequiredService<IHealthCheckFactory>();
            var config = services.BuildServiceProvider().GetRequiredService<IOptions<AppSettingsConfig>>().Value;
            services.AddHealthChecks().AddHealthChecks(factory, config);
            services.AddHealthChecks().AddCommonHealthChecks();
            services.AddMemoryCache();
            
        }

        private static void RegisterHttpClients(IServiceCollection services)
        {
            services.AddHttpClient(HttpClientConfig.AchievementsClient, (serviceProvider, client) =>
            {
                var config = serviceProvider.GetRequiredService<IOptions<AppSettingsConfig>>().Value;
                client.BaseAddress = new Uri(config.AchievementsService!.Address!);
                client.Timeout = TimeSpan.FromMilliseconds(config.AchievementsService.TimeoutMilliseconds);
            });

            services.AddHttpClient(HttpClientConfig.NutritionsClient, (serviceProvider, client) =>
            {
                var config = serviceProvider.GetRequiredService<IOptions<AppSettingsConfig>>().Value;
                client.BaseAddress = new Uri(config.NutritionsService!.Address!);
                client.Timeout = TimeSpan.FromMilliseconds(config.NutritionsService.TimeoutMilliseconds);
            });

            services.AddHttpClient(HttpClientConfig.TrainingsClient, (serviceProvider, client) =>
            {
                var config = serviceProvider.GetRequiredService<IOptions<AppSettingsConfig>>().Value;
                client.BaseAddress = new Uri(config.TrainingsService!.Address!);
                client.Timeout = TimeSpan.FromMilliseconds(config.TrainingsService.TimeoutMilliseconds);
            });
        }
    }
}
