using TrainingsApi.Properties;

namespace TrainingsApi.Extensions
{
    public static class ConfigurationRegistrar
    {
        public static void AddConfigurations(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<AppSettingsConfig>(configuration.GetSection("AppSettingsConfig"));
        }
    }
}
