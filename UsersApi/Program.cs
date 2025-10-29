using Commons.Config;
using Commons.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using UsersApi.Abstractions;
using UsersApi.BLL.Mapper;
using UsersApi.BLL.Services;
using UsersApi.Listeners;
using UsersApi.Properties;
using UsersApi.Repositories;

namespace UsersApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            builder.Services.AddOpenApi();
            builder.Services.AddScoped<UserMapper>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddSingleton<IDbListener, DbNotificationListener>();
            builder.Services.AddScoped<IAchievementsService, BLL.Services.AchievementsService>();
            builder.Services.AddScoped<INutritionsService, BLL.Services.NutritionsService>();
            builder.Services.AddScoped<ITrainingsService, BLL.Services.TrainingsService>();
            builder.Services.AddHostedService(provider =>
                (DbNotificationListener)provider.GetRequiredService<IDbListener>());
            builder.Services.Configure<AppSettingsConfig>(
                builder.Configuration.GetSection("AppSettingsConfig"));                 
           
            builder.Services.AddHttpClient(HttpClientConfig.AchievementsClient, (serviceProvider, client) =>
            {
                var config = serviceProvider.GetRequiredService<IOptions<AppSettingsConfig>>().Value;
                client.BaseAddress = new Uri(config.AchievementsService!.Address!);
                client.Timeout = TimeSpan.FromMilliseconds(config.AchievementsService.TimeoutMilliseconds);
            });
            builder.Services.AddHttpClient(HttpClientConfig.NutritionsClient, (serviceProvider, client) =>
            {
                var config = serviceProvider.GetRequiredService<IOptions<AppSettingsConfig>>().Value;
                client.BaseAddress = new Uri(config.NutritionsService!.Address!);
                client.Timeout = TimeSpan.FromMilliseconds(config.NutritionsService.TimeoutMilliseconds);
            });
            builder.Services.AddHttpClient(HttpClientConfig.TrainingsClient, (serviceProvider, client) =>
            {
                var config = serviceProvider.GetRequiredService<IOptions<AppSettingsConfig>>().Value;
                client.BaseAddress = new Uri(config.TrainingsService!.Address!);
                client.Timeout = TimeSpan.FromMilliseconds(config.TrainingsService.TimeoutMilliseconds);
            });

            builder.Services.AddDbContext<DataAccess.AppContext>(options =>
                    options.UseNpgsql(builder.Configuration.GetConnectionString("Npgsql")));

            builder.Services.AddSingleton(provider =>
            {
                var connectionString = builder.Configuration.GetConnectionString("Npgsql")
                    ?? throw new InvalidOperationException("Connection string 'Npgsql' not found.");
                var options = provider.GetRequiredService<IOptions<PostgresHealthCheckOptions>>();
                return new PostgresHealthCheck(connectionString, options);
            });

            builder.Services.AddHealthChecks()
                            .AddCommonHealthChecks();                

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.UseSwaggerUi(options =>
                {
                    options.DocumentPath = "openapi/v1.json";
                });
            }

            app.MapHealthChecks("/health", HealthCheckOptionsFactory.Create("UsersApi"));

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
