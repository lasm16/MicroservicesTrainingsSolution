using Microsoft.AspNetCore.Mvc;
using NutritionsApi.Abstractions;
using NutritionsApi.BLL.DTO.RequestDto;

namespace NutritionsApi.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class NutritionsController(INutritionService nutritionService) : ControllerBase
    {
        
        [HttpGet("{nutritionId:int}")]
        public async Task<IActionResult> GetNutritionAsync([FromRoute] int nutritionId)
        {
            var result = await nutritionService.GetByIdAsync(nutritionId);
            return Ok(result);
        }
        
        [HttpGet("user/{userId:int}")]
        public async Task<IActionResult> GetAllNutritionAsync([FromRoute] int userId)
        {
            var result = await nutritionService.GetAllAsync(userId);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateNutritionAsync([FromBody] CreateNutritionRequestDto createNutritionRequestDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            
            var result = await nutritionService.CreateAsync(createNutritionRequestDto);
            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateNutritionAsync([FromBody] UpdateNutritionRequestDto updateNutritionRequestDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            
            await nutritionService.UpdateAsync(updateNutritionRequestDto);
            return Ok("Updated");
        }

        [HttpDelete("{nutritionId:int}")]
        public async Task<IActionResult> DeleteNutritionAsync([FromRoute] int nutritionId)
        { 
            await nutritionService.DeleteAsync(nutritionId);
            return Ok("Deleted");
        }
    }
}
