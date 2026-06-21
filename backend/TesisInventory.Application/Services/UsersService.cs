using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TesisInventory.Application.DTOs;
using TesisInventory.Application.Interfaces;
using TesisInventory.Domain.Entities;
using TesisInventory.Domain.Interfaces;

namespace TesisInventory.Application.Services
{
    public class UsersService : IUsersService
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IAuditService _auditService;

        public UsersService(IUserRepository userRepository, IRoleRepository roleRepository, IAuditService auditService)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _auditService = auditService;
        }

        public async Task<UserDto> CreateUserAsync(CreateUserDto dto)
        {
            if (await _userRepository.ExistsByEmailAsync(dto.Email))
                throw new Exception("El correo electrónico ya está registrado.");

            var usuario = new Usuario
            {
                NombreUsuario = dto.NombreUsuario,
                Email = dto.Email,
                Password = dto.Password,
                IdRol = dto.IdRol,
                IdSede = dto.IdSede,
                TodasLasSedes = dto.TodasLasSedes,
                LimitarOperacionSedePrimaria = dto.LimitarOperacionSedePrimaria,
                Estado = true,
                FechaRegistro = DateTime.Now,
                UsuariosSedes = dto.SedesIds
                    .Select(idSede => new UsuarioSede { IdSede = idSede })
                    .ToList()
            };

            await _userRepository.AddAsync(usuario);
            var newUserSnapshot = await GetSnapshotAsync(usuario.IdUsuario);
            await _auditService.LogActionAsync(usuario.IdUsuario, "CREATE", $"Usuario creado: {usuario.NombreUsuario}", null, newUserSnapshot);

            var created = await _userRepository.GetByIdAsync(usuario.IdUsuario);
            if (created == null) throw new Exception("Error al crear usuario.");
            return MapToDto(created);
        }

        public async Task<UserDto> UpdateUserAsync(int id, UpdateUserDto dto)
        {
            var usuario = await _userRepository.GetByIdAsync(id);
            if (usuario == null) throw new Exception("Usuario no encontrado.");

            var oldSnapshot = await GetSnapshotAsync(id);

            usuario.NombreUsuario = dto.NombreUsuario;
            usuario.IdRol = dto.IdRol;
            usuario.IdSede = dto.IdSede;
            usuario.Estado = dto.Estado;
            usuario.TodasLasSedes = dto.TodasLasSedes;
            usuario.LimitarOperacionSedePrimaria = dto.LimitarOperacionSedePrimaria;

            usuario.UsuariosSedes.Clear();
            foreach (var idSede in dto.SedesIds)
                usuario.UsuariosSedes.Add(new UsuarioSede { IdUsuario = id, IdSede = idSede });

            await _userRepository.UpdateAsync(usuario);
            var newSnapshot = await GetSnapshotAsync(id);
            await _auditService.LogActionAsync(usuario.IdUsuario, "UPDATE", $"Usuario actualizado: {usuario.NombreUsuario}", oldSnapshot, newSnapshot);
            return MapToDto(usuario);
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            var usuario = await _userRepository.GetByIdAsync(id);
            if (usuario == null) return false;

            var oldSnapshot = await GetSnapshotAsync(id);
            await _userRepository.DeleteAsync(id);
            await _auditService.LogActionAsync(id, "DELETE", $"Usuario eliminado: {usuario.NombreUsuario}", oldSnapshot, null);
            return true;
        }

        public async Task<UserDto?> GetUserByIdAsync(int id)
        {
            var usuario = await _userRepository.GetByIdAsync(id);
            return usuario == null ? null : MapToDto(usuario);
        }

        public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
        {
            var users = await _userRepository.GetAllAsync();
            return users.Select(MapToDto);
        }

        public async Task<UserDto> ChangeUserStatusAsync(int id, bool nuevoEstado)
        {
            var usuario = await _userRepository.GetByIdAsync(id);
            if (usuario == null) throw new Exception("Usuario no encontrado.");

            var oldSnapshot = await GetSnapshotAsync(id);
            usuario.Estado = nuevoEstado;
            await _userRepository.UpdateAsync(usuario);
            var newSnapshot = await GetSnapshotAsync(id);
            await _auditService.LogActionAsync(id, "CHANGE_STATUS", $"Estado cambiado a: {(nuevoEstado ? "Activo" : "Inactivo")}", oldSnapshot, newSnapshot);
            return MapToDto(usuario);
        }

        public async Task<bool> ChangePasswordAsync(int id, string newPassword)
        {
            var usuario = await _userRepository.GetByIdAsync(id);
            if (usuario == null) return false;

            usuario.Password = newPassword;
            await _userRepository.UpdateAsync(usuario);
            await _auditService.LogActionAsync(id, "CHANGE_PASSWORD", "Contraseña actualizada");
            return true;
        }

        public async Task<IEnumerable<UserDto>> GetUsersByRoleAsync(int roleId)
        {
            var users = await _userRepository.GetByRoleAsync(roleId);
            return users.Select(MapToDto);
        }

        public async Task<IEnumerable<UserDto>> GetUsersBySedeAsync(int sedeId)
        {
            var users = await _userRepository.GetBySedeAsync(sedeId);
            return users.Select(MapToDto);
        }

        public async Task<IEnumerable<UserDto>> SearchUsersAsync(string? searchTerm, int? roleId, int? sedeId, bool? status)
        {
            var users = await _userRepository.SearchUsersAsync(searchTerm, roleId, sedeId, status);
            return users.Select(MapToDto);
        }

        private static UserDto MapToDto(Usuario u) => new UserDto
        {
            IdUsuario = u.IdUsuario,
            NombreUsuario = u.NombreUsuario,
            Email = u.Email,
            Estado = u.Estado,
            FechaRegistro = u.FechaRegistro,
            IdRol = u.IdRol,
            IdSede = u.IdSede,
            NombreRol = u.Rol?.NombreRol ?? "Sin Rol",
            NombreSede = u.Sede?.NombreSede ?? "Sin Sede",
            TodasLasSedes = u.TodasLasSedes,
            LimitarOperacionSedePrimaria = u.LimitarOperacionSedePrimaria,
            SedesPermitidas = u.UsuariosSedes.Select(us => us.IdSede).ToList()
        };

        private async Task<object?> GetSnapshotAsync(int userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null) return null;

            return new
            {
                Nombre = user.NombreUsuario,
                Email = user.Email,
                Estado = user.Estado ? "Activo" : "Inactivo",
                Rol = user.Rol?.NombreRol ?? user.IdRol.ToString(),
                Sede = user.Sede?.NombreSede ?? user.IdSede.ToString()
            };
        }
    }
}
