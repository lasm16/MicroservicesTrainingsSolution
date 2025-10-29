using Microsoft.Extensions.DependencyInjection;
namespace Commons.HealthChecks
{
    public static class HealthChecksExtensions
    {
        public const string RequestTimeCheckName = "request_time_check";
        public const string PostgreSqlConnectionCheckName = "postgresql_connection";

        public static IHealthChecksBuilder AddCommonHealthChecks(this IHealthChecksBuilder builder)
        {
            return builder
                .AddCheck<RequestTimeHealthCheck>(RequestTimeCheckName)
                .AddCheck<PostgresHealthCheck>(PostgreSqlConnectionCheckName);
        }
    }
}
