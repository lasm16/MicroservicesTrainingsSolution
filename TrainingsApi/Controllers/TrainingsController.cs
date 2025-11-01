using Microsoft.AspNetCore.Mvc;
using TrainingsApi.Abstractions;
using TrainingsApi.BLL.Dtos;

namespace TrainingsApi.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class TrainingsController(ITrainingService trainingService) : ControllerBase
    {
        [HttpGet("{trainingId:int}")]
        public async Task<IActionResult> GetTrainingAsync([FromRoute] int trainingId)
        {
            var result = await trainingService.GetByIdAsync(trainingId);
            return result is null ? NotFound() : Ok(result);
        }

        [HttpGet("user/{userId:int}")]
        public async Task<IActionResult> GetAllTrainingsAsync(int userId)
        {
            if (userId <= 0) return BadRequest("userId is required");
            var result = await trainingService.GetAllAsync(userId);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTrainingAsync([FromBody] TrainingCreateDto dto)
        {
            await trainingService.CreateAsync(dto);
            return NoContent();
        }

        [HttpPut]
        public async Task<IActionResult> UpdateTrainingAsync([FromBody] TrainingUpdateDto dto)
        {
            await trainingService.UpdateAsync(dto);
            return NoContent();
        }
        [HttpPatch("{id:int}/status")]
        public async Task<IActionResult> UpdateTrainingStatus([FromRoute] TrainingStatusUpdateDto dto)
        {
            await trainingService.UpdateStatusAsync(dto);
            return NoContent();
        }

        [HttpPatch("status")]
        public async Task<IActionResult> UpdateTrainingStatus([FromBody] TrainingStatusUpdateDto dto)
        {
            await trainingService.UpdateStatusAsync(dto);
            return NoContent();
        }

        [HttpDelete("{trainingId:int}")]
        public async Task<IActionResult> DeleteTrainingAsync([FromRoute] int trainingId)
        {
            await trainingService.DeleteAsync(trainingId);
            return NoContent();
        }
    }
}
