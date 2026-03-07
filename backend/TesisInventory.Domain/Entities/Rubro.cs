using System;
using System.Collections.Generic;

namespace TesisInventory.Domain.Entities
{
    public class Rubro
    {
        public int IdRubro { get; set; }
        public string CodigoRubro { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public bool Activo { get; set; } = true;
        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;
        public DateTime FechaActualizacion { get; set; } = DateTime.UtcNow;

        // Propiedad de navegación
        public ICollection<Familia> Familias { get; set; } = new List<Familia>();
    }
}
