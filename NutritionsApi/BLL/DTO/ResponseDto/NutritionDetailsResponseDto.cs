namespace NutritionsApi.BLL.DTO.ResponseDto;

public class NutritionDetailsResponseDto
{
    public int NutritionId { get; set; }
    public int UserId { get; set; }
    public string? Description { get; set; }
    public double Calories { get; set; }
    public bool IsDeleted { get; set; }
}