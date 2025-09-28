﻿using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using TrainingsApi.BLL.Services;
using TrainingsApi.Repositories;
using UsersApi.BLL.Services;
using UsersApi.Repositories;
using NSwag.AspNetCore;

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
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IUserService, UserService>();

            builder.Services.AddDbContext<DataAccess.AppContext>(x =>
            {
                x.UseNpgsql("UserName=postgres;Password=postgres;Host=localhost;Port=5432;Database=TrainingsDb;");
            });

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

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
    }
}
