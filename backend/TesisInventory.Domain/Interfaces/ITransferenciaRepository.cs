using System.Threading.Tasks;
using TesisInventory.Domain.Entities;

namespace TesisInventory.Domain.Interfaces
{
    public interface ITransferenciaRepository
    {
        Task<Transferencia> AddTransferenciaAsync(Transferencia transferencia);
        Task UpdateTransferenciaAsync(Transferencia transferencia);
        Task<Transferencia?> GetTransferenciaByIdAsync(int id);
        
        Task AddHistorialTransferenciaAsync(HistorialTransferencia historial);
    }
}
