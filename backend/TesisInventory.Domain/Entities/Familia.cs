using System;
using System.Collections.Generic;

namespace TesisInventory.Domain.Entities
{
    public class Familia
    {
        public int IdFamilia { get; set; }
        public int IdRubro { get; set; }
        public string CodigoFamilia { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public bool Activo { get; set; } = true;
        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;
        public DateTime FechaActualizacion { get; set; } = DateTime.UtcNow;

        // Propiedades de navegación
        public Rubro? Rubro { get; set; }
        public ICollection<Producto> Productos { get; set; } = new List<Producto>();
        public ICollection<FamiliaAtributo> FamiliaAtributos { get; set; } = new List<FamiliaAtributo>();
    }
}
