namespace UsersApi.Properties
{
    public class AppSettingsConfig
    {
        public AchievementsService? AchievementsService { get; set; }
        public NutritionsService? NutritionsService { get; set; }
        public TrainingsService? TrainingsService { get; set; }
    }

    public class AchievementsService
    {
        public string? Address { get; set; }
        public int TimeoutMilliseconds { get; set; }
    }

    public class NutritionsService
    {
        public string? Address { get; set; }
        public int TimeoutMilliseconds { get; set; }
    }

    public class TrainingsService
    {
        public string? Address { get; set; }
        public int TimeoutMilliseconds { get; set; }
    }
}
