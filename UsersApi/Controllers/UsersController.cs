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
            var userDto = await userService.GetByIdAsync(id, ct);

            if (userDto == null)
                return NotFound();

            var fullName = $"{userDto.Name} {userDto.Surname}".Trim();
            return Ok(new { userName = fullName });
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsersAsync(CancellationToken ct)
        {
            var usersDto = await userService.GetAllAsync(ct);

            var result = usersDto
                .Select(u => new
                {
                    userName = $"{u.Name} {u.Surname}".Trim()
                })
                .ToList();

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateUserAsync(
            [FromBody] CreateUserDTO request,
            CancellationToken ct)
        {
            await userService.CreateAsync(request, ct);
            return NoContent();
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateUserAsync(
            [FromRoute] int id,
            [FromBody] UpdateUserDTO request,
            CancellationToken ct)
        {
            var success = await userService.UpdateAsync(request, ct);
            return success ? NoContent() : NotFound();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteUserAsync([FromRoute] int id, CancellationToken ct)
        {
            await userService.DeleteAsync(id, ct);
            return NoContent();
        }
    }
}
