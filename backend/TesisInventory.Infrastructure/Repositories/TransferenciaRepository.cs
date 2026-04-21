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
                .Include(t => t.Detalles)
                    .ThenInclude(d => d.Producto)
                .Include(t => t.SedeOrigen)
                .Include(t => t.SedeDestino)
                .Include(t => t.UsuarioSolicita)
                .Include(t => t.HistorialTransferencias)
                .ThenInclude(h => h.Usuario)
                .FirstOrDefaultAsync(t => t.IdTransferencia == id);
        }

        public async Task<System.Collections.Generic.IEnumerable<Transferencia>> GetEntrantesAsync(int idSede)
        {
            return await _context.Transferencia
                .Include(t => t.Detalles)
                    .ThenInclude(d => d.Producto)
                .Include(t => t.SedeOrigen)
                .Include(t => t.SedeDestino)
                .Include(t => t.UsuarioSolicita)
                .Where(t => t.IdSedeOrigen == idSede)
                .OrderByDescending(t => t.FechaSolicitud)
                .ToListAsync();
        }

        public async Task<System.Collections.Generic.IEnumerable<Transferencia>> GetSalientesAsync(int idSede)
        {
            return await _context.Transferencia
                .Include(t => t.Detalles)
                    .ThenInclude(d => d.Producto)
                .Include(t => t.SedeOrigen)
                .Include(t => t.SedeDestino)
                .Include(t => t.UsuarioSolicita)
                .Where(t => t.IdSedeDestino == idSede)
                .OrderByDescending(t => t.FechaSolicitud)
                .ToListAsync();
        }

        public async Task AddHistorialTransferenciaAsync(HistorialTransferencia historial)
        {
            _context.HistorialTransferencia.Add(historial);
            await _context.SaveChangesAsync();
        }
    }
}
