using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TesisInventory.Application.DTOs;
using TesisInventory.Application.Interfaces;

namespace TesisInventory.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Microsoft.AspNetCore.Authorization.Authorize]
    public class UsersController : ControllerBase
    {
        private readonly IUsersService _usersService;

        public UsersController(IUsersService usersService)
        {
            _usersService = usersService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string? search, [FromQuery] int? roleId, [FromQuery] int? sedeId, [FromQuery] bool? status)
        {
            if (string.IsNullOrEmpty(search) && !roleId.HasValue && !sedeId.HasValue && !status.HasValue)
            {
                var users = await _usersService.GetAllUsersAsync();
                return Ok(users);
            }
            else
            {
                var users = await _usersService.SearchUsersAsync(search, roleId, sedeId, status);
                return Ok(users);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var user = await _usersService.GetUserByIdAsync(id);
            if (user == null) return NotFound();
            return Ok(user);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateUserDto createUserDto)
        {
            try
            {
                var newUser = await _usersService.CreateUserAsync(createUserDto);
                return CreatedAtAction(nameof(GetById), new { id = newUser.IdUsuario }, newUser);
            }
            catch (System.Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateUserDto updateUserDto)
        {
            try
            {
                var updatedUser = await _usersService.UpdateUserAsync(id, updateUserDto);
                return Ok(updatedUser);
            }
            catch (System.Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _usersService.DeleteUserAsync(id);
            if (!result) return NotFound();
            return NoContent();
        }

        [HttpPatch("{id}/status")]
        public async Task<IActionResult> ChangeStatus(int id, [FromBody] ChangeUserStatusDto statusDto)
        {
            try
            {
                var updatedUser = await _usersService.ChangeUserStatusAsync(id, statusDto.Estado);
                return Ok(updatedUser);
            }
            catch (System.Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        
        [HttpPatch("{id}/password")]
        public async Task<IActionResult> ChangePassword(int id, [FromBody] ChangePasswordDto passwordDto)
        {
             var result = await _usersService.ChangePasswordAsync(id, passwordDto.NewPassword);
             if (!result) return NotFound();
             return NoContent();
        }

        [HttpGet("filter/role/{roleId}")]
        public async Task<IActionResult> GetByRole(int roleId)
        {
            var users = await _usersService.GetUsersByRoleAsync(roleId);
            return Ok(users);
        }

        [HttpGet("filter/sede/{sedeId}")]
        public async Task<IActionResult> GetBySede(int sedeId)
        {
            var users = await _usersService.GetUsersBySedeAsync(sedeId);
            return Ok(users);
        }
    }
}
