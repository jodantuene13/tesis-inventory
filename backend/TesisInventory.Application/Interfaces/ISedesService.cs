using System.Collections.Generic;
using System.Threading.Tasks;
using TesisInventory.Domain.Entities;

namespace TesisInventory.Application.Interfaces
{
    public interface ISedesService
    {
        Task<IEnumerable<Sede>> GetAllSedesAsync();
        Task<Sede?> GetSedeByIdAsync(int id);
    }
}
