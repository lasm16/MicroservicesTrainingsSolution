using AutoMapper;
using DataAccess.Models;
using NutritionsApi.Abstractions;
using NutritionsApi.BLL.DTO.RequestDto;
using NutritionsApi.BLL.DTO.ResponseDto;
using NutritionsApi.Exceptions;

namespace NutritionsApi.BLL.Services
{
    public class NutritionService(
        INutritionRepository nutritionRepository,
        IMapper mapper,
        IDtoFactory dtoFactory) : INutritionService
    {
        public async Task<NutritionDetailsResponseDto> GetByIdAsync(int nutritionId, CancellationToken cancellationToken = default)
        {
            var model = await nutritionRepository.GetByIdAsync(nutritionId, cancellationToken)
                ?? throw new NotFoundException("GetNutrition", nutritionId);

            var dto = dtoFactory.GenerateDetailsResponse(
                model.Id,
                model.UserId,
                model.Description,
                model.Calories,
                model.IsDeleted);

            return dto;
        }

        public async Task<List<NutritionDetailsResponseDto>> GetAllAsync(int userId, CancellationToken cancellationToken = default)
        {
            var nutritionList = await nutritionRepository.GetAllAsync(userId, cancellationToken);

            var dtoList = nutritionList?.Select(nutrition =>
                    dtoFactory.GenerateDetailsResponse(
                        nutrition.Id,
                        nutrition.UserId,
                        nutrition.Description,
                        nutrition.Calories,
                        nutrition.IsDeleted)
                    ).ToList();

            return dtoList ?? [];
        }


        public async Task<NutritionDetailsResponseDto> CreateAsync(CreateNutritionRequestDto dto, CancellationToken cancellationToken = default)
        {
            var model = new Nutrition();
            var createModel = mapper.Map(dto, model);
            var result = await nutritionRepository.CreateAsync(createModel, cancellationToken);

            if (!result)
            {
                throw new InvalidOperationException("Failed to create nutrition");
            }

            return dtoFactory.GenerateDetailsResponse(
                model.Id,
                model.UserId,
                model.Description,
                model.Calories,
                model.IsDeleted);
        }

        public async Task<bool> UpdateAsync(UpdateNutritionRequestDto dto, CancellationToken cancellationToken = default)
        {
            var model = await nutritionRepository.GetByIdAsync(dto.NutritionId, cancellationToken)
                ?? throw new NotFoundException("Update nutrition", dto.NutritionId);
            var updatedModel = mapper.Map(dto, model);
            return await nutritionRepository.UpdateAsync(updatedModel, cancellationToken);
        }

        public async Task<bool> DeleteAsync(int nutritionId, CancellationToken cancellationToken = default)
        {
            var model = await nutritionRepository.GetByIdAsync(nutritionId, cancellationToken);

            return model is null
                ? throw new NotFoundException("Delete nutrition", nutritionId)
                : await nutritionRepository.DeleteAsync(model.Id, cancellationToken);
        }
    }
}
