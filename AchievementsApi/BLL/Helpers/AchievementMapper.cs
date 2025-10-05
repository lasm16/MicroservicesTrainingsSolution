using AchievementsApi.BLL.DTO;
using AchievementsApi.BLL.DTO.Requests;
using DataAccess.Models;

namespace AchievementsApi.BLL.Helpers
{
    internal static class AchievementMapper
    {
        internal static AchievementDto MapEntityToDto(Achievement achievement)
        {
            return new AchievementDto
            {
                Id = achievement.Id,
                UserId = achievement.UserId,
                AchievedDate = achievement.AchievedDate,
                Value = achievement.Value,
                Reward = achievement.Reward,
                Type = achievement.Type,
                IsDeleted = achievement.IsDeleted,
            };
        }

        internal static List<AchievementDto> MapEntityCollectionToDto(List<Achievement> achievements)
        {
            return [.. achievements.Select(MapEntityToDto)];
        }

        internal static Achievement MapDtoToEntity(AchievementDto achievementDto)
        {
            return new Achievement
            {
                Id = achievementDto.Id,
                UserId = achievementDto.UserId,
                AchievedDate = achievementDto.AchievedDate,
                Value = achievementDto.Value,
                Reward = achievementDto.Reward,
                Type = achievementDto.Type,
                IsDeleted = achievementDto.IsDeleted,
            };
        }

        internal static Achievement MapDtoToEntity(AchievementUpdateRequest achievementDto)
        {
            return new Achievement
            {
                Id = achievementDto.Id,
                UserId = achievementDto.UserId,
                AchievedDate = achievementDto.AchievedDate,
                Value = achievementDto.Value,
                Type = achievementDto.Type,
                IsDeleted = achievementDto.IsDeleted,
            };
        }

        internal static Achievement MapDtoToEntity(AchievementCreateRequest request)
        {
            return new Achievement
            {
                UserId = request.UserId,
                AchievedDate = request.AchievedDate,
                Value = request.Value,
                Type = request.Type,
            };
        }
    }
}
