namespace DataAccess.Models
{
    public class Achievement
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string? Type { get; set; }
        public decimal Value { get; set; }
        public DateTime AchievedDate { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
        public bool IsDeleted { get; set; }
    }
}
