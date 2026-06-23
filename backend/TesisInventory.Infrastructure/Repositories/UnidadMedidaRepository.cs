using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TesisInventory.Domain.Entities;
using TesisInventory.Domain.Interfaces;
using TesisInventory.Infrastructure.Persistence;

namespace TesisInventory.Infrastructure.Repositories
{
    public class UnidadMedidaRepository : IUnidadMedidaRepository
    {
        private readonly InventoryDbContext _context;

        public UnidadMedidaRepository(InventoryDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<UnidadMedida>> GetAllAsync(bool includeInactive = false)
        {
            var query = _context.UnidadMedida.AsQueryable();
            if (!includeInactive)
                query = query.Where(u => u.Activo);
            return await query.OrderBy(u => u.Simbolo).ToListAsync();
        }

        public async Task<UnidadMedida?> GetByIdAsync(int id)
            => await _context.UnidadMedida.FirstOrDefaultAsync(u => u.IdUnidadMedida == id);

        public async Task<UnidadMedida?> GetBySimbolo(string simbolo)
            => await _context.UnidadMedida.FirstOrDefaultAsync(u => u.Simbolo == simbolo);

        public async Task<UnidadMedida> AddAsync(UnidadMedida unidad)
        {
            await _context.UnidadMedida.AddAsync(unidad);
            await _context.SaveChangesAsync();
            return unidad;
        }

        public async Task UpdateAsync(UnidadMedida unidad)
        {
            _context.UnidadMedida.Update(unidad);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(UnidadMedida unidad)
        {
            unidad.Activo = false;
            _context.UnidadMedida.Update(unidad);
            await _context.SaveChangesAsync();
        }
    }
}
