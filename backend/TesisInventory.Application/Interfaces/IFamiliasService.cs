using System.Collections.Generic;
using System.Threading.Tasks;
using TesisInventory.Application.DTOs.Familias;

namespace TesisInventory.Application.Interfaces
{
    public interface IFamiliasService
    {
        Task<IEnumerable<FamiliaDto>> GetAllFamiliasAsync(bool includeInactive = false);
        Task<IEnumerable<FamiliaDto>> GetFamiliasByRubroAsync(int idRubro, bool includeInactive = false);
        Task<FamiliaDto?> GetFamiliaByIdAsync(int id);
        Task<FamiliaDto> CreateFamiliaAsync(CreateFamiliaDto createFamiliaDto);
        Task<FamiliaDto> UpdateFamiliaAsync(int id, UpdateFamiliaDto updateFamiliaDto);
        Task DeleteFamiliaAsync(int id);
    }
}
