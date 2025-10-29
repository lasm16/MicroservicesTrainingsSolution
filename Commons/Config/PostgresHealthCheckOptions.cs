namespace Commons.Config
{
    public class PostgresHealthCheckOptions
    {
        public const string SectionName = "PostgresHealthCheck";
        public TimeSpan DegradedThreshold { get; set; } = TimeSpan.FromMilliseconds(500);
        public TimeSpan UnhealthyThreshold { get; set; } = TimeSpan.FromMilliseconds(2000);
    }
}
