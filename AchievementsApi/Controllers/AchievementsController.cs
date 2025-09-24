using AchievementsApi.BLL.Services;
using Microsoft.AspNetCore.Mvc;
using UsersApi.BLL.Services;

namespace AchievementsApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AchievementsController(IAchievementService achievementService, IUserService userService) : ControllerBase
    {
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetTrainingAsync([FromRoute] int id)
        {
            var result = await achievementService.GetByIdAsync(id);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTrainingsAsync()
        {
            var result = await achievementService.GetAllAsync();
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTrainingAsync(int userId, string description, string dateString, double duration, bool isCompleted)
        {
            var user = await userService.GetByIdAsync(userId);
            _ = DateTime.TryParse(dateString, out DateTime date);
            //await achievementService.CreateAsync(userId, description, date, duration, isCompleted);
            return NoContent();
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateTrainingAsync([FromRoute] int id, int userId, string description, string dateString, double duration, bool isCompleted)
        {
            _ = DateTime.TryParse(dateString, out DateTime date);
            //await achievementService.UpdateAsync(id, userId, description, date, duration, isCompleted);
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteTrainingAsync([FromRoute] int id)
        {
            await achievementService.DeleteAsync(id);
            return NoContent();
        }
    }
}
