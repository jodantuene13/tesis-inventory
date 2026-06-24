using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TesisInventory.Domain.Entities;
using TesisInventory.Domain.Interfaces;
using TesisInventory.Infrastructure.Persistence;

namespace TesisInventory.Infrastructure.Repositories
{
    public class SedeRepository : ISedeRepository
    {
        private readonly InventoryDbContext _context;

        public SedeRepository(InventoryDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Sede>> GetAllAsync()
        {
            return await _context.Sede.ToListAsync();
        }

        public async Task<Sede?> GetByIdAsync(int id)
        {
            return await _context.Sede.FindAsync(id);
        }

        public async Task<Sede> AddAsync(Sede sede)
        {
            await _context.Sede.AddAsync(sede);
            await _context.SaveChangesAsync();
            return sede;
        }

        public async Task UpdateAsync(Sede sede)
        {
            _context.Sede.Update(sede);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Sede sede)
        {
            _context.Sede.Remove(sede);
            await _context.SaveChangesAsync();
        }

        public async Task SoftDeleteAsync(int id)
        {
            var sede = await _context.Sede.FindAsync(id);
            if (sede == null) return;

            sede.Activo = false;

            // Limpiar registros operativos que no tienen sentido en una sede inactiva
            var alertas = await _context.AlertaStock.Where(a => a.IdSede == id).ToListAsync();
            _context.AlertaStock.RemoveRange(alertas);

            var stockVacio = await _context.Stock.Where(s => s.IdSede == id && s.CantidadActual == 0).ToListAsync();
            _context.Stock.RemoveRange(stockVacio);

            var usuariosSedes = await _context.UsuarioSede.Where(us => us.IdSede == id).ToListAsync();
            _context.UsuarioSede.RemoveRange(usuariosSedes);

            var rolesSedes = await _context.RolSede.Where(rs => rs.IdSede == id).ToListAsync();
            _context.RolSede.RemoveRange(rolesSedes);

            await _context.SaveChangesAsync();
        }
    }
}
