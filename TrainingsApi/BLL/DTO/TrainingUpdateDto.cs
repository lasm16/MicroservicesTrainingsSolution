namespace TrainingsApi.BLL.DTO
{
    public class TrainingUpdateDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string? Description { get; set; }
        public int Status { get; set; }
        public DateTime Date { get; set; }
        public double DurationInMinutes { get; set; }
        public bool IsDeleted { get; set; }
    }
}
