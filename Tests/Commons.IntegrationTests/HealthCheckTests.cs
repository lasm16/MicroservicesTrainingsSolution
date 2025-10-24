using Commons;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Tests.Commons.IntegrationTests
{
    [TestClass]
    public class HealthCheckTests
    {
        [TestMethod]
        public async Task CheckHealthAsync_Always_ReturnsHealthy()
        {
            // Arrange
            var healthCheck = new RequestTimeHealthCheck();
            var context = new HealthCheckContext();

            // Act
            var result = await healthCheck.CheckHealthAsync(context);

            // Assert
            Assert.AreEqual(HealthStatus.Healthy, result.Status);
            Assert.IsTrue(result.Description.Contains("Request time check passed"));
        }

        [TestMethod]
        public void RequestTimeHealthCheck_Constructor_NoParameters_Success()
        {
            // Arrange & Act
            var healthCheck = new RequestTimeHealthCheck();

            // Assert
            Assert.IsNotNull(healthCheck);
        }
    }    
}
