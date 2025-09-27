using NutritionsApi.Abstractions;
using NutritionsApi.BLL.DTO;
using NutritionsApi.BLL.DTO.RequestDto;
using NutritionsApi.BLL.DTO.ResponseDto;

namespace NutritionsApi.BLL.Factories;

public class DtoFactory : IDtoFactory
{
    public CreateNutritionRequestDto GenerateCreateDto(int userId, string? description, double calories)
    {
        return new CreateNutritionRequestDto
        {
            UserId = userId,
            Description = description,
            Calories = calories
        };
    }

    public UpdateNutritionRequestDto GenerateUpdateDto(int nutritionId, string? description, double calories)
    {
        return new UpdateNutritionRequestDto
        {
            NutritionId = nutritionId,
            Description = description,
            Calories = calories
        };
    }

    public NutritionDetailsResponseDto GenerateDetailsResponse(
        int nutritionId,
        int userId,
        string? description,
        double calories,
        bool isDeleted)
    {
        return new NutritionDetailsResponseDto
        {
            NutritionId = nutritionId,
            UserId = userId,
            Description = description,
            Calories = calories,
            IsDeleted = isDeleted
        };
    }
}