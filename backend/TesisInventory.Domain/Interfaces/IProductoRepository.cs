using System.Collections.Generic;
using System.Threading.Tasks;
using TesisInventory.Domain.Entities;

namespace TesisInventory.Domain.Interfaces
{
    public interface IProductoRepository
    {
        Task<IEnumerable<Producto>> GetAllProductosAsync(bool includeInactive = false);
        Task<Producto?> GetProductoByIdAsync(int id);
        Task<Producto?> GetProductoBySkuAsync(string sku);
        Task<Producto> AddProductoAsync(Producto producto);
        Task UpdateProductoAsync(Producto producto);
        Task DeleteProductoAsync(Producto producto); // Baja Lógica
        
        // Atributos Valor
        Task<IEnumerable<ProductoAtributoValor>> GetAtributosValorByProductoAsync(int idProducto);
        Task AddProductoAtributoValorAsync(ProductoAtributoValor valor);
        Task UpdateProductoAtributoValorAsync(ProductoAtributoValor valor);
        Task RemoveProductoAtributoValorAsync(ProductoAtributoValor valor);
        Task<ProductoAtributoValor?> GetAtributoValorAsync(int idProducto, int idAtributo);
    }
}
