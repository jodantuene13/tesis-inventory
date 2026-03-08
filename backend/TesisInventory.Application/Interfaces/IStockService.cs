using System.Collections.Generic;
using System.Threading.Tasks;
using TesisInventory.Application.DTOs.Stock;

namespace TesisInventory.Application.Interfaces
{
    public interface IStockService
    {
        Task<(IEnumerable<StockDto> Items, int TotalCount)> GetStockAsync(
            int idSede,
            string? searchSkuOrName = null,
            int? idRubro = null,
            int? idFamilia = null,
            bool? estado = null,
            bool? bajoStock = null,
            int skip = 0,
            int take = 50);

        Task<StockDto> IncrementarStockAsync(int idSede, int idUsuario, IncrementarStockDto dto);
        Task<StockDto> RegistrarConsumoAsync(int idSede, int idUsuario, RegistrarConsumoDto dto);
        Task<TransferenciaDto> RegistrarTransferenciaAsync(int idSedeOrigen, int idUsuario, RegistrarTransferenciaDto dto);
        Task<IEnumerable<MovimientoDto>> GetHistorialMovimientosAsync(int idProducto, int idSede, string? tipoMovimiento = null, string? fechaDesde = null, string? fechaHasta = null);
        Task<IEnumerable<TransferenciaDto>> GetTransferenciasSedeAsync(int idSede);
    }
}
