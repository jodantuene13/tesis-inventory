using System;
using TesisInventory.Domain.Enums;

namespace TesisInventory.Domain.Entities
{
    public class AlertaStock
    {
        public int IdAlertaStock { get; set; }
        public int IdProducto { get; set; }
        public int IdSede { get; set; }

        /// <summary>Snapshot del stock actual en el momento en que se detectó la alerta.</summary>
        public int StockAlMomento { get; set; }

        /// <summary>Snapshot del punto de reposición vigente al momento de la alerta.</summary>
        public int PuntoReposicion { get; set; }

        /// <summary>Fecha en que se registró la primera alerta para este producto/sede.</summary>
        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;

        /// <summary>Fecha de la última vez que el stock volvió a caer por debajo del umbral.</summary>
        public DateTime FechaUltimaAlerta { get; set; } = DateTime.UtcNow;

        /// <summary>Estado actual de la alerta.</summary>
        public EstadoAlerta Estado { get; set; } = EstadoAlerta.Activa;

        // Propiedades de navegación
        public virtual Producto? Producto { get; set; }
        public virtual Sede? Sede { get; set; }
    }
}
