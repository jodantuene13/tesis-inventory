using System.Collections.Generic;
using System.Threading.Tasks;
using TesisInventory.Application.DTOs.Rubros;

namespace TesisInventory.Application.Interfaces
{
    public interface IRubrosService
    {
        Task<IEnumerable<RubroDto>> GetAllRubrosAsync(bool includeInactive = false);
        Task<RubroDto?> GetRubroByIdAsync(int id);
        Task<RubroDto> CreateRubroAsync(CreateRubroDto createRubroDto);
        Task<RubroDto> UpdateRubroAsync(int id, UpdateRubroDto updateRubroDto);
        Task DeleteRubroAsync(int id);
    }
}
