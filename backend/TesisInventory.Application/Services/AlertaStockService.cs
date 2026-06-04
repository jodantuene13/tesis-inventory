using System;
using System.Threading.Tasks;
using TesisInventory.Application.Interfaces;
using TesisInventory.Domain.Entities;
using TesisInventory.Domain.Enums;
using TesisInventory.Domain.Interfaces;

namespace TesisInventory.Application.Services
{
    public class AlertaStockService : IAlertaStockService
    {
        private readonly IAlertaStockRepository _alertaRepository;

        public AlertaStockService(IAlertaStockRepository alertaRepository)
        {
            _alertaRepository = alertaRepository;
        }

        public async Task VerificarYRegistrarAlertaAsync(int idProducto, int idSede, int cantidadActual, int puntoReposicion)
        {
            var alertaActiva = await _alertaRepository.GetAlertaActivaAsync(idProducto, idSede);

            bool enAlerta = cantidadActual <= puntoReposicion;

            if (enAlerta)
            {
                if (alertaActiva == null)
                {
                    // Primera vez que este producto/sede cae en alerta → crear registro
                    var nuevaAlerta = new AlertaStock
                    {
                        IdProducto = idProducto,
                        IdSede = idSede,
                        StockAlMomento = cantidadActual,
                        PuntoReposicion = puntoReposicion,
                        FechaCreacion = DateTime.UtcNow,
                        FechaUltimaAlerta = DateTime.UtcNow,
                        Estado = EstadoAlerta.Activa
                    };
                    await _alertaRepository.AddAsync(nuevaAlerta);
                }
                else
                {
                    // Ya existe alerta activa → actualizar snapshot y timestamp
                    alertaActiva.StockAlMomento = cantidadActual;
                    alertaActiva.FechaUltimaAlerta = DateTime.UtcNow;
                    await _alertaRepository.UpdateAsync(alertaActiva);
                }
            }
            else
            {
                // El stock superó el umbral → resolver alerta activa si existe
                if (alertaActiva != null)
                {
                    alertaActiva.Estado = EstadoAlerta.Resuelta;
                    alertaActiva.StockAlMomento = cantidadActual;
                    await _alertaRepository.UpdateAsync(alertaActiva);
                }
            }
        }
    }
}
