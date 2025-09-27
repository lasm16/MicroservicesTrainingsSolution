namespace NutritionsApi.BLL.DTO.RequestDto
{
    public class CreateNutritionRequestDto
    {
        public int UserId { get; set; }
        public string? Description { get; set; }
        public double Calories { get; set; }
    }
}
