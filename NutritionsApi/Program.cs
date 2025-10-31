using Commons.Config;
using Commons.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using NutritionsApi.Abstractions;
using NutritionsApi.BLL.Factories;
using NutritionsApi.BLL.Profiles;
using NutritionsApi.BLL.Services;
using NutritionsApi.Middleware;
using NutritionsApi.Properties;
using NutritionsApi.Repositories;

namespace NutritionsApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            builder.Services.AddOpenApi();
            
            builder.Services.AddAutoMapper(cfg =>
            {
                cfg.AddProfile<NutritionProfile>();
                cfg.AddProfile<UpdateNutritionProfile>();
            }, typeof(Program));
            
            builder.Services.AddScoped<INutritionService, NutritionService>();
            builder.Services.AddScoped<INutritionRepository, NutritionRepository>();
            builder.Services.AddScoped<IDtoFactory, DtoFactory>();
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
                app.MapOpenApi();
                app.UseSwaggerUi(options =>
                {
                    options.DocumentPath = "openapi/v1.json";
                });
            }

            app.MapHealthChecks("/health", HealthCheckOptionsFactory.Create("NutritionsApi"));

            app.UseMiddleware<ExceptionHandlingMiddleware>();
            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
    }
}