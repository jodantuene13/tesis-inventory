using System;

namespace TesisInventory.Domain.Entities
{
    public class Stock
    {
        public int IdStock { get; set; }
        public int IdProducto { get; set; }
        public int IdSede { get; set; }
        public int CantidadActual { get; set; } = 0;
        public int PuntoReposicion { get; set; } = 0;
        public DateTime FechaActualizacion { get; set; } = DateTime.UtcNow;

        // Propiedades de navegación
        public Producto? Producto { get; set; }
        public Sede? Sede { get; set; }
    }
}
