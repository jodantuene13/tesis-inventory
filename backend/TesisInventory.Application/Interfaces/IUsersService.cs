using System.Collections.Generic;
using System.Threading.Tasks;
using TesisInventory.Application.DTOs;

namespace TesisInventory.Application.Interfaces
{
    public interface IUsersService
    {
        Task<UserDto> CreateUserAsync(CreateUserDto createUserDto);
        Task<UserDto> UpdateUserAsync(int id, UpdateUserDto updateUserDto);
        Task<bool> DeleteUserAsync(int id);
        Task<UserDto?> GetUserByIdAsync(int id);
        Task<IEnumerable<UserDto>> GetAllUsersAsync();
        Task<UserDto> ChangeUserStatusAsync(int id, bool nuevoEstado);
        Task<bool> ChangePasswordAsync(int id, string newPassword);
        Task<IEnumerable<UserDto>> GetUsersByRoleAsync(int roleId);
        Task<IEnumerable<UserDto>> GetUsersBySedeAsync(int sedeId);
    }
}
