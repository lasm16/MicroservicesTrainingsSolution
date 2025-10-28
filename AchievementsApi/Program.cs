using AchievementsApi.Abstractions;
using AchievementsApi.BLL.Services;
using AchievementsApi.Repositores;
using Commons.Config;
using Commons.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

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
                var configurationString = builder.Configuration.GetConnectionString("DefaultConnection");
                x.UseNpgsql(configurationString);
                x.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            });

            builder.Services.AddSingleton(provider =>
            {
                var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
                var options = provider.GetRequiredService<IOptions<HealthCheckConfig>>();
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
