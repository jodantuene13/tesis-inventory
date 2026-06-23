using System.Collections.Generic;
using System.Threading.Tasks;
using TesisInventory.Application.DTOs;

namespace TesisInventory.Application.Interfaces
{
    public interface IUnidadMedidaService
    {
        Task<IEnumerable<UnidadMedidaDto>> GetAllAsync(bool includeInactive = false);
        Task<UnidadMedidaDto?> GetByIdAsync(int id);
        Task<UnidadMedidaDto> CreateAsync(CreateUnidadMedidaDto dto);
        Task<UnidadMedidaDto> UpdateAsync(int id, UpdateUnidadMedidaDto dto);
        Task DeleteAsync(int id);
    }
}
