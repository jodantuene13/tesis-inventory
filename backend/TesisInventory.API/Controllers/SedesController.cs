using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;
using TesisInventory.Application.DTOs.Sedes;
using TesisInventory.Application.Exceptions;
using TesisInventory.Application.Interfaces;
using TesisInventory.API.Filters;

namespace TesisInventory.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SedesController : ControllerBase
    {
        private readonly ISedesService _sedesService;

        public SedesController(ISedesService sedesService)
        {
            _sedesService = sedesService;
        }

        private int GetCurrentUserId()
        {
            var claim = User.FindFirstValue("id") ?? User.FindFirstValue(ClaimTypes.NameIdentifier);
            return int.TryParse(claim, out var id) ? id : 0;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var sedes = await _sedesService.GetAllSedesAsync();
            return Ok(sedes);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var sede = await _sedesService.GetSedeByIdAsync(id);
            if (sede == null) return NotFound();
            return Ok(sede);
        }

        [HttpPost]
        [RequirePermiso("ConfiguracionAdmin_Ver")]
        public async Task<IActionResult> Create([FromBody] CreateSedeDto createDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var sede = await _sedesService.CreateSedeAsync(createDto);
            return CreatedAtAction(nameof(GetById), new { id = sede.IdSede }, sede);
        }

        [HttpPut("{id}")]
        [RequirePermiso("ConfiguracionAdmin_Ver")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateSedeDto updateDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var sede = await _sedesService.UpdateSedeAsync(id, updateDto);
            if (sede == null) return NotFound();

            return Ok(sede);
        }

        [HttpDelete("{id}")]
        [RequirePermiso("ConfiguracionAdmin_Ver")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var deleted = await _sedesService.DeleteSedeAsync(id);
                if (!deleted) return NotFound();
                return NoContent();
            }
            catch (SedeDeleteBloqueadaException ex)
            {
                return BadRequest(new { bloqueantes = ex.Bloqueantes });
            }
            catch (System.InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Microsoft.EntityFrameworkCore.DbUpdateException)
            {
                return StatusCode(500, new { message = "Error de base de datos al intentar eliminar la sede." });
            }
        }

        [HttpPost("{id}/transferir-stock")]
        [RequirePermiso("ConfiguracionAdmin_Ver")]
        public async Task<IActionResult> TransferirStock(int id, [FromBody] TransferirStockDto dto)
        {
            var userId = GetCurrentUserId();
            if (userId == 0) return Unauthorized();

            try
            {
                await _sedesService.TransferirStockAsync(id, dto.IdSedeDestino, userId);
                return NoContent();
            }
            catch (System.InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
