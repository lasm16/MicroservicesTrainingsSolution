using AchievementsApi.Abstractions;
using AchievementsApi.BLL.DTO.Requests;
using Microsoft.AspNetCore.Mvc;

namespace AchievementsApi.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class AchievementsController(IAchievementService achievementService) : ControllerBase
    {
        [HttpGet("{achievementId:int}")]
        public async Task<IActionResult> GetAchievementAsync([FromRoute] int achievementId)
        {
            var response = await achievementService.GetByIdAsync(achievementId);
            return response == null ? BadRequest($"Не найдено достижение с id={achievementId}!") : Ok(response);
        }

        [HttpGet("user/{userId:int}")]
        public async Task<IActionResult> GetAllAchievementsAsync([FromRoute] int userId)
        {
            var result = await achievementService.GetAllByUserIdAsync(userId);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAchievementAsync([FromBody] AchievementCreateRequest request)
        {
            var result = await achievementService.CreateAsync(request);
            return result ? Ok("Created") : BadRequest("Не удалось создать достижение");
        }

        [HttpPut]
        public async Task<IActionResult> UpdateAchievementAsync([FromBody] AchievementUpdateRequest request)
        {
            var result = await achievementService.UpdateAsync(request);
            return result ? Ok("Updated") : BadRequest($"Не найдено достижение с id={request.Id}!");
        }

        [HttpDelete("{achievementId:int}")]
        public async Task<IActionResult> DeleteAchievementAsync([FromRoute] int achievementId)
        {
            var result = await achievementService.DeleteAsync(achievementId);
            return result ? Ok("Deleted") : BadRequest($"Не найдено достижение с id={achievementId}!");
        }
    }
}
