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

        public async Task<IEnumerable<Rol>> GetAllAsync()
        {
            return await _context.Rol.ToListAsync();
        }
    }
}
