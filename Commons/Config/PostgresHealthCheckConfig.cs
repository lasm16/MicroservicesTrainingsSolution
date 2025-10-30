using System.Reflection;
using System.Text.Json;

namespace Commons.Config
{
    public class PostgresHealthCheckConfig
    {
        public TimeSpan DegradedThresholdMilliseconds { get; set; }
        public TimeSpan UnhealthyThresholdMilliseconds { get; set; }

        public static PostgresHealthCheckConfig LoadFromEmbeddedResource()
        {
            var assembly = Assembly.GetExecutingAssembly();
            
            var resourceName = "Commons.appsettings.json";

            using var stream = assembly.GetManifestResourceStream(resourceName);
            if (stream == null)
            {
                var resourceNames = assembly.GetManifestResourceNames();
                Console.WriteLine("Available embedded resources in Commons:");
                foreach (var name in resourceNames)
                {
                    Console.WriteLine($" - {name}");
                }
                throw new InvalidOperationException($"Embedded resource '{resourceName}' not found in assembly '{assembly.FullName}'" +
                    $". Check the file name and Build Action (should be Embedded Resource).");
            }

            using var reader = new StreamReader(stream);
            var jsonString = reader.ReadToEnd();

            using var jsonDocument = JsonDocument.Parse(jsonString);
            var root = jsonDocument.RootElement;

            var config = new PostgresHealthCheckConfig();

            var postgresSection = root.GetProperty("HealthCheck").GetProperty("PostgresHealthCheck");

            config.DegradedThresholdMilliseconds = TimeSpan.Parse(postgresSection.GetProperty(nameof(config.DegradedThresholdMilliseconds)).GetString());
            config.UnhealthyThresholdMilliseconds = TimeSpan.Parse(postgresSection.GetProperty(nameof(config.UnhealthyThresholdMilliseconds)).GetString());

            return config;
        }

    }
}
