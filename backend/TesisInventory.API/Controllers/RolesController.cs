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
    }
}
