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
    }
}
