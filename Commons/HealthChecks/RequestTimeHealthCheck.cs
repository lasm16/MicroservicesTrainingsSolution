using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Commons
{
    public class RequestTimeHealthCheck : IHealthCheck
    {
        public Task<HealthCheckResult> CheckHealthAsync(
         HealthCheckContext context,
         CancellationToken cancellationToken = default)
        {
            try
            {
                return Task.FromResult(
                    HealthCheckResult.Healthy("Request time check passed"));
            }
            catch (Exception ex)
            {
                return Task.FromResult(
                    HealthCheckResult.Unhealthy("Request time check failed", ex));
            }
        }
    }
}
