using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using TesisInventory.Domain.Entities;
using TesisInventory.Domain.Interfaces;
using TesisInventory.Infrastructure.Persistence;

namespace TesisInventory.Infrastructure.Repositories
{
    public class TransferenciaRepository : ITransferenciaRepository
    {
        private readonly InventoryDbContext _context;

        public TransferenciaRepository(InventoryDbContext context)
        {
            _context = context;
        }

        public async Task<Transferencia> AddTransferenciaAsync(Transferencia transferencia)
        {
            _context.Transferencia.Add(transferencia);
            await _context.SaveChangesAsync();
            return transferencia;
        }

        public async Task UpdateTransferenciaAsync(Transferencia transferencia)
        {
            _context.Transferencia.Update(transferencia);
            await _context.SaveChangesAsync();
        }

        public async Task<Transferencia?> GetTransferenciaByIdAsync(int id)
        {
            return await _context.Transferencia
                .Include(t => t.Producto)
                .Include(t => t.SedeOrigen)
                .Include(t => t.SedeDestino)
                .Include(t => t.UsuarioSolicita)
                .Include(t => t.HistorialTransferencias)
                .ThenInclude(h => h.Usuario)
                .FirstOrDefaultAsync(t => t.IdTransferencia == id);
        }

        public async Task AddHistorialTransferenciaAsync(HistorialTransferencia historial)
        {
            _context.HistorialTransferencia.Add(historial);
            await _context.SaveChangesAsync();
        }
    }
}
