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

        public UsersService(IUserRepository userRepository, IRoleRepository roleRepository)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
        }

        public async Task<UserDto> CreateUserAsync(CreateUserDto createUserDto)
        {
            if (await _userRepository.ExistsByEmailAsync(createUserDto.Email))
            {
                throw new Exception("El correo electrónico ya está registrado.");
            }

            // Validar correo institucional (si aplica lógica de dominio, p.ej. @ucc.edu.ar)
            if (!createUserDto.Email.EndsWith("@ucc.edu.ar"))
            {
                 // Nota: Esto podría ser configurable o parte de una regla de negocio más compleja.
                 // throw new Exception("El correo debe ser institucional (@ucc.edu.ar).");
            }

            var usuario = new Usuario
            {
                NombreUsuario = createUserDto.NombreUsuario,
                Email = createUserDto.Email,
                Password = createUserDto.Password, // TODO: Hash password here or in repository? Usually Service ensures hashing.
                IdRol = createUserDto.IdRol,
                IdSede = createUserDto.IdSede,
                Estado = true,
                FechaRegistro = DateTime.Now
            };

            await _userRepository.AddAsync(usuario);
            
            // Fetch complete entity with relations if needed, or just map back
            // For now mapping manually to return DTO
            var createdUser = await _userRepository.GetByIdAsync(usuario.IdUsuario);
            if (createdUser == null) throw new Exception("Error al crear usuario.");
            return MapToDto(createdUser);
        }

        public async Task<UserDto> UpdateUserAsync(int id, UpdateUserDto updateUserDto)
        {
            var usuario = await _userRepository.GetByIdAsync(id);
            if (usuario == null) throw new Exception("Usuario no encontrado.");

            usuario.NombreUsuario = updateUserDto.NombreUsuario;
            usuario.IdRol = updateUserDto.IdRol;
            usuario.IdSede = updateUserDto.IdSede;

            await _userRepository.UpdateAsync(usuario);
            return MapToDto(usuario);
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            var usuario = await _userRepository.GetByIdAsync(id);
            if (usuario == null) return false;

            await _userRepository.DeleteAsync(id);
            return true;
        }

        public async Task<UserDto?> GetUserByIdAsync(int id)
        {
            var usuario = await _userRepository.GetByIdAsync(id);
            if (usuario == null) return null;
            return MapToDto(usuario);
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

            usuario.Estado = nuevoEstado;
            await _userRepository.UpdateAsync(usuario);
            return MapToDto(usuario);
        }

        public async Task<bool> ChangePasswordAsync(int id, string newPassword)
        {
            var usuario = await _userRepository.GetByIdAsync(id);
            if (usuario == null) return false;

            usuario.Password = newPassword; // TODO: Remember to Hash
            await _userRepository.UpdateAsync(usuario);
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

        private UserDto MapToDto(Usuario u)
        {
            return new UserDto
            {
                IdUsuario = u.IdUsuario,
                NombreUsuario = u.NombreUsuario,
                Email = u.Email,
                Estado = u.Estado,
                FechaRegistro = u.FechaRegistro,
                IdRol = u.IdRol,
                IdSede = u.IdSede,
                NombreRol = u.Rol?.NombreRol ?? "Sin Rol",
                NombreSede = u.Sede?.NombreSede ?? "Sin Sede"
            };
        }
    }
}
