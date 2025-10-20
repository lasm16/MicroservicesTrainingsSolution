using AchievementsApi.Abstractions;
using AchievementsApi.BLL.DTO.Requests;
using Microsoft.AspNetCore.Mvc;

namespace AchievementsApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AchievementsController(IAchievementService achievementService) : ControllerBase
    {
        private readonly IAchievementService _achievementService = achievementService;

        [HttpGet("get/{id:int}")]
        public async Task<IActionResult> GetAchievementAsync([FromRoute] int id)
        {
            var response = await _achievementService.GetByIdAsync(id);
            return response == null ? BadRequest($"Не найдено достижение с id={id}!") : Ok(response);
        }

        [HttpGet("get-all/{userId:int}")]
        public async Task<IActionResult> GetAllAchievementsAsync([FromRoute] int userId)
        {
            var result = await _achievementService.GetAllByUserIdAsync(userId);
            return Ok(result);
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateAchievementAsync([FromBody] AchievementCreateRequest request)
        {
            var result = await _achievementService.CreateAsync(request);
            return result ? Ok("Created") : BadRequest("Не удалось создать достижение");
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateAchievementAsync([FromBody] AchievementUpdateRequest request)
        {
            var result = await _achievementService.UpdateAsync(request);
            return result ? Ok("Updated") : BadRequest($"Не найдено достижение с id={request.Id}!");
        }

        [HttpDelete("delete/{id:int}")]
        public async Task<IActionResult> DeleteAchievementAsync([FromRoute] int id)
        {
            var result = await _achievementService.DeleteAsync(id);
            return result ? Ok("Deleted") : BadRequest($"Не найдено достижение с id={id}!");
        }
    }
}
