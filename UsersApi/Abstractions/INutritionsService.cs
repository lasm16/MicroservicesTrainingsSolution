using UsersApi.BLL.DTOs;

namespace UsersApi.Abstractions
{
    public interface INutritionsService
    {
        Task<List<NutritionDto>> GetAllNutritions(int userId);
    }
}
