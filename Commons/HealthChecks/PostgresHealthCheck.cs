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
        private readonly IOptions<PostgresHealthCheckOptions> _options;
        private readonly Func<string, CancellationToken, Task<TimeSpan>> _connectionTester;

        public PostgresHealthCheck(string connectionString, IOptions<PostgresHealthCheckOptions> options)
            : this(connectionString, options, TestConnection)
        {
        }

        public PostgresHealthCheck(
            string connectionString,
            IOptions<PostgresHealthCheckOptions> options, 
            Func<string, CancellationToken, Task<TimeSpan>> connectionTester)
        {
            _connectionString = connectionString;
            _options = options; // Сохраняем ссылку на IOptions
            _connectionTester = connectionTester;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(
            HealthCheckContext context,
            CancellationToken cancellationToken = default)
        {
            var responseTime = await _connectionTester(_connectionString, cancellationToken);

            var message = $"PostgreSQL check completed. Response time: {responseTime.TotalMilliseconds}ms";

            if (responseTime < _options.Value.DegradedThreshold)
            {
                return HealthCheckResult.Healthy(message);
            }
            else if (responseTime < _options.Value.UnhealthyThreshold)
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
