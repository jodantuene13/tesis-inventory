using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TesisInventory.Application.Interfaces;

namespace TesisInventory.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InformesController : ControllerBase
    {
        private readonly IInformesService _informesService;

        public InformesController(IInformesService informesService)
        {
            _informesService = informesService;
        }

        private int? TryGetCurrentSedeId()
        {
            if (Request.Headers.TryGetValue("Sede-Contexto", out var sedeContexto) &&
                int.TryParse(sedeContexto, out int headerSedeId))
            {
                return headerSedeId;
            }

            var sedeIdClaim = User.FindFirst("sede_id")?.Value;
            if (int.TryParse(sedeIdClaim, out int sId) && sId > 0)
                return sId;

            return null;
        }

        /// <summary>
        /// GET /api/informes/alertas-stock
        /// Devuelve los datos para el informe de stock bajo y alertas.
        /// Parámetros opcionales: idSede, idFamilia, semanas (default 5).
        /// Si el usuario es Admin y no especifica sede, se retornan datos de todas las sedes.
        /// </summary>
        [HttpGet("alertas-stock")]
        public async Task<IActionResult> GetAlertasStock(
            [FromQuery] int? idSede = null,
            [FromQuery] int? idFamilia = null,
            [FromQuery] int semanas = 5)
        {
            try
            {
                // Si el front no envía idSede explícito, intentamos obtenerlo del contexto
                var sedeEfectiva = idSede ?? TryGetCurrentSedeId();

                var resultado = await _informesService.GetAlertasStockAsync(sedeEfectiva, idFamilia, semanas);
                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }
    }
}
