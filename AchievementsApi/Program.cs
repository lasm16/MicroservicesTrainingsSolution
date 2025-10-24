using AchievementsApi.Abstractions;
using AchievementsApi.BLL.Services;
using AchievementsApi.Repositores;
using Microsoft.EntityFrameworkCore;
using Commons.HealthChecks;

namespace AchievementsApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            builder.Services.AddOpenApi();
            builder.Services.AddScoped<IAchievementRepository, AchievementRepository>();
            builder.Services.AddScoped<IAchievementService, AchievementService>();
            builder.Services.AddSingleton<INotificationService, NotificationService>();
            builder.Services.AddHostedService<NotificationProcessingService>();
            builder.Services.AddDbContext<DataAccess.AppContext>(x =>
            {
                var configuration = GetConfiguration();
                var configurationString = configuration.GetConnectionString("DefaultConnection");
                x.UseNpgsql(configurationString);
                x.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            });

            builder.Services.AddSingleton(provider =>
            new PostgresHealthCheck(
                builder.Configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string 'Npgsql' not found.")));
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

            app.MapHealthChecks("/health", HealthCheckOptionsFactory.Create("AchievmentsApi"));

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }

        private static IConfigurationRoot GetConfiguration()
        {
            return new ConfigurationBuilder()
                                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                                .AddJsonFile("appsettings.json")
                                .Build();
        }
    }
}
