using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TesisInventory.Domain.Entities;

namespace TesisInventory.Domain.Interfaces
{
    public interface IMovimientoRepository
    {
        Task<IEnumerable<Movimiento>> GetMovimientosAsync(int idProducto, int idSede, string? tipoMovimiento, string? fechaDesde, string? fechaHasta);
        Task<(IEnumerable<Movimiento> Items, int TotalCount)> GetHistorialGlobalAsync(
            int idSede, 
            string? search = null, 
            int? idRubro = null, 
            int? idFamilia = null, 
            string? tipoMovimiento = null, 
            int? idUsuario = null, 
            string? fechaDesde = null, 
            string? fechaHasta = null, 
            int skip = 0, 
            int take = 50);
        Task<Movimiento> AddMovimientoAsync(Movimiento movimiento);

        /// <summary>
        /// Retorna todos los movimientos de stock en el período indicado,
        /// junto con los datos de producto, familia y sede para calcular
        /// el índice de rotación (salidas / stock promedio ponderado).
        /// </summary>
        Task<IEnumerable<Movimiento>> GetDatosRotacionAsync(
            int? idSede,
            int? idFamilia,
            DateTime fechaDesde,
            DateTime fechaHasta);

        /// <summary>
        /// Retorna todos los ingresos (sin filtro de fecha) para las sedes/familias indicadas.
        /// Usado para determinar el último ingreso histórico por producto en stock inmovilizado.
        /// </summary>
        Task<IEnumerable<Movimiento>> GetIngresosAsync(
            int? idSede,
            int? idFamilia);
    }
}
