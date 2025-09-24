namespace AchievementsApi.BLL.DTO
{
    public class AchievementDto
    {
        public int Id { get; set; }
        public UserDto? User { get; set; }
        public int Type { get; set; }
        public decimal Value { get; set; }
        public DateTime AchievedDate { get; set; }
    }
}
