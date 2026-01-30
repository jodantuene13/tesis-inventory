using System.Collections.Generic;
using System.Threading.Tasks;
using TesisInventory.Application.Interfaces;
using TesisInventory.Domain.Entities;
using TesisInventory.Domain.Interfaces;

namespace TesisInventory.Application.Services
{
    public class RolesService : IRolesService
    {
        private readonly IRoleRepository _roleRepository;

        public RolesService(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }

        public async Task<IEnumerable<Rol>> GetAllRolesAsync()
        {
            return await _roleRepository.GetAllAsync();
        }

        public async Task<Rol?> GetRoleByIdAsync(int id)
        {
            return await _roleRepository.GetByIdAsync(id);
        }

        public async Task<Rol> CreateRoleAsync(Rol rol)
        {
            return await _roleRepository.AddAsync(rol);
        }

        public async Task UpdateRoleAsync(Rol rol)
        {
            await _roleRepository.UpdateAsync(rol);
        }

        public async Task<bool> DeleteRoleAsync(int id)
        {
            var role = await _roleRepository.GetByIdWithUsersAsync(id);
            if (role == null) return false;

            // Validation: Cannot delete if assigned to users
            if (role.Usuarios != null && role.Usuarios.Any())
            {
                throw new System.InvalidOperationException("No se puede eliminar un rol asignado a un usuario.");
            }

            await _roleRepository.DeleteAsync(id);
            return true;
        }
    }
}
