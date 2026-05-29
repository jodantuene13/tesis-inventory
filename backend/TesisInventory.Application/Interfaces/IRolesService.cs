using System.Collections.Generic;
using System.Threading.Tasks;
using TesisInventory.Application.DTOs.Roles;

namespace TesisInventory.Application.Interfaces
{
    public interface IRolesService
    {
        Task<IEnumerable<RolDto>> GetAllRolesAsync();
        Task<RolDto?> GetRoleByIdAsync(int id);
        Task<RolDto> CreateRoleAsync(RolCreateDto rolDto);
        Task UpdateRoleAsync(RolUpdateDto rolDto);
        Task<bool> DeleteRoleAsync(int id);
        
        Task<IEnumerable<PermisoDto>> GetAllPermisosAsync();
    }
}
