using AutoMapper;
using DataAccess.Models;
using NutritionsApi.BLL.DTO.RequestDto;

namespace NutritionsApi.BLL.Profiles;

public class NutritionProfile : Profile
{
    public NutritionProfile()
    {
        CreateMap<Nutrition, CreateNutritionRequestDto>().
            ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId));
        
        CreateMap<Nutrition, CreateNutritionRequestDto>().
            ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description));
        
        CreateMap<Nutrition, CreateNutritionRequestDto>().
            ForMember(dest => dest.Calories, opt => opt.MapFrom(src => src.Calories));
        
        CreateMap<CreateNutritionRequestDto, Nutrition>().
            ForMember(dest => dest.Calories, opt => opt.MapFrom(src => src.Calories));
        
        CreateMap<CreateNutritionRequestDto, Nutrition>().
            ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description));
        
        CreateMap<CreateNutritionRequestDto, Nutrition>().
            ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId));
    }
}