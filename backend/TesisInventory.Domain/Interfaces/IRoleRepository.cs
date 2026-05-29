using System.Collections.Generic;
using System.Threading.Tasks;
using TesisInventory.Domain.Entities;

namespace TesisInventory.Domain.Interfaces
{
    public interface IRoleRepository
    {
        Task<Rol?> GetByIdAsync(int id);
        Task<Rol?> GetByIdWithUsersAsync(int id);
        Task<Rol?> GetByIdWithPermissionsAndSedesAsync(int id);
        Task<IEnumerable<Rol>> GetAllWithPermissionsAndSedesAsync();
        Task<IEnumerable<Permiso>> GetAllPermisosAsync();
        Task<Rol> AddAsync(Rol rol);
        Task UpdateAsync(Rol rol);
        Task DeleteAsync(int id);
    }
}
