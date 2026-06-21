using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TesisInventory.Domain.Entities;
using TesisInventory.Domain.Enums;

namespace TesisInventory.Domain.Interfaces
{
    public interface IAlertaStockRepository
    {
        /// <summary>Obtiene la alerta activa para un producto en una sede, si existe.</summary>
        Task<AlertaStock?> GetAlertaActivaAsync(int idProducto, int idSede);

        /// <summary>Persiste una nueva alerta.</summary>
        Task<AlertaStock> AddAsync(AlertaStock alerta);

        /// <summary>Actualiza una alerta existente (FechaUltimaAlerta, Estado, StockAlMomento).</summary>
        Task UpdateAsync(AlertaStock alerta);

        /// <summary>
        /// Devuelve todos los stocks actualmente en alerta (estado Activa) para el informe
        /// "Productos en bajo stock", con soporte de filtros opcionales.
        /// </summary>
        Task<IEnumerable<AlertaStock>> GetAlertasActivasAsync(
            int? idSede = null,
            int? idFamilia = null);

        /// <summary>
        /// Devuelve el historial de alertas (Activas + Resueltas) en el rango de fechas,
        /// agrupado por producto/sede para calcular recurrencia.
        /// </summary>
        Task<IEnumerable<AlertaStock>> GetHistorialAlertasAsync(
            int? idSede = null,
            int? idFamilia = null,
            DateTime? fechaDesde = null,
            DateTime? fechaHasta = null);

        /// <summary>
        /// Devuelve la cantidad de alertas registradas por semana en las últimas N semanas,
        /// para el gráfico de evolución temporal.
        /// </summary>
        Task<IEnumerable<(DateTime InicioSemana, int CantidadAlertas)>> GetEvolucionSemanalAsync(
            int? idSede = null,
            int semanas = 5);
    }
}
