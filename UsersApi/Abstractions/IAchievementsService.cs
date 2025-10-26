using UsersApi.BLL.DTOs;

namespace UsersApi.Abstractions
{
    public interface IAchievementsService
    {
        Task<List<AchievementDto>> GetAllAchievements(int userId);
    }
}
