using System.Threading.Tasks;
using TesisInventory.Domain.Entities;

namespace TesisInventory.Domain.Interfaces
{
    public interface IOperacionStockRepository
    {
        Task<OperacionStock> AddOperacionStockAsync(OperacionStock operacionStock);
        Task<OperacionStock?> GetByIdAsync(int idOperacion);
        Task<(System.Collections.Generic.IEnumerable<OperacionStock> Items, int TotalCount)> GetOperacionesPaginadasAsync(
            int idSede,
            string? search, 
            string? tipoOperacion, 
            int? idUsuario, 
            string? fechaDesde, 
            string? fechaHasta, 
            int skip, 
            int take);
    }
}
