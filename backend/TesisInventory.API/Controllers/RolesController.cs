using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TesisInventory.Application.Interfaces;

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
        public async Task<IActionResult> Create([FromBody] TesisInventory.Domain.Entities.Rol rol)
        {
            var createdRole = await _rolesService.CreateRoleAsync(rol);
            return CreatedAtAction(nameof(GetById), new { id = createdRole.IdRol }, createdRole);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] TesisInventory.Domain.Entities.Rol rol)
        {
            if (id != rol.IdRol) return BadRequest();
            await _rolesService.UpdateRoleAsync(rol);
            return NoContent();
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
    }
}
