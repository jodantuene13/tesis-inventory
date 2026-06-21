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

        public async Task<System.Collections.Generic.IEnumerable<Transferencia>> GetAllAsync()
        {
            return await _context.Transferencia
                .Include(t => t.Detalles)
                    .ThenInclude(d => d.Producto)
                .Include(t => t.SedeOrigen)
                .Include(t => t.SedeDestino)
                .Include(t => t.UsuarioSolicita)
                .OrderByDescending(t => t.FechaSolicitud)
                .ToListAsync();
        }

        public async Task<System.Collections.Generic.IEnumerable<Transferencia>> GetDatosInformeAsync(
            int? idSedeOrigen, int? idSedeDestino, int? idFamilia,
            TesisInventory.Domain.Enums.MotivoTransferencia? motivo, TesisInventory.Domain.Enums.EstadoTransferencia? estado,
            System.DateTime fechaDesde, System.DateTime fechaHasta)
        {
            var query = _context.Transferencia
                .Include(t => t.Detalles)
                    .ThenInclude(d => d.Producto)
                        .ThenInclude(p => p.Familia)
                .Include(t => t.SedeOrigen)
                .Include(t => t.SedeDestino)
                .Include(t => t.UsuarioSolicita)
                .Include(t => t.HistorialTransferencias)
                .Where(t => t.FechaSolicitud >= fechaDesde && t.FechaSolicitud <= fechaHasta.AddDays(1).AddTicks(-1))
                .AsQueryable();

            if (idSedeOrigen.HasValue)
                query = query.Where(t => t.IdSedeOrigen == idSedeOrigen.Value);

            if (idSedeDestino.HasValue)
                query = query.Where(t => t.IdSedeDestino == idSedeDestino.Value);

            if (motivo.HasValue)
                query = query.Where(t => t.Motivo == motivo.Value);

            if (estado.HasValue)
                query = query.Where(t => t.Estado == estado.Value);

            if (idFamilia.HasValue)
                query = query.Where(t => t.Detalles.Any(d => d.Producto.IdFamilia == idFamilia.Value));

            return await query.OrderByDescending(t => t.FechaSolicitud).ToListAsync();
        }

        public async Task AddHistorialTransferenciaAsync(HistorialTransferencia historial)
        {
            _context.HistorialTransferencia.Add(historial);
            await _context.SaveChangesAsync();
        }
    }
}
