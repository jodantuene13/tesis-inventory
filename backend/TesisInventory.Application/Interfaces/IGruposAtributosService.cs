using System.Collections.Generic;
using System.Threading.Tasks;
using TesisInventory.Application.DTOs.Atributos;

namespace TesisInventory.Application.Interfaces
{
    public interface IGruposAtributosService
    {
        // Grupos maestros
        Task<IEnumerable<GrupoAtributoDto>> GetAllGruposAsync(bool includeInactive = false);
        Task<GrupoAtributoDto?> GetGrupoByIdAsync(int id);
        Task<GrupoAtributoDto> CreateGrupoAsync(CreateGrupoAtributoDto dto);
        Task<GrupoAtributoDto> UpdateGrupoAsync(int id, UpdateGrupoAtributoDto dto);
        Task DeleteGrupoAsync(int id);

        // Items del grupo (atributos que lo componen)
        Task<GrupoAtributoItemDto> AddItemAsync(int idGrupo, AddItemToGrupoDto dto);
        Task DeleteItemAsync(int idGrupo, int idAtributo);

        // Asignación a familia
        Task<IEnumerable<FamiliaGrupoAtributoDto>> GetGruposDeFamiliaAsync(int idFamilia);
        Task<IEnumerable<FamiliaGrupoAtributoDto>> GetFamiliasByGrupoAsync(int idGrupo);
        Task<FamiliaGrupoAtributoDto> AssignGrupoToFamiliaAsync(int idFamilia, CreateFamiliaGrupoAtributoDto dto);
        Task RemoveGrupoFromFamiliaAsync(int idFamilia, int idGrupo);
    }
}
