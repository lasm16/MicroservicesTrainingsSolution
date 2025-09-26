using Microsoft.AspNetCore.Mvc;
using UsersApi.BLL.Models;
using UsersApi.BLL.Services;

namespace UsersApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController(IUserService userService) : ControllerBase
    {
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetUserAsync([FromRoute] int id, CancellationToken cancellationToken)
        {
            var userDto = await userService.GetByIdAsync(id, cancellationToken);

            if (userDto == null)
                return NotFound();

            var fullName = $"{userDto.Name} {userDto.Surname}".Trim();
            return Ok(new { userName = fullName });
        }

        [HttpGet]
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

        [HttpPost]
        public async Task<IActionResult> CreateUserAsync(
            [FromBody] UserRequest request,
            CancellationToken cancellationToken)
        {
            await userService.CreateAsync(request, cancellationToken);
            return NoContent();
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateUserAsync(            
            [FromBody] UserRequest request,
            CancellationToken cancellationToken)
        {
            var success = await userService.UpdateAsync(request, cancellationToken);
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteUserAsync([FromRoute] int id, CancellationToken cancellationToken)
        {
            await userService.DeleteAsync(id, cancellationToken);
            return NoContent();
        }
    }
}
