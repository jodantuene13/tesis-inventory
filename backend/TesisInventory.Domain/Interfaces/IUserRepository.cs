using System.Collections.Generic;
using System.Threading.Tasks;
using TesisInventory.Domain.Entities;

namespace TesisInventory.Domain.Interfaces
{
    public interface IUserRepository
    {
        Task<Usuario?> GetByIdAsync(int id);
        Task<Usuario?> GetByEmailAsync(string email);
        Task<IEnumerable<Usuario>> GetAllAsync();
        Task AddAsync(Usuario usuario);
        Task UpdateAsync(Usuario usuario);
        Task DeleteAsync(int id);
        Task<bool> ExistsByEmailAsync(string email);
        Task<IEnumerable<Usuario>> GetByRoleAsync(int roleId);
        Task<IEnumerable<Usuario>> GetBySedeAsync(int sedeId);
    }
}
