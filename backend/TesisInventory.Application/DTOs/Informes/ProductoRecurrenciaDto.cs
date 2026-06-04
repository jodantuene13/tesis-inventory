using System;

namespace TesisInventory.Application.DTOs.Informes
{
    /// <summary>
    /// Representa un producto con alto índice de recurrencia de alertas en el período.
    /// Alimenta la tab "Recurrencia" del informe.
    /// </summary>
    public class ProductoRecurrenciaDto
    {
        public int IdProducto { get; set; }
        public string Producto { get; set; } = string.Empty;
        public string Familia { get; set; } = string.Empty;
        public string Sede { get; set; } = string.Empty;

        /// <summary>Cantidad de veces que se registró una alerta (FechaUltimaAlerta se actualizó).</summary>
        public int CantidadAlertas { get; set; }

        /// <summary>Total de días acumulados en estado de alerta.</summary>
        public int DiasAcumulados { get; set; }

        public int StockActual { get; set; }
        public int StockMinimo { get; set; }
        public DateTime UltimaAlerta { get; set; }
        public string EstadoActual { get; set; } = string.Empty;
        public string Criticidad { get; set; } = string.Empty;
    }
}
