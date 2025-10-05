using DataAccess.Enums;

namespace AchievementsApi.BLL.DTO.Requests
{
    public abstract class AchievementRequest
    {
        public AchievementType Type { get; set; }
        public decimal Value { get; set; }
        public DateTime AchievedDate { get; set; }
    }
}
