using System.Collections.Generic;
using System.Threading.Tasks;
using TesisInventory.Application.DTOs.Productos;

namespace TesisInventory.Application.Interfaces
{
    public interface IProductosService
    {
        Task<IEnumerable<ProductoDto>> GetAllProductosAsync(bool includeInactive = false);
        Task<ProductoDto?> GetProductoByIdAsync(int id);
        Task<ProductoDto> CreateProductoAsync(CreateProductoDto createDto);
        Task<ProductoDto> UpdateProductoAsync(int id, UpdateProductoDto updateDto);
        Task DeleteProductoAsync(int id, string confirmacionNombre); // Baja lógica estricta
    }
}
