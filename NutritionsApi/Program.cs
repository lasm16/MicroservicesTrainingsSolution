using Microsoft.EntityFrameworkCore;
using NutritionsApi.Abstractions;
using NutritionsApi.BLL.Services;
using NutritionsApi.Middleware;
using NutritionsApi.Repositories;
using AutoMapper;
using NutritionsApi.BLL.Factories;
using NutritionsApi.BLL.Profiles;

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

            var app = builder.Build();
            
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.UseSwaggerUi(options =>
                {
                    options.DocumentPath = "openapi/v1.json";
                });
            }
            
            app.UseMiddleware<ExceptionHandlingMiddleware>();
            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
    }
}