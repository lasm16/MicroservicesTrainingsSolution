using Commons.Config;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Diagnostics;

namespace Commons.HealthChecks
{
    public class RequestHealthCheck(IHttpClientFactory httpClientFactory, string requestUri, RequestHealthCheckConfig requestHealthCheck) : IHealthCheck
    {
        public const string RequestTimeCheckHealthy = "Request time check completed. Response time: ";
        public const string RequestTimeCheckFailed = "Request time check failed. ";

        private readonly TimeSpan Degrated = TimeSpan.FromMilliseconds(requestHealthCheck.DegradedThresholdMilliseconds);
        private readonly TimeSpan Unhealthy = TimeSpan.FromMilliseconds(requestHealthCheck.UnhealthyThresholdMilliseconds);
        public async Task<HealthCheckResult> CheckHealthAsync(
         HealthCheckContext context,
         CancellationToken cancellationToken = default)
        {
            var stopwatch = Stopwatch.StartNew();
            try
            {
                var client = httpClientFactory.CreateClient();
                using var httpResponseMessage = await client.GetAsync(requestUri + "/health", cancellationToken);
                stopwatch.Stop();
                var responseTime = TimeSpan.FromMilliseconds(stopwatch.ElapsedMilliseconds);
                var messageSuccess = RequestTimeCheckHealthy + $"{responseTime.TotalMilliseconds}  ms";
                if (responseTime < Degrated)
                {
                    return HealthCheckResult.Healthy(messageSuccess);
                }
                if (responseTime < Unhealthy)
                {
                    return HealthCheckResult.Degraded(messageSuccess);
                }
                var messageFail = RequestTimeCheckFailed + $"Response time: {responseTime.TotalMilliseconds}  ms";
                return HealthCheckResult.Unhealthy(messageFail);
            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                Console.WriteLine(RequestTimeCheckFailed + ex.Message);
                return HealthCheckResult.Unhealthy(RequestTimeCheckFailed);
            }
        }
    }
}
