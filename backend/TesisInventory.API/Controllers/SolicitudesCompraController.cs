using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TesisInventory.Application.DTOs.SolicitudesCompra;
using TesisInventory.Application.Interfaces;
using TesisInventory.Domain.Enums;

namespace TesisInventory.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SolicitudesCompraController : ControllerBase
    {
        private readonly ISolicitudCompraService _solicitudService;

        public SolicitudesCompraController(ISolicitudCompraService solicitudService)
        {
            _solicitudService = solicitudService;
        }

        private int GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? User.FindFirst("id")?.Value;
            if (int.TryParse(userIdClaim, out int userId)) return userId;
            throw new UnauthorizedAccessException("Usuario no autenticado.");
        }

        private int GetCurrentSedeId()
        {
            if (Request.Headers.TryGetValue("Sede-Contexto", out var sedeContexto) && int.TryParse(sedeContexto, out int headerSedeId))
                return headerSedeId;

            var sedeIdClaim = User.FindFirst("sede_id")?.Value;
            if (int.TryParse(sedeIdClaim, out int sId)) return sId;

            throw new UnauthorizedAccessException("Debe especificar la Sede en Contexto.");
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] string? search = null,
            [FromQuery] int? estado = null,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            try
            {
                // Si es admin puede ver de todas las sedes o la seleccionada.
                // Por ahora usamos la sede en contexto.
                int idSede = GetCurrentSedeId();
                var (items, totalCount) = await _solicitudService.GetPagedSolicitudesAsync(idSede, search, estado, page, pageSize);

                return Ok(new
                {
                    Data = items,
                    TotalCount = totalCount,
                    Page = page,
                    PageSize = pageSize,
                    TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize)
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateSolicitudCompraDto dto)
        {
            try
            {
                int idUsuario = GetCurrentUserId();
                int idSede = GetCurrentSedeId();

                var result = await _solicitudService.CreateSolicitudAsync(idSede, idUsuario, dto);
                return CreatedAtAction(nameof(GetById), new { id = result.IdSolicitudCompra }, result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _solicitudService.GetByIdAsync(id);
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpPut("{id}/estado")]
        // [Authorize(Roles = "Admin,Administrador")] // Opcional: solo admins pueden aprobar/rechazar
        public async Task<IActionResult> UpdateEstado(int id, [FromBody] UpdateSolicitudCompraEstadoDto dto)
        {
            try
            {
                int idUsuarioAprobador = GetCurrentUserId();
                var result = await _solicitudService.UpdateEstadoAsync(id, idUsuarioAprobador, dto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }
    }
}
