namespace TrainingsApi.BLL.Dtos
{
    public class TrainingCreateDto
    {
        public int UserId { get; set; }
        public string? Description { get; set; }
        public DateTime Date { get; set; }
        public double DurationInMinutes { get; set; }
        public string? Status { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
        public bool IsDeleted { get; set;}
    }
}
