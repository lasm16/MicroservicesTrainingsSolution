namespace TrainingsApi.BLL.Dtos
{
    public class TrainingUpdateDto
    {
        public int Id { get; set; }
        public string? Description { get; set; }
        public DateTime Date { get; set; }
        public double DurationInMinutes { get; set; }
        public DateTime Updated { get; set; }
    }
}
