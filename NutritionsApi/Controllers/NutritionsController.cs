using Microsoft.AspNetCore.Mvc;
using NutritionsApi.BLL.Services;
using UsersApi.BLL.Services;

namespace NutritionsApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NutritionsController(INutritionService nutritionService, IUserService userService) : ControllerBase
    {
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetNutritionAsync([FromRoute] int id)
        {
            var result = await nutritionService.GetByIdAsync(id);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllNutritionsAsync()
        {
            var result = await nutritionService.GetAllAsync();
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateNutritionAsync(int userId, string description, double calories)
        {
            await nutritionService.CreateAsync(userId, description, calories);
            return NoContent();
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateNutritionAsync([FromRoute] int id, int userId, string description, double calories)
        {
            await nutritionService.UpdateAsync(id, userId, description, calories);
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteNutritionAsync([FromRoute] int id)
        {
            await nutritionService.DeleteAsync(id);
            return NoContent();
        }
    }
}
