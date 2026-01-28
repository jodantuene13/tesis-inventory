using System.Threading.Tasks;
using TesisInventory.Domain.Entities;

namespace TesisInventory.Application.Interfaces
{
    public interface IUserRepository
    {
        Task<Usuario> GetByEmailAsync(string email);
        Task<Usuario> GetByGoogleIdAsync(string googleId);
        Task UpdateAsync(Usuario usuario);
    }
}
