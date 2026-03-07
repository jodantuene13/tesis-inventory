using System.Collections.Generic;
using System.Threading.Tasks;
using TesisInventory.Application.DTOs.Atributos;

namespace TesisInventory.Application.Interfaces
{
    public interface IAtributosService
    {
        // Administracion maestra de Atributos
        Task<IEnumerable<AtributoDto>> GetAllAtributosAsync(bool includeInactive = false);
        Task<AtributoDto?> GetAtributoByIdAsync(int id);
        Task<AtributoDto> CreateAtributoAsync(CreateAtributoDto createDto);
        Task<AtributoDto> UpdateAtributoAsync(int id, UpdateAtributoDto updateDto);
        Task DeleteAtributoAsync(int id); // Baja logica

        // Opciones de Atributo List (Ej. Color: Rojo, Azul)
        Task<IEnumerable<AtributoOpcionDto>> GetOpcionesAsync(int idAtributo);
        Task<AtributoOpcionDto> AddOpcionAsync(int idAtributo, CreateAtributoOpcionDto createDto);
        Task DeleteOpcionAsync(int idOpcion);

        // Configuracion Familia-Atributo
        Task<IEnumerable<FamiliaAtributoDto>> GetAtributosDeFamiliaAsync(int idFamilia);
        Task<FamiliaAtributoDto> AssignAtributoToFamiliaAsync(int idFamilia, CreateFamiliaAtributoDto req);
        Task RemoveAtributoFromFamiliaAsync(int idFamilia, int idAtributo);
    }
}
