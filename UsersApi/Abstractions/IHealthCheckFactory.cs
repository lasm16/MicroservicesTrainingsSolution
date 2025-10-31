using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace UsersApi.Abstractions
{
    public interface IHealthCheckFactory
    {
        IHealthCheck Create(string url);
    }
}
