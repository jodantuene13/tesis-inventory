using System;

namespace TesisInventory.Application.DTOs.Informes
{
    /// <summary>
    /// Representa un producto actualmente en estado de bajo stock (alerta activa).
    /// Alimenta la tab "Bajo Stock" del informe.
    /// </summary>
    public class ProductoAlertaStockDto
    {
        public int IdAlertaStock { get; set; }
        public int IdProducto { get; set; }
        public string Producto { get; set; } = string.Empty;
        public string Familia { get; set; } = string.Empty;
        public string Sede { get; set; } = string.Empty;
        public int StockActual { get; set; }
        public int StockMinimo { get; set; }
        public int Diferencia { get; set; }
        public string UnidadMedida { get; set; } = "u.";

        /// <summary>Días transcurridos desde que se creó la alerta.</summary>
        public int DiasEnAlerta { get; set; }

        public DateTime UltimaAlerta { get; set; }

        /// <summary>Alta / Media / Baja – calculado en runtime.</summary>
        public string Criticidad { get; set; } = string.Empty;
    }
}
