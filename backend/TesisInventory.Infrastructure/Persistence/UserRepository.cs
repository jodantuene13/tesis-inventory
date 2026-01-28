using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TesisInventory.Application.Interfaces;
using TesisInventory.Domain.Entities;

namespace TesisInventory.Infrastructure.Persistence
{
    public class UserRepository : IUserRepository
    {
        private readonly InventoryDbContext _context;

        public UserRepository(InventoryDbContext context)
        {
            _context = context;
        }

        public async Task<Usuario> GetByEmailAsync(string email)
        {
            return await _context.Usuario.Include(u => u.Rol).FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<Usuario> GetByGoogleIdAsync(string googleId)
        {
            return await _context.Usuario.Include(u => u.Rol).FirstOrDefaultAsync(u => u.GoogleId == googleId);
        }

        public async Task UpdateAsync(Usuario usuario)
        {
            _context.Usuario.Update(usuario);
            await _context.SaveChangesAsync();
        }
        
        // Helper to match interface if SaveChanges is not part of UpdateAsync contract usually 
        // but for simplicity we save here or use UnitOfWork. 
        // Re-implementing correctly:
        // The interface says UpdateAsync.
    }
}
