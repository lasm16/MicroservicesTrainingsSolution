using DataAccess.Enums;

namespace AchievementsApi.BLL.DTO
{
    public class AchievementDto
    {
        public int Id { get; set; }
        public AchievementType Type { get; set; }
        public decimal Value { get; set; }
        public DateTime AchievedDate { get; set; }
    }
}
