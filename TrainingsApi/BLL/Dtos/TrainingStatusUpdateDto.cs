namespace TrainingsApi.BLL.Dtos
{
    public class TrainingStatusUpdateDto
    {
        public int Id { get; set; }
        public string Status { get; set; } = null!;
        public DateTime Updated { get; set; }
    }
}
