using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Commons.HealthChecks
{
    public class RequestTimeHealthCheck : IHealthCheck
    {
        public const string RequestTimeCheckPassed = "Request time check passed";
        public const string RequestTimeCheckFailed = "Request time check failed";
        public Task<HealthCheckResult> CheckHealthAsync(
         HealthCheckContext context,
         CancellationToken cancellationToken = default)
        {
            try
            {
                return Task.FromResult(
                    HealthCheckResult.Healthy(RequestTimeCheckPassed));
            }
            catch (Exception ex)
            {
                return Task.FromResult(
                    HealthCheckResult.Unhealthy(RequestTimeCheckFailed, ex));
            }
        }
    }
}
