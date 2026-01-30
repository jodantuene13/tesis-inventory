using System.Collections.Generic;
using System.Threading.Tasks;
using TesisInventory.Domain.Entities;

namespace TesisInventory.Application.Interfaces
{
    public interface IRolesService
    {
        Task<IEnumerable<Rol>> GetAllRolesAsync();
        Task<Rol?> GetRoleByIdAsync(int id);
        Task<Rol> CreateRoleAsync(Rol rol);
        Task UpdateRoleAsync(Rol rol);
        Task<bool> DeleteRoleAsync(int id);
    }
}
