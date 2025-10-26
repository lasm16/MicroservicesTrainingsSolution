using NutritionsApi.BLL.DTO;
using NutritionsApi.BLL.DTO.RequestDto;
using NutritionsApi.BLL.DTO.ResponseDto;

namespace NutritionsApi.Abstractions;

public interface IDtoFactory
{
    CreateNutritionRequestDto GenerateCreateDto(int userId, string? description, double calories);
    UpdateNutritionRequestDto GenerateUpdateDto(int nutritionId, string? description, double calories);
    NutritionDetailsResponseDto GenerateDetailsResponse(
        int nutritionId,
        int userId,
        string? description,
        double calories,
        bool isDeleted);
}