using UsersApi.Abstractions;
using UsersApi.BLL.Mapper;
using UsersApi.BLL.Services;
using UsersApi.Repositories;

namespace UsersApi.Extensions
{
    public static class BusinessLayerRegistrar
    {
        public static void AddBusinessComponents(this IServiceCollection services)
        {
            services.AddScoped<UserMapper>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IAchievementsService, AchievementsService>();
            services.AddScoped<INutritionsService, NutritionsService>();
            services.AddScoped<ITrainingsService, TrainingsService>();
            services.AddScoped<IMemoryCacheService, MemoryCacheService>();
        }
    }
}
