using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TesisInventory.Application.DTOs.Informes;
using TesisInventory.Application.Interfaces;
using TesisInventory.Domain.Enums;
using TesisInventory.Domain.Interfaces;

namespace TesisInventory.Application.Services
{
    public class InformesService : IInformesService
    {
        private readonly IAlertaStockRepository _alertaRepository;
        private readonly IStockRepository _stockRepository;

        public InformesService(IAlertaStockRepository alertaRepository, IStockRepository stockRepository)
        {
            _alertaRepository = alertaRepository;
            _stockRepository = stockRepository;
        }

        public async Task<InformeAlertasStockDto> GetAlertasStockAsync(
            int? idSede = null,
            int? idFamilia = null,
            int semanas = 5)
        {
            // ── 1. Bajo Stock (alertas activas) ───────────────────────────────────
            var alertasActivas = await _alertaRepository.GetAlertasActivasAsync(idSede, idFamilia);

            var bajoStock = alertasActivas.Select(a => new ProductoAlertaStockDto
            {
                IdAlertaStock = a.IdAlertaStock,
                IdProducto    = a.IdProducto,
                Producto      = a.Producto?.Nombre ?? string.Empty,
                Familia       = a.Producto?.Familia?.Nombre ?? string.Empty,
                Sede          = a.Sede?.NombreSede ?? string.Empty,
                StockActual   = a.StockAlMomento,
                StockMinimo   = a.PuntoReposicion,
                Diferencia    = a.StockAlMomento - a.PuntoReposicion,
                DiasEnAlerta  = (int)(DateTime.UtcNow - a.FechaCreacion).TotalDays,
                UltimaAlerta  = a.FechaUltimaAlerta,
                Criticidad    = CalcularCriticidad(a.StockAlMomento, a.PuntoReposicion)
            }).ToList();

            // ── 2. Recurrencia (historial últimas N semanas) ───────────────────────
            var fechaDesde = DateTime.UtcNow.AddDays(-(semanas * 7));
            var historial  = await _alertaRepository.GetHistorialAlertasAsync(idSede, idFamilia, fechaDesde, null);

            // Agrupar por producto + sede para calcular recurrencia
            var recurrencia = historial
                .GroupBy(a => new { a.IdProducto, a.IdSede })
                .Select(g =>
                {
                    var ultimo = g.OrderByDescending(a => a.FechaUltimaAlerta).First();
                    int diasAcumulados = g.Sum(a =>
                    {
                        var fin = a.Estado == EstadoAlerta.Resuelta ? a.FechaUltimaAlerta : DateTime.UtcNow;
                        return Math.Max(0, (int)(fin - a.FechaCreacion).TotalDays);
                    });

                    return new ProductoRecurrenciaDto
                    {
                        IdProducto      = g.Key.IdProducto,
                        Producto        = ultimo.Producto?.Nombre ?? string.Empty,
                        Familia         = ultimo.Producto?.Familia?.Nombre ?? string.Empty,
                        Sede            = ultimo.Sede?.NombreSede ?? string.Empty,
                        CantidadAlertas = g.Count(),
                        DiasAcumulados  = diasAcumulados,
                        StockActual     = ultimo.StockAlMomento,
                        StockMinimo     = ultimo.PuntoReposicion,
                        UltimaAlerta    = ultimo.FechaUltimaAlerta,
                        EstadoActual    = ultimo.Estado == EstadoAlerta.Activa ? "Bajo stock" : "Resuelto",
                        Criticidad      = CalcularCriticidad(ultimo.StockAlMomento, ultimo.PuntoReposicion)
                    };
                })
                .OrderByDescending(r => r.CantidadAlertas)
                .ToList();

            // ── 3. Evolución semanal ───────────────────────────────────────────────
            var evolucionRaw = await _alertaRepository.GetEvolucionSemanalAsync(idSede, semanas);

            var evolucionSemanal = evolucionRaw.Select(e => new EvolucionSemanalDto
            {
                Semana  = e.InicioSemana.ToString("d MMM"),
                Alertas = e.CantidadAlertas
            }).ToList();

            return new InformeAlertasStockDto
            {
                BajoStock        = bajoStock,
                Recurrencia      = recurrencia,
                EvolucionSemanal = evolucionSemanal
            };
        }

        // ── Helpers ───────────────────────────────────────────────────────────────

        private static string CalcularCriticidad(int stockActual, int puntoReposicion)
        {
            if (stockActual == 0) return "Alta";
            if (puntoReposicion > 0 && stockActual <= puntoReposicion / 2) return "Media";
            return "Baja";
        }
    }
}
