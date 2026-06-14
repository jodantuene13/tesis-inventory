using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TesisInventory.Application.DTOs.Roles;
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

        public async Task<IEnumerable<RolDto>> GetAllRolesAsync()
        {
            var roles = await _roleRepository.GetAllWithPermissionsAndSedesAsync();
            return roles.Select(MapToDto);
        }

        public async Task<RolDto?> GetRoleByIdAsync(int id)
        {
            var role = await _roleRepository.GetByIdWithPermissionsAndSedesAsync(id);
            return role == null ? null : MapToDto(role);
        }

        public async Task<RolDto> CreateRoleAsync(RolCreateDto rolDto)
        {
            var newRole = new Rol
            {
                NombreRol = rolDto.NombreRol,
                Descripcion = rolDto.Descripcion,
                RolesPermisos = rolDto.PermisosIds.Select(id => new RolPermiso { IdPermiso = id }).ToList()
            };

            var createdRole = await _roleRepository.AddAsync(newRole);
            return new RolDto
            {
                IdRol = createdRole.IdRol,
                NombreRol = createdRole.NombreRol,
                Descripcion = createdRole.Descripcion,
                PermisosIds = rolDto.PermisosIds
            };
        }

        public async Task UpdateRoleAsync(RolUpdateDto rolDto)
        {
            var role = await _roleRepository.GetByIdWithPermissionsAndSedesAsync(rolDto.IdRol);
            if (role == null) throw new System.ArgumentException("Rol no encontrado");

            role.NombreRol = rolDto.NombreRol;
            role.Descripcion = rolDto.Descripcion;

            role.RolesPermisos.Clear();
            foreach (var id in rolDto.PermisosIds)
            {
                role.RolesPermisos.Add(new RolPermiso { IdRol = role.IdRol, IdPermiso = id });
            }

            await _roleRepository.UpdateAsync(role);
        }

        public async Task<bool> DeleteRoleAsync(int id)
        {
            var role = await _roleRepository.GetByIdWithUsersAsync(id);
            if (role == null) return false;

            if (role.Usuarios != null && role.Usuarios.Any())
                throw new System.InvalidOperationException("No se puede eliminar un rol asignado a un usuario.");

            await _roleRepository.DeleteAsync(id);
            return true;
        }

        public async Task<IEnumerable<PermisoDto>> GetAllPermisosAsync()
        {
            var permisos = await _roleRepository.GetAllPermisosAsync();
            return permisos.Select(p => new PermisoDto
            {
                IdPermiso = p.IdPermiso,
                Nombre = p.Nombre,
                Modulo = p.Modulo,
                Descripcion = p.Descripcion
            });
        }

        private static RolDto MapToDto(Rol r) => new RolDto
        {
            IdRol = r.IdRol,
            NombreRol = r.NombreRol,
            Descripcion = r.Descripcion,
            PermisosIds = r.RolesPermisos.Select(rp => rp.IdPermiso).ToList()
        };
    }
}
