using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TesisInventory.Domain.Entities;
using TesisInventory.Domain.Interfaces;
using TesisInventory.Infrastructure.Persistence;

namespace TesisInventory.Infrastructure.Repositories
{
    public class SolicitudCompraRepository : ISolicitudCompraRepository
    {
        private readonly InventoryDbContext _context;

        public SolicitudCompraRepository(InventoryDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<SolicitudCompra>> GetAllAsync(int? idSede = null)
        {
            var query = _context.SolicitudCompra
                .Include(s => s.Producto)
                .Include(s => s.Sede)
                .Include(s => s.UsuarioSolicitante)
                .Include(s => s.UsuarioAprobador)
                .AsQueryable();

            if (idSede.HasValue)
                query = query.Where(s => s.IdSede == idSede.Value);

            return await query.OrderByDescending(s => s.FechaSolicitud).ToListAsync();
        }

        public async Task<SolicitudCompra?> GetByIdAsync(int id)
        {
            return await _context.SolicitudCompra
                .Include(s => s.Producto)
                .Include(s => s.Sede)
                .Include(s => s.UsuarioSolicitante)
                .Include(s => s.UsuarioAprobador)
                .FirstOrDefaultAsync(s => s.IdSolicitudCompra == id);
        }

        public async Task AddAsync(SolicitudCompra solicitud)
        {
            await _context.SolicitudCompra.AddAsync(solicitud);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(SolicitudCompra solicitud)
        {
            _context.SolicitudCompra.Update(solicitud);
            await _context.SaveChangesAsync();
        }

        public async Task<(IEnumerable<SolicitudCompra> Items, int TotalCount)> GetPagedAsync(
            int? idSede, string? search, int? estado, int skip, int take)
        {
            var query = _context.SolicitudCompra
                .Include(s => s.Producto)
                .Include(s => s.Sede)
                .Include(s => s.UsuarioSolicitante)
                .Include(s => s.UsuarioAprobador)
                .AsQueryable();

            if (idSede.HasValue)
                query = query.Where(s => s.IdSede == idSede.Value);

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(s => s.Producto!.Nombre.Contains(search) || s.Producto!.Sku.Contains(search));
            }

            if (estado.HasValue)
                query = query.Where(s => (int)s.Estado == estado.Value);

            int totalCount = await query.CountAsync();
            var items = await query.OrderByDescending(s => s.FechaSolicitud)
                                   .Skip(skip)
                                   .Take(take)
                                   .ToListAsync();

            return (items, totalCount);
        }
    }
}
