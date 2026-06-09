using System;
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
    }
}
