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
        private readonly Func<string, CancellationToken, Task<TimeSpan>> _connectionTester;

        public PostgresHealthCheck(string connectionString)
            : this(connectionString, TestConnectionReal)
        {
        }

        public PostgresHealthCheck(
            string connectionString,
            Func<string, CancellationToken, Task<TimeSpan>> connectionTester)
        {
            _connectionString = connectionString;
            _connectionTester = connectionTester;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(
            HealthCheckContext context,
            CancellationToken cancellationToken = default)
        {
            var responseTime = await _connectionTester(_connectionString, cancellationToken);

            var message = $"PostgreSQL check completed. Response time: {responseTime.TotalMilliseconds}ms";

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

        private static async Task<TimeSpan> TestConnectionReal(string connectionString, CancellationToken cancellationToken)
        {
            var stopwatch = Stopwatch.StartNew();
            try
            {
                await using var connection = new NpgsqlConnection(connectionString);
                await connection.OpenAsync(cancellationToken);
                await using var command = new NpgsqlCommand("SELECT 1;", connection);
                await command.ExecuteScalarAsync(cancellationToken);
                return stopwatch.Elapsed;
            }
            catch (Exception)
            {
                stopwatch.Stop();
                return TimeSpan.FromMilliseconds(3000);
            }
        }
    }
}
