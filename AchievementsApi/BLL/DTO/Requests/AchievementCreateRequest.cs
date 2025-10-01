using DataAccess.Enums;

namespace AchievementsApi.BLL.DTO.Requests
{
    public class AchievementCreateRequest : AchievementRequest
    {
        public int UserId { get; set; }
    }
}
