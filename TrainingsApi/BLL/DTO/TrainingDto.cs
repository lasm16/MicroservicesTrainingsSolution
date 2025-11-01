namespace TrainingsApi.BLL.DTO
{
    public class TrainingDto
    {
        public int UserId { get; set; }
        public string? Description { get; set; }
        public DateTime Date { get; set; }
        public double DurationInMinutes { get; set; }
        public int Status { get; set; }
    }
}
