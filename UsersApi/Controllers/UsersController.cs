using Microsoft.AspNetCore.Mvc;
using UsersApi.Abstractions;
using UsersApi.BLL.DTOs;

namespace UsersApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController(IUserService userService) : ControllerBase
    {
        [HttpGet("get-user/{id:int}")]
        public async Task<IActionResult> GetUserAsync([FromRoute] int id, CancellationToken cancellationToken)
        {
            var response = await userService.GetByIdAsync(id, cancellationToken);

            if (response == null)
                return NotFound();

            var fullName = $"{response.Name} {response.Surname}".Trim();
            return Ok(new { userName = fullName, response.Achievements, response.Nutritions, response.Trainings });
        }

        [HttpGet("get-all-users")]
        public async Task<IActionResult> GetAllUsersAsync(CancellationToken cancellationToken)
        {
            var usersDto = await userService.GetAllAsync(cancellationToken);

            var result = usersDto
                .Select(u => new
                {
                    userName = $"{u.Name} {u.Surname}".Trim()
                })
                .ToList();

            return Ok(result);
        }

        [HttpPost("api/create")]
        public async Task<IActionResult> CreateUserAsync(
            [FromBody] UserRequest request,
            CancellationToken cancellationToken)
        {
            await userService.CreateAsync(request, cancellationToken);
            return NoContent();
        }

        [HttpPut("update-user")]
        public async Task<IActionResult> UpdateUserAsync(
            [FromBody] UserRequest request,
            CancellationToken cancellationToken)
        {
            await userService.UpdateAsync(request, cancellationToken);
            return NoContent();
        }

        [HttpDelete("delete-user/{id:int}")]
        public async Task<IActionResult> DeleteUserAsync([FromRoute] int id, CancellationToken cancellationToken)
        {
            await userService.DeleteAsync(id, cancellationToken);
            return NoContent();
        }
    }
}
