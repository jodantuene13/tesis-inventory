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
    }
}
