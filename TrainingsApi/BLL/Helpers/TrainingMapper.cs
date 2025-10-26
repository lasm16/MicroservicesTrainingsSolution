using DataAccess.Models;
using TrainingsApi.BLL.Dtos;

namespace TrainingsApi.BLL.Helpers
{
    public static class TrainingMapper
    {
        public static TrainingDto ToDto(Training entity) => new()
        {
            UserId = entity.UserId,
            Description = entity.Description,
            Date = entity.Date,
            DurationInMinutes = entity.DurationInMinutes,
            Status = entity.Status
        };

        public static List<TrainingDto> ToDtoList(IEnumerable<Training> trainings)
        {
            return trainings.Select(ToDto).ToList();
        }

        public static Training FromCreateDto(TrainingCreateDto dto) => new()
        {
            UserId = dto.UserId,
            Description = dto.Description,
            Date = dto.Date,
            DurationInMinutes = dto.DurationInMinutes,
            Status = dto.Status ?? "Planned",
            Created = DateTime.UtcNow,
            Updated = DateTime.UtcNow,
            IsDeleted = false
        };

        public static void UpdateEntity(Training entity, TrainingUpdateDto dto)
        {
            entity.Description = dto.Description;
            entity.Date = dto.Date;
            entity.DurationInMinutes = dto.DurationInMinutes;
            entity.Updated = DateTime.UtcNow;
        }

        public static void UpdateStatus(Training entity, TrainingStatusUpdateDto dto)
        {
            entity.Status = dto.Status;
            entity.Updated = DateTime.UtcNow;
        }

        public static void MarkDeleted(Training entity, TrainingDeleteDto dto)
        {
            entity.IsDeleted = true;
            entity.Updated = DateTime.UtcNow;
        }
    }
}
