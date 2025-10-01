using DataAccess.Enums;

namespace AchievementsApi.BLL.DTO.Requests
{
    public class AchievementUpdateRequest : AchievementRequest
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public bool IsDeleted { get; set; }
    }
}
