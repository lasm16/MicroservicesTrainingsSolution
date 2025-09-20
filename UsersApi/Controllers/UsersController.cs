using Microsoft.AspNetCore.Mvc;
using UsersApi.BLL.Services;

namespace UsersApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController(IUserService userService) : ControllerBase
    {
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetUserAsync([FromRoute] int id)
        {
            var result = await userService.GetByIdAsync(id);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsersAsync()
        {
            var result = await userService.GetAllAsync();
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateUserAsync(string name, string email)
        {
            await userService.CreateAsync(name, email);
            return NoContent();
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateUserAsync([FromRoute] int id, string userName, string email)
        {
            await userService.UpdateAsync(id, userName, email);
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteUserAsync([FromRoute] int id)
        {
            await userService.DeleteAsync(id);
            return NoContent();
        }
    }
}
