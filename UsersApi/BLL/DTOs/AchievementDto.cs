using DataAccess.Enums;

namespace UsersApi.BLL.DTOs
{
    public class AchievementDto
    {
        public int Id { get; set; }
        public AchievementType Type { get; set; }
        public decimal Value { get; set; }
        public decimal Reward { get; set; }
        public DateTime AchievedDate { get; set; }
    }
}
