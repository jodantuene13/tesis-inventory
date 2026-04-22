using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TesisInventory.Domain.Entities;
using TesisInventory.Domain.Interfaces;
using TesisInventory.Infrastructure.Persistence;

namespace TesisInventory.Infrastructure.Repositories
{
    public class FamiliaRepository : IFamiliaRepository
    {
        private readonly InventoryDbContext _context;

        public FamiliaRepository(InventoryDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Familia>> GetAllAsync(bool includeInactive = false)
        {
            var query = _context.Familia.Include(f => f.Rubro).AsQueryable();
            if (!includeInactive)
            {
                query = query.Where(f => f.Activo);
            }
            return await query.ToListAsync();
        }

        public async Task<IEnumerable<Familia>> GetByRubroIdAsync(int idRubro, bool includeInactive = false)
        {
            var query = _context.Familia.Where(f => f.IdRubro == idRubro);
            if (!includeInactive)
            {
                query = query.Where(f => f.Activo);
            }
            return await query.ToListAsync();
        }

        public async Task<Familia?> GetByIdAsync(int id)
        {
            return await _context.Familia.Include(f => f.Rubro).FirstOrDefaultAsync(f => f.IdFamilia == id);
        }

        public async Task<Familia?> GetByCodigoLocalAsync(int idRubro, string codigoFamilia)
        {
            return await _context.Familia.FirstOrDefaultAsync(f => f.IdRubro == idRubro && f.CodigoFamilia == codigoFamilia);
        }

        public async Task<Familia> AddAsync(Familia familia)
        {
            await _context.Familia.AddAsync(familia);
            await _context.SaveChangesAsync();
            return familia;
        }

        public async Task UpdateAsync(Familia familia)
        {
            _context.Familia.Update(familia);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Familia familia)
        {
            familia.Activo = false;
            _context.Familia.Update(familia);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> HasAsociacionesAsync(int idFamilia)
        {
            bool hasProductos = await _context.Producto.AnyAsync(p => p.IdFamilia == idFamilia && p.Activo);
            bool hasAtributos = await _context.FamiliaAtributo.AnyAsync(fa => fa.IdFamilia == idFamilia && fa.Atributo!.Activo);
            return hasProductos || hasAtributos;
        }

        public async Task<(IEnumerable<string> Productos, IEnumerable<string> Atributos)> GetAsociacionesAsync(int idFamilia)
        {
            var productos = await _context.Producto
                .Where(p => p.IdFamilia == idFamilia && p.Activo)
                .Select(p => p.Nombre)
                .ToListAsync();

            var atributos = await _context.FamiliaAtributo
                .Where(fa => fa.IdFamilia == idFamilia && fa.Atributo!.Activo)
                .Select(fa => fa.Atributo!.Nombre)
                .ToListAsync();

            return (productos, atributos);
        }
    }
}
