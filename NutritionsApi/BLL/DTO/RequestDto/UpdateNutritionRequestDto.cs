namespace NutritionsApi.BLL.DTO.RequestDto;

public class UpdateNutritionRequestDto
{
    public int NutritionId { get; set; }
    public string? Description { get; set; }
    public double Calories { get; set; }
    public DateTime Updated { get; set; }
    public bool IsDeleted { get; set; }
}