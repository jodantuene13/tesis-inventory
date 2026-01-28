using System.Collections.Generic;
using System.Threading.Tasks;
using TesisInventory.Domain.Entities;

namespace TesisInventory.Application.Interfaces
{
    public interface IRolesService
    {
        Task<IEnumerable<Rol>> GetAllRolesAsync();
        Task<Rol?> GetRoleByIdAsync(int id);
    }
}
