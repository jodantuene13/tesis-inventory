using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using TesisInventory.Domain.Entities;
using TesisInventory.Domain.Interfaces;
using TesisInventory.Infrastructure.Persistence;

namespace TesisInventory.Infrastructure.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private readonly InventoryDbContext _context;

        public RoleRepository(InventoryDbContext context)
        {
            _context = context;
        }

        public async Task<Rol?> GetByIdAsync(int id)
        {
            return await _context.Rol.FindAsync(id);
        }

        public async Task<Rol?> GetByIdWithUsersAsync(int id)
        {
            return await _context.Rol
                .Include(r => r.Usuarios)
                .FirstOrDefaultAsync(r => r.IdRol == id);
        }

        public async Task<IEnumerable<Rol>> GetAllAsync()
        {
            return await _context.Rol.ToListAsync();
        }

        public async Task<Rol> AddAsync(Rol rol)
        {
            _context.Rol.Add(rol);
            await _context.SaveChangesAsync();
            return rol;
        }

        public async Task UpdateAsync(Rol rol)
        {
            _context.Entry(rol).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var rol = await GetByIdAsync(id);
            if (rol != null)
            {
                _context.Rol.Remove(rol);
                await _context.SaveChangesAsync();
            }
        }
    }
}
