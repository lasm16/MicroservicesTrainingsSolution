using Commons.HealthChecks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Tests.Commons.UnitTests;

[TestClass]
public class HealthCheckOptionsFactoryTests
{
    [TestMethod]
    public void Create_WithServiceName_ReturnsConfiguredOptions()
    {
        var serviceName = "TestService";

        var options = HealthCheckOptionsFactory.Create(serviceName);

        Assert.IsNotNull(options);
        Assert.IsNotNull(options.ResponseWriter);
    }

    [TestMethod]
    public async Task ResponseWriter_WritesCorrectJsonStructure()
    {
        var serviceName = "TestService";
        var options = HealthCheckOptionsFactory.Create(serviceName);

        var context = new DefaultHttpContext();
        context.Response.Body = new MemoryStream();

        var report = new HealthReport(new Dictionary<string, HealthReportEntry>
        {
            ["test_check"] = new HealthReportEntry(
                HealthStatus.Healthy,
                "Test check passed",
                TimeSpan.FromMilliseconds(100),
                null,
                null)
        }, TimeSpan.FromMilliseconds(150));

        await options.ResponseWriter!(context, report);

        context.Response.Body.Seek(0, SeekOrigin.Begin);
        var responseBody = new StreamReader(context.Response.Body).ReadToEnd();

        Assert.AreEqual("application/json", context.Response.ContentType);
        Assert.IsTrue(responseBody.Contains(serviceName));
        Assert.IsTrue(responseBody.Contains("Healthy"));
        Assert.IsTrue(responseBody.Contains("test_check"));
        Assert.IsTrue(responseBody.Contains("Test check passed"));
    }

    [TestMethod]
    public async Task ResponseWriter_WithUnhealthyStatus_WritesCorrectJson()
    {
        var serviceName = "TestService";
        var options = HealthCheckOptionsFactory.Create(serviceName);

        var context = new DefaultHttpContext();
        context.Response.Body = new MemoryStream();

        var exception = new Exception("Database connection failed");
        var report = new HealthReport(new Dictionary<string, HealthReportEntry>
        {
            ["db_check"] = new HealthReportEntry(
                HealthStatus.Unhealthy,
                "Database is down",
                TimeSpan.FromMilliseconds(5000),
                exception,
                null)
        }, TimeSpan.FromMilliseconds(5000));

        await options.ResponseWriter!(context, report);

        context.Response.Body.Seek(0, SeekOrigin.Begin);
        var responseBody = new StreamReader(context.Response.Body).ReadToEnd();

        Assert.IsTrue(responseBody.Contains("Unhealthy"));
        Assert.IsTrue(responseBody.Contains("Database is down"));
        Assert.IsTrue(responseBody.Contains("db_check"));
    }
}
