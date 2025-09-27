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
        private readonly INutritionRepository _nutritionRepository = nutritionRepository;
        private readonly IMapper _dtoMapper = mapper;
        private readonly IDtoFactory _dtoFactory = dtoFactory;
        
        public async Task<NutritionDetailsResponseDto> GetByIdAsync(int nutritionId, CancellationToken cancellationToken = default)
        {
            var model = await _nutritionRepository.GetByIdAsync(nutritionId, cancellationToken);
            
            if (model is null)
                throw new NotFoundException("GetNutrition", nutritionId);
            
            var dto = _dtoFactory.GenerateDetailsResponse(
                model.Id, 
                model.UserId, 
                model.Description, 
                model.Calories, 
                model.IsDeleted);

            return dto;
        }

        public async Task<List<NutritionDetailsResponseDto>> GetAllAsync(int userId, CancellationToken cancellationToken = default)
        {
            var nutritionList = await _nutritionRepository.GetAllAsync(userId, cancellationToken);

            var dtoList = nutritionList?.Select(nutrition =>
                    _dtoFactory.GenerateDetailsResponse(
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
            var createModel = _dtoMapper.Map(dto, model);
            var result = await _nutritionRepository.CreateAsync(createModel, cancellationToken);
            
            if (!result)
            {
                throw new InvalidOperationException("Failed to create nutrition");
            }

            return _dtoFactory.GenerateDetailsResponse(
                model.Id, 
                model.UserId, 
                model.Description, 
                model.Calories, 
                model.IsDeleted);
        }

        public async Task<bool> UpdateAsync(UpdateNutritionRequestDto dto, CancellationToken cancellationToken = default)
        {
            var model = await _nutritionRepository.GetByIdAsync(dto.NutritionId, cancellationToken);

            if (model is null)
            {
                throw new NotFoundException("Update nutrition", dto.NutritionId);
            }
            
            var updatedModel = _dtoMapper.Map(dto, model);
            return await _nutritionRepository.UpdateAsync(updatedModel, cancellationToken);
        }

        public async Task<bool> DeleteAsync(int nutritionId, CancellationToken cancellationToken = default)
        {
            var model = await _nutritionRepository.GetByIdAsync(nutritionId, cancellationToken);

            if (model is null)
            {
                throw new NotFoundException("Delete nutrition", nutritionId);
            }
            
            return await _nutritionRepository.DeleteAsync(model.Id, cancellationToken);
        }
    }
}
