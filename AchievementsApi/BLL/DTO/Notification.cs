namespace AchievementsApi.BLL.DTO
{
    public class Notification
    {
        public required string Message { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
