using AutoMapper;
using DataAccess.Models;
using NutritionsApi.BLL.DTO;
using NutritionsApi.BLL.DTO.RequestDto;

namespace NutritionsApi.BLL.Profiles;

public class UpdateNutritionProfile : Profile
{
    public UpdateNutritionProfile()
    {
        CreateMap<Nutrition, UpdateNutritionRequestDto>().
            ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description));
            
        CreateMap<Nutrition, UpdateNutritionRequestDto>().
            ForMember(dest => dest.Calories, opt => opt.MapFrom(src => src.Calories));
        
        CreateMap<Nutrition, UpdateNutritionRequestDto>().
            ForMember(dest => dest.Updated, opt => opt.MapFrom(src => src.Updated));
        
        CreateMap<Nutrition, UpdateNutritionRequestDto>().
            ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(src => src.IsDeleted));
        
        CreateMap<UpdateNutritionRequestDto, Nutrition>().
            ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description));
        
        CreateMap<UpdateNutritionRequestDto, Nutrition>().
            ForMember(dest => dest.Calories, opt => opt.MapFrom(src => src.Calories));
        
        CreateMap<UpdateNutritionRequestDto, Nutrition>().
            ForMember(dest => dest.Updated, opt => opt.MapFrom(src => src.Updated));
        
        CreateMap<UpdateNutritionRequestDto, Nutrition>().
            ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(src => src.IsDeleted));
    }
}