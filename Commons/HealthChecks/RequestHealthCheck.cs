using Commons.Config;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Diagnostics;

namespace Commons.HealthChecks
{
    public class RequestHealthCheck(RequestHealthCheckConfig requestHealthCheck, string requestUri) : IHealthCheck
    {
        public const string RequestTimeCheckHealthy = "Request time check completed. Response time: ";
        public const string RequestTimeCheckFailed = "Request time check failed";

        private readonly TimeSpan Degrated = TimeSpan.FromMilliseconds(requestHealthCheck.DegradedThresholdMilliseconds);
        private readonly TimeSpan Unhealthy = TimeSpan.FromMilliseconds(requestHealthCheck.UnhealthyThresholdMilliseconds);
        public async Task<HealthCheckResult> CheckHealthAsync(
         HealthCheckContext context,
         CancellationToken cancellationToken = default)
        {
            var stopwatch = Stopwatch.StartNew();
            try
            {
                using var client = new HttpClient();
                await client.GetAsync(requestUri + "/health", cancellationToken);
                stopwatch.Stop();
                var responseTime = TimeSpan.FromMilliseconds(stopwatch.ElapsedMilliseconds);
                var message = RequestTimeCheckHealthy + $"{responseTime.TotalMilliseconds}  ms";

                if (responseTime < Degrated)
                {
                    return HealthCheckResult.Healthy(message);
                }
                if (responseTime < Unhealthy)
                {
                    return HealthCheckResult.Degraded(message);
                }
                return HealthCheckResult.Unhealthy(message);
            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                Console.WriteLine(RequestTimeCheckFailed + ": " + ex.Message);
                return HealthCheckResult.Unhealthy(RequestTimeCheckFailed);
            }
        }
    }
}
