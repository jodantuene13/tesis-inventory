using System.Collections.Generic;
using System.Threading.Tasks;
using TesisInventory.Domain.Entities;

namespace TesisInventory.Domain.Interfaces
{
    public interface IMovimientoRepository
    {
        Task<IEnumerable<Movimiento>> GetMovimientosAsync(int idProducto, int idSede, string? tipoMovimiento, string? fechaDesde, string? fechaHasta);
        Task<Movimiento> AddMovimientoAsync(Movimiento movimiento);
    }
}
