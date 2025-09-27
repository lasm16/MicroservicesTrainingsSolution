using AchievementsApi.BLL.Services;
using Microsoft.AspNetCore.Mvc;

namespace AchievementsApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AchievementsController(IAchievementService achievementService) : ControllerBase
    {
        private readonly IAchievementService _achievementService = achievementService;

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetAchievementAsync([FromRoute] int userId)
        {
            var response = await _achievementService.GetByIdAsync(userId);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTrainingsAsync()
        {
            var result = await _achievementService.GetAllAsync();
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTrainingAsync(int userId, string description, string dateString, double duration, bool isCompleted)
        {
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
            await _achievementService.DeleteAsync(id);
            return NoContent();
        }
    }
}
