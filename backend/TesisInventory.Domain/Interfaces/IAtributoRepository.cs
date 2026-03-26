using System.Collections.Generic;
using System.Threading.Tasks;
using TesisInventory.Domain.Entities;

namespace TesisInventory.Domain.Interfaces
{
    public interface IAtributoRepository
    {
        // Atributo CRUD
        Task<IEnumerable<Atributo>> GetAllAtributosAsync(bool includeInactive = false);
        Task<Atributo?> GetAtributoByIdAsync(int id);
        Task<Atributo?> GetAtributoByCodigoAsync(string codigo);
        Task<Atributo> AddAtributoAsync(Atributo atributo);
        Task UpdateAtributoAsync(Atributo atributo);
        Task DeleteAtributoAsync(Atributo atributo);
        
        // AtributoOpcion
        Task<IEnumerable<AtributoOpcion>> GetOpcionesByAtributoIdAsync(int idAtributo);
        Task<AtributoOpcion> AddOpcionAsync(AtributoOpcion opcion);
        Task UpdateOpcionAsync(AtributoOpcion opcion);
        Task DeleteOpcionAsync(AtributoOpcion opcion); // Delete físico o lógico según caso
        Task<AtributoOpcion?> GetOpcionByIdAsync(int idOpcion);

        // FamiliaAtributo
        Task<IEnumerable<FamiliaAtributo>> GetAtributosByFamiliaIdAsync(int idFamilia);
        Task<FamiliaAtributo> AddFamiliaAtributoAsync(FamiliaAtributo familiaAtributo);
        Task UpdateFamiliaAtributoAsync(FamiliaAtributo familiaAtributo);
        Task DeleteFamiliaAtributoAsync(FamiliaAtributo familiaAtributo);
        Task<FamiliaAtributo?> GetFamiliaAtributoAsync(int idFamilia, int idAtributo);
        Task<IEnumerable<FamiliaAtributo>> GetFamiliasByAtributoIdAsync(int idAtributo);
    }
}
