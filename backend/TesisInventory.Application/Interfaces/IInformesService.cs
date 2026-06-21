using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TesisInventory.Application.DTOs.Informes;

namespace TesisInventory.Application.Interfaces
{
    public interface IInformesService
    {
        /// <summary>
        /// Retorna los datos completos para el informe de alertas de stock:
        /// productos en bajo stock, recurrencia y evolución semanal.
        /// </summary>
        Task<InformeAlertasStockDto> GetAlertasStockAsync(
            int? idSede = null,
            int? idFamilia = null,
            int semanas = 5);

        /// <summary>
        /// Retorna los datos del informe de Rotación de Productos:
        /// ranking por índice de rotación, ranking de ingresos y ranking de egresos.
        /// </summary>
        Task<InformeRotacionDto> GetRotacionProductosAsync(
            int? idSede, int? idFamilia,
            DateTime fechaDesde, DateTime fechaHasta,
            int topN = 10);

        Task<InformeTransferenciaDto> GetInformeTransferenciasAsync(
            int? idSedeOrigen, int? idSedeDestino, int? idFamilia,
            TesisInventory.Domain.Enums.MotivoTransferencia? motivo, TesisInventory.Domain.Enums.EstadoTransferencia? estado,
            DateTime fechaDesde, DateTime fechaHasta, int topN = 10);

        /// <summary>
        /// Retorna productos con stock que no tuvieron egresos en el período seleccionado.
        /// </summary>
        Task<IEnumerable<ProductoInmovilizadoDto>> GetStockInmovilizadoAsync(
            int? idSede, int? idFamilia,
            DateTime fechaDesde, DateTime fechaHasta);

        /// <summary>
        /// Retorna el ranking de familias por consumo (ratio egresos/ingresos) en el período.
        /// </summary>
        Task<IEnumerable<FamiliaConsumoDto>> GetFamiliasConsumoAsync(
            int? idSede, int? idFamilia,
            DateTime fechaDesde, DateTime fechaHasta);

        /// <summary>
        /// Retorna el informe completo de solicitudes de compra:
        /// KPIs, agrupación por usuario/sede, pendientes y productos más solicitados.
        /// </summary>
        Task<InformeSolicitudesCompraDto> GetInformeSolicitudesCompraAsync(
            int? idSede,
            DateTime fechaDesde,
            DateTime fechaHasta,
            int topN = 10);
    }
}
