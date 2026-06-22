using System.Collections.Generic;
using System.Threading.Tasks;
using TesisInventory.Domain.Entities;

namespace TesisInventory.Domain.Interfaces
{
    public interface IUnidadMedidaRepository
    {
        Task<IEnumerable<UnidadMedida>> GetAllAsync(bool includeInactive = false);
        Task<UnidadMedida?> GetByIdAsync(int id);
        Task<UnidadMedida?> GetBySimbolo(string simbolo);
        Task<UnidadMedida> AddAsync(UnidadMedida unidad);
        Task UpdateAsync(UnidadMedida unidad);
        Task DeleteAsync(UnidadMedida unidad);
    }
}
