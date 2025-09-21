using Microsoft.AspNetCore.Mvc;
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
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTrainingsAsync()
        {
            var result = await trainingService.GetAllAsync();
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTrainingAsync(int userId, string description, string dateString, double duration, bool isCompleted)
        {
            var user = await userService.GetByIdAsync(userId);
            _ = DateTime.TryParse(dateString, out DateTime date);
            await trainingService.CreateAsync(userId, description, date, duration, isCompleted);
            return NoContent();
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateTrainingAsync([FromRoute] int id, int userId, string description, string dateString, double duration, bool isCompleted)
        {
            _ = DateTime.TryParse(dateString, out DateTime date);
            await trainingService.UpdateAsync(id, userId, description, date, duration, isCompleted);
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
