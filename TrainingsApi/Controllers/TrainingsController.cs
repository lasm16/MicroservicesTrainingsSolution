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
        public async Task<IActionResult> GetAllTrainingsAsync(int id)
        {
            var result = await trainingService.GetAllAsync(id);
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
        public async Task<IActionResult> UpdateTrainingStatus([FromRoute] TrainingStatusUpdateDto dto)
        {
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
        [HttpPost("{id:int}/start")]
        public async Task<IActionResult> StartTrainingAsync([FromRoute] int id)
        {
            await trainingService.StartTrainingAsync(id);
            return NoContent();
        }

        [HttpPost("{id:int}/complete")]
        public async Task<IActionResult> CompleteTrainingAsync([FromRoute] int id)
        {
            await trainingService.CompleteTrainingAsync(id);
            return NoContent();
        }

        [HttpPost("{id:int}/cancel")]
        public async Task<IActionResult> CancelTrainingAsync([FromRoute] int id)
        {
            await trainingService.CancelTrainingAsync(id);
            return NoContent();
        }
    }
}
