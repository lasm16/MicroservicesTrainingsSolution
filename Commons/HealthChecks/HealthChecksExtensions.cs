using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
namespace Commons.HealthChecks
{
    public static class HealthChecksExtensions
    {
        public static IHealthChecksBuilder AddCommonHealthChecks(this IHealthChecksBuilder builder)
        {
            return builder
                .AddCheck<RequestTimeHealthCheck>("request_time_check")
                .AddCheck<PostgresHealthCheck>("postgresql_connection");
        }        
    }
}
