namespace TrainingsApi.BLL
{
    public class TrainingDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string? Description { get; set; }
        public DateTime Date { get; set; }
        public double DurationInMinutes { get; set; }
        public bool IsCompleted { get; set; }
    }
}
