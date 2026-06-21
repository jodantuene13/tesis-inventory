using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TesisInventory.Domain.Entities;
using TesisInventory.Domain.Enums;
using TesisInventory.Domain.Interfaces;
using TesisInventory.Infrastructure.Persistence;

namespace TesisInventory.Infrastructure.Repositories
{
    public class AlertaStockRepository : IAlertaStockRepository
    {
        private readonly InventoryDbContext _context;

        public AlertaStockRepository(InventoryDbContext context)
        {
            _context = context;
        }

        public async Task<AlertaStock?> GetAlertaActivaAsync(int idProducto, int idSede)
        {
            return await _context.AlertaStock
                .FirstOrDefaultAsync(a =>
                    a.IdProducto == idProducto &&
                    a.IdSede == idSede &&
                    a.Estado == EstadoAlerta.Activa);
        }

        public async Task<AlertaStock> AddAsync(AlertaStock alerta)
        {
            _context.AlertaStock.Add(alerta);
            await _context.SaveChangesAsync();
            return alerta;
        }

        public async Task UpdateAsync(AlertaStock alerta)
        {
            _context.AlertaStock.Update(alerta);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<AlertaStock>> GetAlertasActivasAsync(
            int? idSede = null,
            int? idFamilia = null)
        {
            var query = _context.AlertaStock
                .Include(a => a.Producto)
                    .ThenInclude(p => p!.Familia)
                .Include(a => a.Sede)
                .Where(a => a.Estado == EstadoAlerta.Activa)
                .AsQueryable();

            if (idSede.HasValue)
                query = query.Where(a => a.IdSede == idSede.Value);

            if (idFamilia.HasValue)
                query = query.Where(a => a.Producto!.IdFamilia == idFamilia.Value);

            return await query
                .OrderBy(a => a.StockAlMomento)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<AlertaStock>> GetHistorialAlertasAsync(
            int? idSede = null,
            int? idFamilia = null,
            DateTime? fechaDesde = null,
            DateTime? fechaHasta = null)
        {
            var query = _context.AlertaStock
                .Include(a => a.Producto)
                    .ThenInclude(p => p!.Familia)
                .Include(a => a.Sede)
                .AsQueryable();

            if (idSede.HasValue)
                query = query.Where(a => a.IdSede == idSede.Value);

            if (idFamilia.HasValue)
                query = query.Where(a => a.Producto!.IdFamilia == idFamilia.Value);

            if (fechaDesde.HasValue)
                query = query.Where(a => a.FechaCreacion >= fechaDesde.Value);

            if (fechaHasta.HasValue)
                query = query.Where(a => a.FechaCreacion <= fechaHasta.Value);

            return await query
                .OrderByDescending(a => a.FechaUltimaAlerta)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<(DateTime InicioSemana, int CantidadAlertas)>> GetEvolucionSemanalAsync(
            int? idSede = null,
            int semanas = 5)
        {
            var fechaLimite = DateTime.UtcNow.AddDays(-(semanas * 7));

            var query = _context.AlertaStock
                .Where(a => a.FechaUltimaAlerta >= fechaLimite)
                .AsQueryable();

            if (idSede.HasValue)
                query = query.Where(a => a.IdSede == idSede.Value);

            var alertas = await query.Select(a => a.FechaUltimaAlerta).ToListAsync();

            // Agrupar en memoria por semana
            var resultado = new List<(DateTime InicioSemana, int CantidadAlertas)>();

            for (int i = semanas - 1; i >= 0; i--)
            {
                var inicioSemana = DateTime.UtcNow.Date.AddDays(-(i * 7 + (int)DateTime.UtcNow.DayOfWeek));
                var finSemana    = inicioSemana.AddDays(7);
                var count        = alertas.Count(f => f >= inicioSemana && f < finSemana);
                resultado.Add((inicioSemana, count));
            }

            return resultado;
        }
    }
}
