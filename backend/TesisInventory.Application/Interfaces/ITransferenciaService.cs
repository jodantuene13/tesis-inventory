using System.Collections.Generic;
using System.Threading.Tasks;
using TesisInventory.Application.DTOs.Transferencias;

namespace TesisInventory.Application.Interfaces
{
    public interface ITransferenciaService
    {
        Task<IEnumerable<TransferenciaDto>> GetEntrantesAsync(int idSede);
        Task<IEnumerable<TransferenciaDto>> GetSalientesAsync(int idSede);
        Task<TransferenciaDto> CreateAsync(CreateTransferenciaDto createDto, int idUsuarioSolicita);
        Task AceptarTransferenciaAsync(int idTransferencia, int idUsuarioAutenticado, string? observaciones = null);
        Task RechazarTransferenciaAsync(int idTransferencia, int idUsuarioAutenticado, string? observaciones = null);
        Task ConfirmarRecepcionAsync(int idTransferencia, int idUsuarioAutenticado, string? observaciones = null);
        Task DevolverPrestamoAsync(int idTransferencia, int idUsuarioAutenticado, string? observaciones = null);
    }
}
