using Commons.Config;
using Commons.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Tests.Commons.UnitTests;

[TestClass]
public class PostgresHealthCheckTest
{
    public TestContext TestContext { get; set; }

    [TestMethod]
    public async Task CheckHealthAsync_FastResponse_ReturnsHealthy()
    {
        var connectionString = "any-string";
        var config = GetPostgresHealthCheckConfig(500, 2000);
        var healthCheck = new PostgresHealthCheck(
            connectionString,
            config,
            (conn, token) => Task.FromResult(TimeSpan.FromMilliseconds(100)) 
        );
        var context = new HealthCheckContext();

        var result = await healthCheck.CheckHealthAsync(context, TestContext.CancellationToken);

        Assert.AreEqual(HealthStatus.Healthy, result.Status);
        Assert.Contains("Response time: 100", result.Description!);
    }

    [TestMethod]
    public async Task CheckHealthAsync_SlowResponse_ReturnsDegraded()
    {
        var connectionString = "any-string";
        var config = GetPostgresHealthCheckConfig(500, 2000);
        var healthCheck = new PostgresHealthCheck(
            connectionString,
            config,
            (conn, token) => Task.FromResult(TimeSpan.FromMilliseconds(1000)) // connectionTester
        );
        var context = new HealthCheckContext();

        var result = await healthCheck.CheckHealthAsync(context, TestContext.CancellationToken);

        Assert.AreEqual(HealthStatus.Degraded, result.Status);
        Assert.Contains("Response time: 1000", result.Description!);
    }

    [TestMethod]
    public async Task CheckHealthAsync_VerySlowResponse_ReturnsUnhealthy()
    {
        var connectionString = "any-string";
        var config = GetPostgresHealthCheckConfig(500, 2000);
        var healthCheck = new PostgresHealthCheck(
            connectionString,
            config,
            (conn, token) => Task.FromResult(TimeSpan.FromMilliseconds(3000)) // connectionTester
        );
        var context = new HealthCheckContext();

        var result = await healthCheck.CheckHealthAsync(context, TestContext.CancellationToken);

        Assert.AreEqual(HealthStatus.Unhealthy, result.Status);
        Assert.Contains("Response time: 3000", result.Description!);
    }

    [TestMethod]
    public async Task CheckHealthAsync_ConnectionError_ReturnsUnhealthy()
    {
        var connectionString = "any-string";
        var config = GetPostgresHealthCheckConfig(500, 2000);
        var healthCheck = new PostgresHealthCheck(
            connectionString,
            config,
            (conn, token) => Task.FromResult(TimeSpan.FromMilliseconds(5000))
        );
        var context = new HealthCheckContext();

        var result = await healthCheck.CheckHealthAsync(context, TestContext.CancellationToken);

        Assert.AreEqual(HealthStatus.Unhealthy, result.Status);
        Assert.Contains("Response time: 5000", result.Description!);
    }

    private static PostgresHealthCheckConfig GetPostgresHealthCheckConfig(int degraded, int unhealthy)
    {
        return new PostgresHealthCheckConfig
        {
            DegradedThresholdMilliseconds = degraded,
            UnhealthyThresholdMilliseconds = unhealthy
        };
    }
}
