using Commons.HealthChecks;
using UsersApi.Extensions;

namespace UsersApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDependencies(builder.Configuration);

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
