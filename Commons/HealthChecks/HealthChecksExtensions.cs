using Microsoft.Extensions.DependencyInjection;
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
