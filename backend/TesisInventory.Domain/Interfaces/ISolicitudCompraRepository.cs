using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TesisInventory.Domain.Entities;
using TesisInventory.Domain.Enums;

namespace TesisInventory.Domain.Interfaces
{
    public interface ISolicitudCompraRepository
    {
        Task<IEnumerable<SolicitudCompra>> GetAllAsync(int? idSede = null);
        Task<SolicitudCompra?> GetByIdAsync(int id);
        Task AddAsync(SolicitudCompra solicitud);
        Task UpdateAsync(SolicitudCompra solicitud);
        Task<(IEnumerable<SolicitudCompra> Items, int TotalCount)> GetPagedAsync(
            int? idSede, string? search, int? estado, int skip, int take);

        Task<IEnumerable<SolicitudCompra>> GetDatosInformeAsync(
            int? idSede,
            EstadoSolicitudCompra? estado,
            DateTime? fechaDesde,
            DateTime? fechaHasta);
    }
}
