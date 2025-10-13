using System.ComponentModel.DataAnnotations;

namespace AchievementsApi.BLL.DTO.Requests
{
    public class AchievementCreateRequest : AchievementRequest
    {
        [Range(1, int.MaxValue, ErrorMessage = $"Значение поля должно быть в диапозоне от 1 до 2147483647")]
        public int UserId { get; set; }
    }
}
