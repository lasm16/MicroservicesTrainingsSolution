using Microsoft.Extensions.Diagnostics.HealthChecks;
using Npgsql;
using System.Diagnostics;

namespace Commons.HealthChecks
{
    public class PostgresHealthCheck : IHealthCheck
    {
        private readonly string _connectionString;
        private readonly TimeSpan _degradedThreshold = TimeSpan.FromMilliseconds(500);
        private readonly TimeSpan _unhealthyThreshold = TimeSpan.FromMilliseconds(2000);

        public PostgresHealthCheck(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(
            HealthCheckContext context,
            CancellationToken cancellationToken = default)
        {
            var stopwatch = Stopwatch.StartNew();

            try
            {
                await using var connection = new NpgsqlConnection(_connectionString);
                await connection.OpenAsync(cancellationToken);

                await using var command = new NpgsqlCommand("SELECT 1;", connection);
                var result = await command.ExecuteScalarAsync(cancellationToken);

                stopwatch.Stop();
                var responseTime = stopwatch.Elapsed;

                var message = $"PostgreSQL is accessible. Response time: {responseTime.TotalMilliseconds}ms";

                if (responseTime < _degradedThreshold)
                {
                    return HealthCheckResult.Healthy(message);
                }
                else if (responseTime < _unhealthyThreshold)
                {
                    return HealthCheckResult.Degraded(message);
                }
                else
                {
                    return HealthCheckResult.Unhealthy(message);
                }
            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                return HealthCheckResult.Unhealthy(
                    $"PostgreSQL connection failed after {stopwatch.Elapsed.TotalMilliseconds}ms: {ex.Message}", ex);
            }
        }
    }
}
