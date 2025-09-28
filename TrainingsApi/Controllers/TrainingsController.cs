using Microsoft.AspNetCore.Mvc;
using TrainingsApi.BLL;
using TrainingsApi.BLL.Services;
using UsersApi.BLL.Services;

namespace TrainingsApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TrainingsController(ITrainingService trainingService, IUserService userService) : ControllerBase
    {
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetTrainingAsync([FromRoute] int id)
        {
            var result = await trainingService.GetByIdAsync(id);
            return result is null ? NotFound() : Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTrainingsAsync()
        {
            var result = await trainingService.GetAllAsync();
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTrainingAsync([FromBody] TrainingDto dto)
        {
            var user = await userService.GetByIdAsync(dto.UserId);
            if (user is null) return NotFound($"User with id {dto.UserId} not found");

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
