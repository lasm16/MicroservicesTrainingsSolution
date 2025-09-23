using Microsoft.AspNetCore.Mvc;
using UsersApi.BLL.DTO;
using UsersApi.BLL.Models;
using UsersApi.BLL.Services;

namespace UsersApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController(IUserService userService) : ControllerBase
    {
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetUserAsync([FromRoute] int id, CancellationToken ct)
        {
            var result = await userService.GetByIdAsync(id, ct);
            return string.IsNullOrEmpty(result) ? NotFound() : Ok(new { userName = result });
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsersAsync(CancellationToken ct)
        {
            var result = await userService.GetAllAsync(ct);
            return Ok(result.Select(name => new { userName = name }));
        }

        [HttpPost]
        public async Task<IActionResult> CreateUserAsync(
            [FromBody] CreateUserRequest request,
            CancellationToken ct)
        {
            await userService.CreateAsync(request.Name, request.Email, ct);
            return NoContent();
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateUserAsync(
            [FromRoute] int id,
            [FromBody] UpdateUserRequest request,
            CancellationToken ct)
        {
            await userService.UpdateAsync(id, request.Name, request.Email, ct);
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteUserAsync([FromRoute] int id, CancellationToken ct)
        {
            await userService.DeleteAsync(id, ct);
            return NoContent();
        }
    }
}
