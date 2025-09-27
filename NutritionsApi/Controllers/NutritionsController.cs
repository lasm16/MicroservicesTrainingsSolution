using Microsoft.AspNetCore.Mvc;
using NutritionsApi.Abstractions;
using NutritionsApi.BLL.DTO;
using NutritionsApi.BLL.DTO.RequestDto;

namespace NutritionsApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NutritionsController(INutritionService nutritionService) : ControllerBase
    {
        private readonly INutritionService _nService = nutritionService;
        
        [HttpGet("get/{id:int}")]
        public async Task<IActionResult> GetNutritionAsync([FromRoute] int id)
        {
            var result = await _nService.GetByIdAsync(id);
            return Ok(result);
        }
        
        [HttpGet("get-all/{userId:int}")]
        public async Task<IActionResult> GetAllNutritionAsync([FromRoute] int userId)
        {
            var result = await _nService.GetAllAsync(userId);
            return Ok(result);
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateNutritionAsync([FromBody] CreateNutritionRequestDto createNutritionRequestDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            
            var result = await _nService.CreateAsync(createNutritionRequestDto);
            return Ok(result);
        }

        [HttpPost("update")]
        public async Task<IActionResult> UpdateNutritionAsync([FromBody] UpdateNutritionRequestDto updateNutritionRequestDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            
            await _nService.UpdateAsync(updateNutritionRequestDto);
            return Ok("Updated");
        }

        [HttpDelete("delete/{id:int}")]
        public async Task<IActionResult> DeleteNutritionAsync([FromRoute] int id)
        { 
            await _nService.DeleteAsync(id);
            return Ok("Deleted");
        }
    }
}
