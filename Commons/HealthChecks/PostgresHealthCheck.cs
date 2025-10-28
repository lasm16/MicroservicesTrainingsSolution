using Commons.Config;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;
using Npgsql;
using System.Diagnostics;

namespace Commons.HealthChecks
{
    public class PostgresHealthCheck : IHealthCheck
    {
        private readonly string _connectionString;
        private readonly TimeSpan _degradedThreshold;
        private readonly TimeSpan _unhealthyThreshold;
        private readonly Func<string, CancellationToken, Task<TimeSpan>> _connectionTester;

        public PostgresHealthCheck(string connectionString, IOptions<HealthCheckConfig> options)
            : this(connectionString, options, TestConnection)
        {
        }

        public PostgresHealthCheck(
            string connectionString,
            IOptions<HealthCheckConfig> options,
            Func<string, CancellationToken, Task<TimeSpan>> connectionTester)
        {
            _connectionString = connectionString;
            _degradedThreshold = options.Value.DegradedThreshold;
            _unhealthyThreshold = options.Value.UnhealthyThreshold;
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

        private static async Task<TimeSpan> TestConnection(string connectionString, CancellationToken cancellationToken)
        {
            var stopwatch = Stopwatch.StartNew();

            try
            {
                using var connection = new NpgsqlConnection(connectionString);
                using var command = new NpgsqlCommand("SELECT 1;", connection);

                await connection.OpenAsync(cancellationToken);
                await command.ExecuteScalarAsync(cancellationToken);

                return stopwatch.Elapsed;
            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                Console.WriteLine($"Connection test failed: {ex.Message}");
                return TimeSpan.FromMilliseconds(3000);
            }
        }
    }
}
