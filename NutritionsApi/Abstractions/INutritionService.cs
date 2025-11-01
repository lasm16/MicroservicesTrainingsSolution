using NutritionsApi.BLL.DTO.RequestDto;
using NutritionsApi.BLL.DTO.ResponseDto;

namespace NutritionsApi.Abstractions
{
    public interface INutritionService
    {
        Task<NutritionDetailsResponseDto> GetByIdAsync(int nutritionId, CancellationToken cancellationToken = default);
        Task<List<NutritionDetailsResponseDto>> GetAllAsync(int userId, CancellationToken cancellationToken = default);
        internal Task<NutritionDetailsResponseDto> CreateAsync(
            CreateNutritionRequestDto dto, 
            CancellationToken cancellationToken = default);
        internal Task<bool> UpdateAsync(
            UpdateNutritionRequestDto dto, 
            CancellationToken cancellationToken = default);
        internal Task<bool> DeleteAsync(int nutritionId, CancellationToken cancellationToken = default);
    }
}
