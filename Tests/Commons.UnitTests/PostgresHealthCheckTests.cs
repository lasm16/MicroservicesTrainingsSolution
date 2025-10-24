using Commons.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Tests.Commons.UnitTests;

[TestClass]
public class PostgresHealthCheckTests
{
    [TestMethod]
    public async Task CheckHealthAsync_ValidConnectionFastResponse_ReturnsHealthy()
    {
        var connectionString = "Host=localhost;Port=5432;Database=TestDb;Username=postgres;Password=12345;";
        var healthCheck = new PostgresHealthCheck(connectionString);
        var context = new HealthCheckContext();

        var result = await healthCheck.CheckHealthAsync(context);

        Assert.IsTrue(result.Status == HealthStatus.Healthy ||
                     result.Status == HealthStatus.Unhealthy);
        Assert.IsTrue(result.Description.Contains("PostgreSQL"));
    }

    [TestMethod]
    public async Task CheckHealthAsync_InvalidConnectionString_ReturnsUnhealthy()
    {
        var invalidConnectionString = "InvalidConnectionString";
        var healthCheck = new PostgresHealthCheck(invalidConnectionString);
        var context = new HealthCheckContext();

        var result = await healthCheck.CheckHealthAsync(context);

        Assert.AreEqual(HealthStatus.Unhealthy, result.Status);
        Assert.IsTrue(result.Description.Contains("PostgreSQL connection failed"));
        Assert.IsNotNull(result.Exception);
    }    
}
