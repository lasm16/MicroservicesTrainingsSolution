using DataAccess.Enums;

namespace AchievementsApi.BLL.DTO
{
    public class AchievementRequest
    {
        public int UserId { get; set; }
        public AchievementType Type { get; set; }
        public decimal Value { get; set; }
        public DateTime AchievedDate { get; set; }
    }
}
