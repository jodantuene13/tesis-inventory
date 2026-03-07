using System.Collections.Generic;
using System.Threading.Tasks;
using TesisInventory.Application.DTOs.Sedes;

namespace TesisInventory.Application.Interfaces
{
    public interface ISedesService
    {
        Task<IEnumerable<SedeDto>> GetAllSedesAsync();
        Task<SedeDto?> GetSedeByIdAsync(int id);
        Task<SedeDto> CreateSedeAsync(CreateSedeDto createDto);
        Task<SedeDto?> UpdateSedeAsync(int id, UpdateSedeDto updateDto);
        Task<bool> DeleteSedeAsync(int id);
    }
}
