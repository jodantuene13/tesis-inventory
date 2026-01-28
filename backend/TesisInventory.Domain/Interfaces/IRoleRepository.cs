using System.Collections.Generic;
using System.Threading.Tasks;
using TesisInventory.Domain.Entities;

namespace TesisInventory.Domain.Interfaces
{
    public interface IRoleRepository
    {
        Task<Rol?> GetByIdAsync(int id);
        Task<IEnumerable<Rol>> GetAllAsync();
    }
}
