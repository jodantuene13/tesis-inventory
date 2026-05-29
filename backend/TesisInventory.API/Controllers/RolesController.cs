using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TesisInventory.Application.Interfaces;
using TesisInventory.Application.DTOs.Roles;

namespace TesisInventory.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly IRolesService _rolesService;

        public RolesController(IRolesService rolesService)
        {
            _rolesService = rolesService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var roles = await _rolesService.GetAllRolesAsync();
            return Ok(roles);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var role = await _rolesService.GetRoleByIdAsync(id);
            if (role == null) return NotFound();
            return Ok(role);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] RolCreateDto rolDto)
        {
            var createdRole = await _rolesService.CreateRoleAsync(rolDto);
            return CreatedAtAction(nameof(GetById), new { id = createdRole.IdRol }, createdRole);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] RolUpdateDto rolDto)
        {
            if (id != rolDto.IdRol) return BadRequest();
            
            try
            {
                await _rolesService.UpdateRoleAsync(rolDto);
                return NoContent();
            }
            catch (System.ArgumentException)
            {
                return NotFound();
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var result = await _rolesService.DeleteRoleAsync(id);
                if (!result) return NotFound();
                return NoContent();
            }
            catch (System.InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }

        [HttpGet("permisos")]
        public async Task<IActionResult> GetPermisos()
        {
            var permisos = await _rolesService.GetAllPermisosAsync();
            return Ok(permisos);
        }
    }
}
