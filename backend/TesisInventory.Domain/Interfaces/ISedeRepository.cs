using System.Collections.Generic;
using System.Threading.Tasks;
using TesisInventory.Domain.Entities;

namespace TesisInventory.Domain.Interfaces
{
    public interface ISedeRepository
    {
        Task<IEnumerable<Sede>> GetAllAsync();
        Task<Sede?> GetByIdAsync(int id);
    }
}
