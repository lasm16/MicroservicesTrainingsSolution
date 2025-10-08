using Microsoft.AspNetCore.Mvc;
using TrainingsApi.BLL;
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
        public async Task<IActionResult> CreateTrainingAsync([FromBody] TrainingDto dto)
        {
            await trainingService.CreateAsync(dto);
            return NoContent();
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateTrainingAsync([FromRoute] int id, [FromBody] TrainingDto dto)
        {
            dto.Id = id;
            await trainingService.UpdateAsync(dto);
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteTrainingAsync([FromRoute] int id)
        {
            await trainingService.DeleteAsync(id);
            return NoContent();
        }
    }
}
