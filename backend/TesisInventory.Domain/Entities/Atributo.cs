using System;
using System.Collections.Generic;

namespace TesisInventory.Domain.Entities
{
    public class Atributo
    {
        public int IdAtributo { get; set; }
        public string CodigoAtributo { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public string TipoDato { get; set; } = string.Empty; // TEXT, NUMBER, DECIMAL, BOOL, LIST
        public string? Unidad { get; set; }
        public string? Descripcion { get; set; }
        public bool Activo { get; set; } = true;
        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;
        public DateTime FechaActualizacion { get; set; } = DateTime.UtcNow;

        // Propiedades de navegación
        public ICollection<FamiliaAtributo> FamiliaAtributos { get; set; } = new List<FamiliaAtributo>();
        public ICollection<AtributoOpcion> Opciones { get; set; } = new List<AtributoOpcion>();
    }
}
