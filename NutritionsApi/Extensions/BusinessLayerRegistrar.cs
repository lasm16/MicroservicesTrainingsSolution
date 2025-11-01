using NutritionsApi.Abstractions;
using NutritionsApi.BLL.Factories;
using NutritionsApi.BLL.Profiles;
using NutritionsApi.BLL.Services;
using NutritionsApi.Repositories;

namespace NutritionsApi.Extensions
{
    public static class BusinessLayerRegistrar
    {
        public static void AddBusinessComponents(this IServiceCollection services)
        {
            services.AddAutoMapper(cfg =>
            {
                cfg.AddProfile<NutritionProfile>();
                cfg.AddProfile<UpdateNutritionProfile>();
            }, typeof(Program));

            services.AddScoped<INutritionService, NutritionService>();
            services.AddScoped<INutritionRepository, NutritionRepository>();
            services.AddScoped<IDtoFactory, DtoFactory>();
        }
    }
}
