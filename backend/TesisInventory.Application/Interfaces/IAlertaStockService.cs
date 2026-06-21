using System.Threading.Tasks;

namespace TesisInventory.Application.Interfaces
{
    public interface IAlertaStockService
    {
        /// <summary>
        /// Verifica si el stock actual está en alerta (CantidadActual &lt;= PuntoReposicion).
        /// - Si está en alerta y NO existe alerta activa → crea una nueva.
        /// - Si está en alerta y YA existe alerta activa → actualiza FechaUltimaAlerta y StockAlMomento.
        /// - Si NO está en alerta y existe alerta activa → la resuelve (estado Resuelta).
        /// </summary>
        Task VerificarYRegistrarAlertaAsync(int idProducto, int idSede, int cantidadActual, int puntoReposicion);
    }
}
