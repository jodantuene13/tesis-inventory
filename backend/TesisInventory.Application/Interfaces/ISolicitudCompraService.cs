using System.Collections.Generic;
using System.Threading.Tasks;
using TesisInventory.Application.DTOs.SolicitudesCompra;

namespace TesisInventory.Application.Interfaces
{
    public interface ISolicitudCompraService
    {
        Task<SolicitudCompraDto> CreateSolicitudAsync(int idSede, int idUsuario, CreateSolicitudCompraDto dto);
        Task<SolicitudCompraDto> UpdateEstadoAsync(int idSolicitud, int idUsuarioAprobador, UpdateSolicitudCompraEstadoDto dto);
        Task<(IEnumerable<SolicitudCompraDto> Items, int TotalCount)> GetPagedSolicitudesAsync(
            int? idSede, string? search, int? estado, int page, int pageSize);
        Task<SolicitudCompraDto?> GetByIdAsync(int id);
    }
}
