using Commons;
using Commons.HealthChecks;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using UsersApi.Abstractions;
using UsersApi.BLL.Mapper;
using UsersApi.BLL.Services;
using UsersApi.Listeners;
using UsersApi.Repositories;

namespace UsersApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();
            builder.Services.AddScoped<UserMapper>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IUserService,UserService>();
            builder.Services.AddSingleton<IDbListener, DbNotificationListener>();
            builder.Services.AddHostedService(provider =>
                (DbNotificationListener)provider.GetRequiredService<IDbListener>());
           
            builder.Services.AddDbContext<DataAccess.AppContext>(options =>
                    options.UseNpgsql(builder.Configuration.GetConnectionString("Npgsql")));

            builder.Services.AddSingleton(provider =>
            new PostgresHealthCheck(
                builder.Configuration.GetConnectionString("Npgsql")
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

            app.MapHealthChecks("/health", HealthCheckOptionsFactory.Create("UsersApi"));

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
