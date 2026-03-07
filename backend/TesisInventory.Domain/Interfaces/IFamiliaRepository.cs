using System.Collections.Generic;
using System.Threading.Tasks;
using TesisInventory.Domain.Entities;

namespace TesisInventory.Domain.Interfaces
{
    public interface IFamiliaRepository
    {
        Task<IEnumerable<Familia>> GetAllAsync(bool includeInactive = false);
        Task<IEnumerable<Familia>> GetByRubroIdAsync(int idRubro, bool includeInactive = false);
        Task<Familia?> GetByIdAsync(int id);
        Task<Familia?> GetByCodigoLocalAsync(int idRubro, string codigoFamilia);
        Task<Familia> AddAsync(Familia familia);
        Task UpdateAsync(Familia familia);
        Task DeleteAsync(Familia familia); // Baja Lógica
    }
}
