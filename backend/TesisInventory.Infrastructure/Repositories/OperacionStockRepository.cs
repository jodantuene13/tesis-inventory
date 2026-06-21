using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TesisInventory.Domain.Entities;
using TesisInventory.Domain.Interfaces;
using TesisInventory.Infrastructure.Persistence;

namespace TesisInventory.Infrastructure.Repositories
{
    public class OperacionStockRepository : IOperacionStockRepository
    {
        private readonly InventoryDbContext _context;

        public OperacionStockRepository(InventoryDbContext context)
        {
            _context = context;
        }

        public async Task<OperacionStock> AddOperacionStockAsync(OperacionStock operacionStock)
        {
            _context.OperacionStock.Add(operacionStock);
            await _context.SaveChangesAsync();
            return operacionStock;
        }

        public async Task<OperacionStock?> GetByIdAsync(int idOperacion)
        {
            return await _context.OperacionStock
                .Include(o => o.Movimientos)
                    .ThenInclude(m => m.Producto)
                .Include(o => o.Usuario)
                .FirstOrDefaultAsync(o => o.IdOperacion == idOperacion);
        }

        public async System.Threading.Tasks.Task<(System.Collections.Generic.IEnumerable<OperacionStock> Items, int TotalCount)> GetOperacionesPaginadasAsync(
            int idSede, 
            string? search, 
            string? tipoOperacion, 
            int? idUsuario, 
            string? fechaDesde, 
            string? fechaHasta, 
            int skip, 
            int take)
        {
            var query = _context.OperacionStock
                .Include(o => o.Usuario)
                .Include(o => o.Movimientos)
                    .ThenInclude(m => m.Producto)
                .Where(o => o.IdSede == idSede)
                .AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(o => 
                    (o.TicketSolicitud != null && o.TicketSolicitud.Contains(search)) ||
                    (o.OrdenCompra != null && o.OrdenCompra.Contains(search)) ||
                    (o.OrdenTrabajo != null && o.OrdenTrabajo.Contains(search)) ||
                    o.IdOperacion.ToString().Contains(search)
                );
            }

            if (!string.IsNullOrEmpty(tipoOperacion))
            {
                if (System.Enum.TryParse<TesisInventory.Domain.Enums.TipoMovimiento>(tipoOperacion, true, out var tipo))
                {
                    query = query.Where(o => o.TipoOperacion == tipo);
                }
            }

            if (idUsuario.HasValue)
            {
                query = query.Where(o => o.IdUsuario == idUsuario.Value);
            }

            if (!string.IsNullOrEmpty(fechaDesde) && System.DateTime.TryParse(fechaDesde, out var dateDesde))
            {
                query = query.Where(o => o.Fecha.Date >= dateDesde.Date);
            }

            if (!string.IsNullOrEmpty(fechaHasta) && System.DateTime.TryParse(fechaHasta, out var dateHasta))
            {
                query = query.Where(o => o.Fecha.Date <= dateHasta.Date);
            }

            var totalCount = await query.CountAsync();

            var items = await query
                .OrderByDescending(o => o.Fecha)
                .Skip(skip)
                .Take(take)
                .ToListAsync();

            return (items, totalCount);
        }
    }
}
