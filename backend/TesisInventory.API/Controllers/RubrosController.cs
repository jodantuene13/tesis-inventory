using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TesisInventory.Application.DTOs.Rubros;
using TesisInventory.Application.Interfaces;

namespace TesisInventory.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class RubrosController : ControllerBase
    {
        private readonly IRubrosService _rubrosService;

        public RubrosController(IRubrosService rubrosService)
        {
            _rubrosService = rubrosService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] bool includeInactive = false)
        {
            var result = await _rubrosService.GetAllRubrosAsync(includeInactive);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _rubrosService.GetRubroByIdAsync(id);
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateRubroDto dto)
        {
            var result = await _rubrosService.CreateRubroAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = result.IdRubro }, result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateRubroDto dto)
        {
            var result = await _rubrosService.UpdateRubroAsync(id, dto);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _rubrosService.DeleteRubroAsync(id);
            return NoContent();
        }
    }
}
