using DataAccess.Enums;
using System.ComponentModel.DataAnnotations;

namespace AchievementsApi.BLL.DTO.Requests
{
    public abstract class AchievementRequest
    {
        [Range(0, 2147483647, ErrorMessage = "Значение поля должно быть в диапозоне от 1 до 2147483647")]
        public AchievementType Type { get; set; }
        public decimal Value { get; set; }
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Range(typeof(DateTime), "1/1/2000", "1/1/2999", ErrorMessage = "Введите корректную дату в между 01.01.2000 0:00:00 и 01.01.2999 0:00:00")]
        public DateTime AchievedDate { get; set; }
    }
}
