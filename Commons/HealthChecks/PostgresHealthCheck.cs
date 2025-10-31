using Commons.Config;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Npgsql;
using System.Diagnostics;

namespace Commons.HealthChecks
{
    public class PostgresHealthCheck : IHealthCheck
    {
        private readonly string _connectionString;
        private readonly PostgresHealthCheckConfig _config;
        private readonly Func<string, CancellationToken, Task<TimeSpan>> _connectionTester;
        private TimeSpan Degrated => TimeSpan.FromMilliseconds(_config.DegradedThresholdMilliseconds);
        private TimeSpan Unhealthy => TimeSpan.FromMilliseconds(_config.UnhealthyThresholdMilliseconds);

        public PostgresHealthCheck(string connectionString, PostgresHealthCheckConfig config)
            : this(connectionString, config, TestConnection)
        {
        }

        public PostgresHealthCheck(
            string connectionString,
            PostgresHealthCheckConfig config,
            Func<string, CancellationToken, Task<TimeSpan>> connectionTester)
        {
            _connectionString = connectionString;
            _config = config;
            _connectionTester = connectionTester;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(
            HealthCheckContext context,
            CancellationToken cancellationToken = default)
        {
            var responseTime = await _connectionTester(_connectionString, cancellationToken);

            var message = $"PostgreSQL check completed. Response time: {responseTime.TotalMilliseconds} ms";

            if (responseTime < Degrated)
            {
                return HealthCheckResult.Healthy(message);
            }
            else if (responseTime < Unhealthy)
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
                stopwatch.Stop();
                return stopwatch.Elapsed;
            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                Console.WriteLine($"Connection test failed: {ex.Message}");
                return stopwatch.Elapsed;
            }
        }
    }
}

