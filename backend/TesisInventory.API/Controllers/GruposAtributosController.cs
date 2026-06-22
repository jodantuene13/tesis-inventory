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
    public class GruposAtributosController : ControllerBase
    {
        private readonly IGruposAtributosService _service;

        public GruposAtributosController(IGruposAtributosService service)
        {
            _service = service;
        }

        // --- GRUPOS MAESTROS ---

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] bool includeInactive = false)
        {
            var result = await _service.GetAllGruposAsync(includeInactive);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _service.GetGrupoByIdAsync(id);
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateGrupoAtributoDto dto)
        {
            var result = await _service.CreateGrupoAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = result.IdGrupoAtributo }, result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateGrupoAtributoDto dto)
        {
            var result = await _service.UpdateGrupoAsync(id, dto);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteGrupoAsync(id);
            return NoContent();
        }

        // --- ITEMS DEL GRUPO ---

        [HttpPost("{idGrupo}/items")]
        public async Task<IActionResult> AddItem(int idGrupo, AddItemToGrupoDto dto)
        {
            var result = await _service.AddItemAsync(idGrupo, dto);
            return Ok(result);
        }

        [HttpDelete("{idGrupo}/items/{idAtributo}")]
        public async Task<IActionResult> DeleteItem(int idGrupo, int idAtributo)
        {
            await _service.DeleteItemAsync(idGrupo, idAtributo);
            return NoContent();
        }

        // --- ASIGNACIÓN A FAMILIA ---

        [HttpGet("familia/{idFamilia}")]
        public async Task<IActionResult> GetGruposDeFamilia(int idFamilia)
        {
            var result = await _service.GetGruposDeFamiliaAsync(idFamilia);
            return Ok(result);
        }

        [HttpPost("familia/{idFamilia}")]
        public async Task<IActionResult> AssignGrupoToFamilia(int idFamilia, CreateFamiliaGrupoAtributoDto dto)
        {
            var result = await _service.AssignGrupoToFamiliaAsync(idFamilia, dto);
            return Ok(result);
        }

        [HttpDelete("familia/{idFamilia}/grupo/{idGrupo}")]
        public async Task<IActionResult> RemoveGrupoFromFamilia(int idFamilia, int idGrupo)
        {
            await _service.RemoveGrupoFromFamiliaAsync(idFamilia, idGrupo);
            return NoContent();
        }
    }
}
