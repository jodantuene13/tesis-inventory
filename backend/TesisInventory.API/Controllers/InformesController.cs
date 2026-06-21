using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using TesisInventory.Application.DTOs.Informes;
using TesisInventory.Application.Interfaces;
using TesisInventory.Domain.Enums;

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
                var sedeEfectiva = idSede ?? TryGetCurrentSedeId();
                var resultado = await _informesService.GetAlertasStockAsync(sedeEfectiva, idFamilia, semanas);
                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        /// <summary>
        /// GET /api/informes/rotacion-productos
        /// Devuelve el informe de rotación de productos con tres rankings:
        /// índice de rotación, mayor ingreso y mayor egreso.
        /// Parámetros: idSede (opcional), idFamilia (opcional),
        ///             fechaDesde (yyyy-MM-dd), fechaHasta (yyyy-MM-dd).
        /// </summary>
        [HttpGet("rotacion-productos")]
        public async Task<IActionResult> GetRotacionProductos(
            [FromQuery] int? idSede = null,
            [FromQuery] int? idFamilia = null,
            [FromQuery] string? fechaDesde = null,
            [FromQuery] string? fechaHasta = null)
        {
            try
            {
                // Sede efectiva (misma lógica que alertas-stock)
                var sedeEfectiva = idSede ?? TryGetCurrentSedeId();

                // Período por defecto: últimos 30 días
                var hoy = DateTime.UtcNow.Date;
                var desde = fechaDesde != null && DateTime.TryParse(fechaDesde, out var d) ? d : hoy.AddDays(-30);
                var hasta = fechaHasta != null && DateTime.TryParse(fechaHasta, out var h) ? h : hoy;

                var resultado = await _informesService.GetRotacionProductosAsync(
                    sedeEfectiva, idFamilia, desde, hasta);

                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }
        [HttpGet("stock-inmovilizado")]
        public async Task<IActionResult> GetStockInmovilizado(
            [FromQuery] int? idSede = null,
            [FromQuery] int? idFamilia = null,
            [FromQuery] string? fechaDesde = null,
            [FromQuery] string? fechaHasta = null)
        {
            try
            {
                var sedeEfectiva = idSede ?? TryGetCurrentSedeId();
                var hoy = DateTime.UtcNow.Date;
                var desde = fechaDesde != null && DateTime.TryParse(fechaDesde, out var d) ? d : hoy.AddDays(-30);
                var hasta = fechaHasta != null && DateTime.TryParse(fechaHasta, out var h) ? h : hoy;

                var resultado = await _informesService.GetStockInmovilizadoAsync(sedeEfectiva, idFamilia, desde, hasta);
                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpGet("familias-consumo")]
        public async Task<IActionResult> GetFamiliasConsumo(
            [FromQuery] int? idSede = null,
            [FromQuery] int? idFamilia = null,
            [FromQuery] string? fechaDesde = null,
            [FromQuery] string? fechaHasta = null)
        {
            try
            {
                var sedeEfectiva = idSede ?? TryGetCurrentSedeId();
                var hoy = DateTime.UtcNow.Date;
                var desde = fechaDesde != null && DateTime.TryParse(fechaDesde, out var d) ? d : hoy.AddDays(-30);
                var hasta = fechaHasta != null && DateTime.TryParse(fechaHasta, out var h) ? h : hoy;

                var resultado = await _informesService.GetFamiliasConsumoAsync(sedeEfectiva, idFamilia, desde, hasta);
                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpGet("solicitudes-compra")]
        public async Task<IActionResult> GetInformeSolicitudesCompra(
            [FromQuery] int? idSede = null,
            [FromQuery] string? fechaDesde = null,
            [FromQuery] string? fechaHasta = null,
            [FromQuery] int topN = 10)
        {
            try
            {
                var sedeEfectiva = idSede ?? TryGetCurrentSedeId();
                var hoy = DateTime.UtcNow.Date;
                var desde = fechaDesde != null && DateTime.TryParse(fechaDesde, out var d) ? d : hoy.AddDays(-30);
                var hasta = fechaHasta != null && DateTime.TryParse(fechaHasta, out var h) ? h : hoy;

                var resultado = await _informesService.GetInformeSolicitudesCompraAsync(
                    sedeEfectiva, desde, hasta, topN);
                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpGet("transferencias")]
        // [Authorize(Roles = "Admin,Deposito")]
        public async Task<ActionResult<InformeTransferenciaDto>> GetInformeTransferencias(
            [FromQuery] int? idSedeOrigen = null,
            [FromQuery] int? idSedeDestino = null,
            [FromQuery] int? idFamilia = null,
            [FromQuery] MotivoTransferencia? motivo = null,
            [FromQuery] EstadoTransferencia? estado = null,
            [FromQuery] DateTime? fechaDesde = null,
            [FromQuery] DateTime? fechaHasta = null,
            [FromQuery] int topN = 10)
        {
            try
            {
                // Por defecto últimos 30 días
                var hasta = fechaHasta ?? DateTime.UtcNow;
                var desde = fechaDesde ?? hasta.AddDays(-30);

                var result = await _informesService.GetInformeTransferenciasAsync(
                    idSedeOrigen, idSedeDestino, idFamilia, motivo, estado, desde, hasta, topN);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error al obtener el informe de transferencias y préstamos", error = ex.Message });
            }
        }
    }
}

