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

        private UserResponse() { }

        public class Builder
        {
            private readonly UserResponse _response = new();

            public Builder SetId(int id)
            {
                _response.Id = id;
                return this;
            }

            public Builder SetName(string name)
            {
                _response.Name = name;
                return this;
            }

            public Builder SetSurname(string surname)
            {
                _response.Surname = surname;
                return this;
            }

            public Builder SetEmail(string email)
            {
                _response.Email = email;
                return this;
            }

            public UserResponse Build()
            {
                return _response;
            }
        }
    }
}
