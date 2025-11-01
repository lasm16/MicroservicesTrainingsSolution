namespace Commons.Config
{
    public class HealthCheckConfig
    {
        public PostgresHealthCheckConfig? PostgresHealthCheckConfig { get; set; }
        public RequestHealthCheckConfig? RequestHealthCheckConfig { get; set; }
    }

    public class PostgresHealthCheckConfig
    {
        public int DegradedThresholdMilliseconds { get; set; }
        public int UnhealthyThresholdMilliseconds { get; set; }
    }

    public class RequestHealthCheckConfig
    {
        public int DegradedThresholdMilliseconds { get; set; }
        public int UnhealthyThresholdMilliseconds { get; set; }
    }
}
