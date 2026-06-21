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
        Task<System.Collections.Generic.IEnumerable<Transferencia>> GetAllAsync();
        Task<System.Collections.Generic.IEnumerable<Transferencia>> GetDatosInformeAsync(
            int? idSedeOrigen, int? idSedeDestino, int? idFamilia,
            TesisInventory.Domain.Enums.MotivoTransferencia? motivo, TesisInventory.Domain.Enums.EstadoTransferencia? estado,
            System.DateTime fechaDesde, System.DateTime fechaHasta);
        
        Task AddHistorialTransferenciaAsync(HistorialTransferencia historial);
    }
}
