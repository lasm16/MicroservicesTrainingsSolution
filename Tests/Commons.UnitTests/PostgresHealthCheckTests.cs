using Commons.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Tests.Commons.UnitTests;

[TestClass]
public class PostgresHealthCheckTests
{
    [TestMethod]
    public async Task CheckHealthAsync_FastResponse_ReturnsHealthy()
    {
        var connectionString = "any-string";
        var healthCheck = new PostgresHealthCheck(
            connectionString,
            (conn, token) => Task.FromResult(TimeSpan.FromMilliseconds(100))
        );
        var context = new HealthCheckContext();

        var result = await healthCheck.CheckHealthAsync(context);

        Assert.AreEqual(HealthStatus.Healthy, result.Status);
        Assert.IsTrue(result.Description.Contains("Response time: 100ms"));
    }

    [TestMethod]
    public async Task CheckHealthAsync_SlowResponse_ReturnsDegraded()
    {
        var connectionString = "any-string";
        var healthCheck = new PostgresHealthCheck(
            connectionString,
            (conn, token) => Task.FromResult(TimeSpan.FromMilliseconds(1000))
        );
        var context = new HealthCheckContext();

        var result = await healthCheck.CheckHealthAsync(context);

        Assert.AreEqual(HealthStatus.Degraded, result.Status);
        Assert.IsTrue(result.Description.Contains("Response time: 1000ms"));
    }

    [TestMethod]
    public async Task CheckHealthAsync_VerySlowResponse_ReturnsUnhealthy()
    {
        var connectionString = "any-string";
        var healthCheck = new PostgresHealthCheck(
            connectionString,
            (conn, token) => Task.FromResult(TimeSpan.FromMilliseconds(3000))
        );
        var context = new HealthCheckContext();

        var result = await healthCheck.CheckHealthAsync(context);

        Assert.AreEqual(HealthStatus.Unhealthy, result.Status);
        Assert.IsTrue(result.Description.Contains("Response time: 3000ms"));
    }

    [TestMethod]
    public async Task CheckHealthAsync_ConnectionError_ReturnsUnhealthy()
    {
        var connectionString = "invalid-string";
        var healthCheck = new PostgresHealthCheck(
            connectionString,
            (conn, token) => Task.FromResult(TimeSpan.FromMilliseconds(5000))
        );
        var context = new HealthCheckContext();

        var result = await healthCheck.CheckHealthAsync(context);

        Assert.AreEqual(HealthStatus.Unhealthy, result.Status);
    }
}
