using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Http;
using System.Text.Json;

namespace Commons.HealthChecks
{
    public static class HealthCheckOptionsFactory
    {
        public static HealthCheckOptions Create(string serviceName)
        {
            return new HealthCheckOptions
            {
                ResponseWriter = async (context, report) =>
                {
                    var result = new
                    {
                        service = serviceName,
                        status = report.Status.ToString(),
                        timestamp = DateTime.UtcNow,
                        checks = report.Entries.Select(entry => new
                        {
                            name = entry.Key,
                            status = entry.Value.Status.ToString(),
                            description = entry.Value.Description,
                            duration = entry.Value.Duration.TotalMilliseconds + "ms",
                            exception = entry.Value.Exception?.Message
                        })
                    };

                    context.Response.ContentType = "application/json";
                    var json = JsonSerializer.Serialize(result, new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                        WriteIndented = true
                    });
                    await context.Response.WriteAsync(json);
                }
            };
        }
    }
}
