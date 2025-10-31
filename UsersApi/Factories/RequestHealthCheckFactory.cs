using Commons.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;
using UsersApi.Abstractions;
using UsersApi.Properties;

namespace UsersApi.Factories
{
    public class RequestHealthCheckFactory(
        IServiceProvider serviceProvider, 
        IHttpClientFactory httpClientFactory) : IHealthCheckFactory
    {

        public IHealthCheck Create(string url)
        {
            var appSettingsConfig = serviceProvider.GetRequiredService<IOptions<AppSettingsConfig>>().Value;
            var requestHealthCheckConfig = appSettingsConfig.HealthCheckConfig.RequestHealthCheckConfig;
            return new RequestHealthCheck(httpClientFactory, url, requestHealthCheckConfig);
        }
    }
}
