using Commons.Config;
using Commons.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using TrainingsApi.BLL.Services;
using TrainingsApi.Properties;
using TrainingsApi.Repositories;
using UsersApi.BLL.Services;
using UsersApi.Repositories;
using UsersApi.Repositories;

namespace TrainingsApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();

            builder.Services.AddOpenApiDocument(config =>
            {
                config.Title = "Trainings API";
                config.Version = "v1";
                config.Description = "API для управления тренировками и пользователями";
            });

            builder.Services.AddScoped<ITrainingRepository, TrainingRepository>();
            builder.Services.AddScoped<ITrainingService, TrainingService>();

            builder.Services.AddDbContext<DataAccess.AppContext>(x =>
            {
                var connectionString = builder.Configuration.GetConnectionString("Npgsql")
                                       ?? throw new InvalidOperationException("Connection string not found.");
                x.UseNpgsql(connectionString);
            });
            builder.Services.Configure<AppSettingsConfig>(
                builder.Configuration.GetSection("AppSettingsConfig"));
            builder.Services.AddSingleton(provider =>
            {
                var config = provider.GetRequiredService<IOptions<AppSettingsConfig>>().Value;
                var postgresConfig = config.HealthCheckConfig.PostgresHealthCheckConfig;
                var connectionString = builder.Configuration.GetConnectionString("Npgsql")
                    ?? throw new InvalidOperationException("Connection string 'Npgsql' not found.");
                var options = provider.GetRequiredService<IOptions<PostgresHealthCheckConfig>>();
                return new PostgresHealthCheck(connectionString, postgresConfig);
            });
            
            builder.Services.AddHealthChecks()
                            .AddCommonHealthChecks();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseOpenApi();
                app.UseSwaggerUi(settings =>
                {
                    settings.Path = "";
                    settings.DocumentPath = "/swagger/v1/swagger.json";
                });
            }

            app.MapHealthChecks("/health", HealthCheckOptionsFactory.Create("TrainingsApi"));

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
    }
}
