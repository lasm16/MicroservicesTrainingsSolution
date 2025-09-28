using DataAccess.Enums;

namespace DataAccess.Models
{
    public class Achievement
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public AchievementType Type { get; set; }
        public decimal Value { get; set; }
        public DateTime AchievedDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool IsDeleted { get; set; }
    }
}
