using System.Collections.Generic;
using System.Threading.Tasks;
using TesisInventory.Domain.Entities;

namespace TesisInventory.Application.Interfaces
{
    public interface IAuthService
    {
        Task<Usuario> ValidateGoogleTokenAsync(string googleToken);
        Task<string> GenerateJwtTokenAsync(Usuario usuario, IEnumerable<string> permisos);
        bool ValidateDomain(string email);
    }
}
