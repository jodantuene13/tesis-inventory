using System.Collections.Generic;
using System.Threading.Tasks;
using TesisInventory.Domain.Entities;

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
    }
}
