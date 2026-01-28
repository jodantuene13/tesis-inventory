using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
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
    }
}
