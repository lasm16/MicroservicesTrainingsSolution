using Microsoft.AspNetCore.Mvc;
using System.Threading;
using TrainingsApi.BLL.Dtos;
using TrainingsApi.BLL.Services;

namespace TrainingsApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TrainingsController(ITrainingService trainingService) : ControllerBase
    {
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetTrainingAsync([FromRoute] int id)
        {
            var result = await trainingService.GetByIdAsync(id);
            return result is null ? NotFound() : Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTrainingsAsync([FromRoute] int userId)
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

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateTrainingAsync([FromRoute] TrainingUpdateDto dto)
        {
            await trainingService.UpdateAsync(dto);
            return NoContent();
        }
        [HttpPatch("{id:int}/status")]
        public async Task<IActionResult> UpdateTrainingStatus([FromRoute] int id, [FromBody] TrainingStatusUpdateDto dto)
        {
            if (id != dto.Id)
                return BadRequest("Route id and body id do not match.");

            await trainingService.UpdateStatusAsync(dto);
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteTrainingAsync([FromRoute] int id)
        {
            var dto = new TrainingDeleteDto
            {
                Id = id,
                IsDeleted = true,
                Updated = DateTime.UtcNow
            };

            await trainingService.DeleteAsync(dto);
            return NoContent();
        }
    }
}
