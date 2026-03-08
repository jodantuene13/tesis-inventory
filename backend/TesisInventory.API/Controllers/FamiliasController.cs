using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TesisInventory.Application.DTOs.Familias;
using TesisInventory.Application.Interfaces;

namespace TesisInventory.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class FamiliasController : ControllerBase
    {
        private readonly IFamiliasService _familiasService;

        public FamiliasController(IFamiliasService familiasService)
        {
            _familiasService = familiasService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] bool includeInactive = false)
        {
            var result = await _familiasService.GetAllFamiliasAsync(includeInactive);
            return Ok(result);
        }

        [HttpGet("rubro/{idRubro}")]
        public async Task<IActionResult> GetByRubro(int idRubro, [FromQuery] bool includeInactive = false)
        {
            var result = await _familiasService.GetFamiliasByRubroAsync(idRubro, includeInactive);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _familiasService.GetFamiliaByIdAsync(id);
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateFamiliaDto dto)
        {
            var result = await _familiasService.CreateFamiliaAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = result.IdFamilia }, result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateFamiliaDto dto)
        {
            var result = await _familiasService.UpdateFamiliaAsync(id, dto);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _familiasService.DeleteFamiliaAsync(id);
            return NoContent();
        }

        [HttpGet("{id}/asociaciones")]
        public async Task<IActionResult> GetAsociaciones(int id)
        {
            var result = await _familiasService.GetAsociacionesAsync(id);
            return Ok(result);
        }
    }
}
