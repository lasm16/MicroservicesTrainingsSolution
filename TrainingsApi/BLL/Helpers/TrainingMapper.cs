using DataAccess.Models;

namespace TrainingsApi.BLL.Helpers
{
    public static class TrainingMapper
    {
        public static TrainingDto ToDto(Training training)
        {
            return new TrainingDto
            {
                Id = training.Id,
                UserId = training.UserId,
                Description = training.Description,
                Date = training.Date,
                DurationInMinutes = training.DurationInMinutes,
                IsCompleted = training.IsCompleted
            };
        }

        public static List<TrainingDto> ToDtoList(IEnumerable<Training> trainings)
        {
            return trainings.Select(ToDto).ToList();
        }

        public static Training ToEntity(TrainingDto dto)
        {
            return new Training
            {
                Id = dto.Id,
                UserId = dto.UserId,
                Description = dto.Description,
                Date = dto.Date,
                DurationInMinutes = dto.DurationInMinutes,
                IsCompleted = dto.IsCompleted,
                Created = DateTime.UtcNow,
                Updated = DateTime.UtcNow,
                IsDeleted = false
            };
        }

        public static void UpdateEntity(Training training, TrainingDto dto)
        {
            training.Description = dto.Description;
            training.Date = dto.Date;
            training.DurationInMinutes = dto.DurationInMinutes;
            training.IsCompleted = dto.IsCompleted;
            training.Updated = DateTime.UtcNow;
        }
    }
}
