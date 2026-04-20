using System.Threading.Tasks;
using TesisInventory.Domain.Entities;

namespace TesisInventory.Domain.Interfaces
{
    public interface ITransferenciaRepository
    {
        Task<Transferencia> AddTransferenciaAsync(Transferencia transferencia);
        Task UpdateTransferenciaAsync(Transferencia transferencia);
        Task<Transferencia?> GetTransferenciaByIdAsync(int id);
        Task<System.Collections.Generic.IEnumerable<Transferencia>> GetEntrantesAsync(int idSede);
        Task<System.Collections.Generic.IEnumerable<Transferencia>> GetSalientesAsync(int idSede);
        
        Task AddHistorialTransferenciaAsync(HistorialTransferencia historial);
    }
}
