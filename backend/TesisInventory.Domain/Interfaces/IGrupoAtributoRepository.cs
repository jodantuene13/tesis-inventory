using System.Collections.Generic;
using System.Threading.Tasks;
using TesisInventory.Domain.Entities;

namespace TesisInventory.Domain.Interfaces
{
    public interface IGrupoAtributoRepository
    {
        // GrupoAtributo CRUD
        Task<IEnumerable<GrupoAtributo>> GetAllAsync(bool includeInactive = false);
        Task<GrupoAtributo?> GetByIdAsync(int id);
        Task<GrupoAtributo?> GetByCodigoAsync(string codigo);
        Task<GrupoAtributo> AddAsync(GrupoAtributo grupo);
        Task UpdateAsync(GrupoAtributo grupo);
        Task DeleteAsync(GrupoAtributo grupo);

        // Items del grupo
        Task<IEnumerable<GrupoAtributoItem>> GetItemsByGrupoIdAsync(int idGrupo);
        Task<GrupoAtributoItem?> GetItemAsync(int idGrupo, int idAtributo);
        Task<GrupoAtributoItem> AddItemAsync(GrupoAtributoItem item);
        Task UpdateItemAsync(GrupoAtributoItem item);
        Task DeleteItemAsync(GrupoAtributoItem item);

        // Familia-Grupo
        Task<IEnumerable<FamiliaGrupoAtributo>> GetGruposByFamiliaIdAsync(int idFamilia);
        Task<IEnumerable<FamiliaGrupoAtributo>> GetFamiliasByGrupoIdAsync(int idGrupo);
        Task<FamiliaGrupoAtributo?> GetFamiliaGrupoAsync(int idFamilia, int idGrupo);
        Task<FamiliaGrupoAtributo> AddFamiliaGrupoAsync(FamiliaGrupoAtributo familiaGrupo);
        Task UpdateFamiliaGrupoAsync(FamiliaGrupoAtributo familiaGrupo);
        Task DeleteFamiliaGrupoAsync(FamiliaGrupoAtributo familiaGrupo);
    }
}
