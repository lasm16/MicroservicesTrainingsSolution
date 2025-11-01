using DataAccess.Models;
using TrainingsApi.BLL.Dtos;

namespace TrainingsApi.BLL.Helpers
{
    public static class TrainingMapper
    {
        public static TrainingDto ToDto(Training training) => new()
        {
            UserId = training.UserId,
            Description = training.Description,
            Date = training.Date,
            DurationInMinutes = training.DurationInMinutes,
            Status = (int)training.Status
        };

        public static List<TrainingDto> ToDtoList(IEnumerable<Training> trainings)
        {
            return [.. trainings.Select(ToDto)];
        }

        public static Training ToModel(TrainingCreateDto dto) => new()
        {
            UserId = dto.UserId,
            Description = dto.Description,
            Date = dto.Date,
            DurationInMinutes = dto.DurationInMinutes,
            Status = (DataAccess.Enums.StatusType)dto.Status,
        };

        public static Training ToModel(TrainingUpdateDto dto) => new()
        {
            Id = dto.Id,
            UserId = dto.UserId,
            Description = dto.Description,
            Date = dto.Date,
            DurationInMinutes = dto.DurationInMinutes,
            Status = (DataAccess.Enums.StatusType)dto.Status,
            IsDeleted = dto.IsDeleted,
        };
    }
}
