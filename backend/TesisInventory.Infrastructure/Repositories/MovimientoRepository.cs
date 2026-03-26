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
    public class MovimientoRepository : IMovimientoRepository
    {
        private readonly InventoryDbContext _context;

        public MovimientoRepository(InventoryDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Movimiento>> GetMovimientosAsync(int idProducto, int idSede, string? tipoMovimiento, string? fechaDesde, string? fechaHasta)
        {
            var query = _context.Movimiento
                .Include(m => m.Usuario)
                .Where(m => m.IdProducto == idProducto && m.IdSede == idSede)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(tipoMovimiento) && Enum.TryParse(typeof(TipoMovimiento), tipoMovimiento, true, out var parsedTipo))
            {
                query = query.Where(m => m.TipoMovimiento == (TipoMovimiento)parsedTipo);
            }

            if (!string.IsNullOrWhiteSpace(fechaDesde) && DateTime.TryParse(fechaDesde, out var dateDesde))
            {
                query = query.Where(m => m.Fecha >= dateDesde);
            }

            if (!string.IsNullOrWhiteSpace(fechaHasta) && DateTime.TryParse(fechaHasta, out var dateHasta))
            {
                query = query.Where(m => m.Fecha <= dateHasta);
            }

            return await query
                .OrderByDescending(m => m.Fecha)
                .ToListAsync();
        }

        public async Task<Movimiento> AddMovimientoAsync(Movimiento movimiento)
        {
            _context.Movimiento.Add(movimiento);
            await _context.SaveChangesAsync();
            return movimiento;
        }
    }
}
