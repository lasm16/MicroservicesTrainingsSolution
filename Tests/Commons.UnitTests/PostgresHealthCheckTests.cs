using Commons.Config;
using Commons.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;
using Moq;

namespace Tests.Commons.UnitTests;

[TestClass]
public class PostgresHealthCheckTests
{
    private IOptions<HealthCheckConfig> CreateOptions(TimeSpan degraded, TimeSpan unhealthy)
    {
        var optionsMock = new Mock<IOptions<HealthCheckConfig>>();
        var config = new HealthCheckConfig { DegradedThreshold = degraded, UnhealthyThreshold = unhealthy };
        optionsMock.Setup(x => x.Value).Returns(config);
        return optionsMock.Object;
    }
    [TestMethod]
    public async Task CheckHealthAsync_FastResponse_ReturnsHealthy()
    {
        var connectionString = "any-string";
        var options = CreateOptions(
            TimeSpan.FromMilliseconds(500),
            TimeSpan.FromMilliseconds(2000)
        );
        var healthCheck = new PostgresHealthCheck(
            connectionString,
            options,
            (conn, token) => Task.FromResult(TimeSpan.FromMilliseconds(100)) // connectionTester
        );
        var context = new HealthCheckContext();

        var result = await healthCheck.CheckHealthAsync(context);

        Assert.AreEqual(HealthStatus.Healthy, result.Status);
        Assert.Contains("Response time: 100",result.Description);
    }

    [TestMethod]
    public async Task CheckHealthAsync_SlowResponse_ReturnsDegraded()
    {
        var connectionString = "any-string";
        var options = CreateOptions(
            TimeSpan.FromMilliseconds(500),
            TimeSpan.FromMilliseconds(2000)
        );
        var healthCheck = new PostgresHealthCheck(
            connectionString,
            options,
            (conn, token) => Task.FromResult(TimeSpan.FromMilliseconds(1000)) // connectionTester
        );
        var context = new HealthCheckContext();

        var result = await healthCheck.CheckHealthAsync(context);

        Assert.AreEqual(HealthStatus.Degraded, result.Status);
        Assert.Contains("Response time: 1000", result.Description);
    }

    [TestMethod]
    public async Task CheckHealthAsync_VerySlowResponse_ReturnsUnhealthy()
    {
        var connectionString = "any-string";
        var options = CreateOptions(
            TimeSpan.FromMilliseconds(500),
            TimeSpan.FromMilliseconds(2000)
        );
        var healthCheck = new PostgresHealthCheck(
            connectionString,
            options,
            (conn, token) => Task.FromResult(TimeSpan.FromMilliseconds(3000)) // connectionTester
        );
        var context = new HealthCheckContext();

        var result = await healthCheck.CheckHealthAsync(context);

        Assert.AreEqual(HealthStatus.Unhealthy, result.Status);
        Assert.Contains("Response time: 3000", result.Description);
    }

    [TestMethod]
    public async Task CheckHealthAsync_ConnectionError_ReturnsUnhealthy()
    {
        var connectionString = "any-string";
        var options = CreateOptions(
            TimeSpan.FromMilliseconds(500),
            TimeSpan.FromMilliseconds(2000)
        );
        var healthCheck = new PostgresHealthCheck(
            connectionString,
            options,
            (conn, token) => Task.FromResult(TimeSpan.FromMilliseconds(5000))
        );
        var context = new HealthCheckContext();

        var result = await healthCheck.CheckHealthAsync(context);

        Assert.AreEqual(HealthStatus.Unhealthy, result.Status);
        Assert.Contains("Response time: 5000", result.Description);
    }
}
