namespace UsersApi.BLL.DTOs
{
    public class UserResponse
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public string? Email { get; set; }
        public List<AchievementDto>? Achievements { get; set; }
        public List<TrainingDto>? Trainings { get; set; }
        public List<NutritionDto>? Nutritions { get; set; }
    }
}
