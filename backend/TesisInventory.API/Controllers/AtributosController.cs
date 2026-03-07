using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TesisInventory.Application.DTOs.Atributos;
using TesisInventory.Application.Interfaces;

namespace TesisInventory.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class AtributosController : ControllerBase
    {
        private readonly IAtributosService _atributosService;

        public AtributosController(IAtributosService atributosService)
        {
            _atributosService = atributosService;
        }

        // --- ATRIBUTOS MAESTROS ---

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] bool includeInactive = false)
        {
            var result = await _atributosService.GetAllAtributosAsync(includeInactive);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _atributosService.GetAtributoByIdAsync(id);
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateAtributoDto dto)
        {
            var result = await _atributosService.CreateAtributoAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = result.IdAtributo }, result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateAtributoDto dto)
        {
            var result = await _atributosService.UpdateAtributoAsync(id, dto);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _atributosService.DeleteAtributoAsync(id);
            return NoContent();
        }

        // --- OPCIONES LISTA ---

        [HttpGet("{idAtributo}/opciones")]
        public async Task<IActionResult> GetOpciones(int idAtributo)
        {
            var result = await _atributosService.GetOpcionesAsync(idAtributo);
            return Ok(result);
        }

        [HttpPost("{idAtributo}/opciones")]
        public async Task<IActionResult> AddOpcion(int idAtributo, CreateAtributoOpcionDto dto)
        {
            var result = await _atributosService.AddOpcionAsync(idAtributo, dto);
            return Ok(result);
        }

        [HttpDelete("opciones/{idOpcion}")]
        public async Task<IActionResult> DeleteOpcion(int idOpcion)
        {
            await _atributosService.DeleteOpcionAsync(idOpcion);
            return NoContent();
        }

        // --- ASIGNACIONES A FAMILIA ---

        [HttpGet("familia/{idFamilia}")]
        public async Task<IActionResult> GetAtributosDeFamilia(int idFamilia)
        {
            var result = await _atributosService.GetAtributosDeFamiliaAsync(idFamilia);
            return Ok(result);
        }

        [HttpPost("familia/{idFamilia}")]
        public async Task<IActionResult> AssignAtributoToFamilia(int idFamilia, CreateFamiliaAtributoDto dto)
        {
            var result = await _atributosService.AssignAtributoToFamiliaAsync(idFamilia, dto);
            return Ok(result);
        }

        [HttpDelete("familia/{idFamilia}/atributo/{idAtributo}")]
        public async Task<IActionResult> RemoveAtributoFromFamilia(int idFamilia, int idAtributo)
        {
            await _atributosService.RemoveAtributoFromFamiliaAsync(idFamilia, idAtributo);
            return NoContent();
        }
    }
}
