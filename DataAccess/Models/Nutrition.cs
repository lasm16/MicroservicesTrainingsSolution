namespace DataAccess.Models
{
    public class Nutrition
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string? Description { get; set; }
        public double Calories { get; set; }
        public DateTime Created { get; set; } = DateTime.UtcNow;
        public DateTime Updated { get; set; }
        public bool IsDeleted { get; set; }
    }
}
