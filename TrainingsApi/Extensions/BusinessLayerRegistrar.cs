using TrainingsApi.Abstractions;
using TrainingsApi.BLL.Services;
using TrainingsApi.Repositories;

namespace TrainingsApi.Extensions
{
    public static class BusinessLayerRegistrar
    {
        public static void AddBusinessComponents(this IServiceCollection services)
        {
            services.AddScoped<ITrainingRepository, TrainingRepository>();
            services.AddScoped<ITrainingService, TrainingService>();
        }
    }
}
