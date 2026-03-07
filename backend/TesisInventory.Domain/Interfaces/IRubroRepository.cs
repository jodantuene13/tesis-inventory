using System.Collections.Generic;
using System.Threading.Tasks;
using TesisInventory.Domain.Entities;

namespace TesisInventory.Domain.Interfaces
{
    public interface IRubroRepository
    {
        Task<IEnumerable<Rubro>> GetAllAsync(bool includeInactive = false);
        Task<Rubro?> GetByIdAsync(int id);
        Task<Rubro?> GetByCodigoAsync(string codigo);
        Task<Rubro> AddAsync(Rubro rubro);
        Task UpdateAsync(Rubro rubro);
        Task DeleteAsync(Rubro rubro); // Baja Lógica
    }
}
