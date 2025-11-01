namespace TrainingsApi.Extensions
{
    public static class DependencyConfigurator
    {
        public static void AddDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddControllers();

            services.AddOpenApiDocument(config =>
            {
                config.Title = "Trainings API";
                config.Version = "v1";
                config.Description = "API для управления тренировками и пользователями";
            });

            services.AddConfigurations(configuration);
            services.AddBusinessComponents();
            services.AddInfrastructure(configuration);
        }
    }
}
