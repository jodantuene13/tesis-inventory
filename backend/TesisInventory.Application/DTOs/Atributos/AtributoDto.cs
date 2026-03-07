using System;

namespace TesisInventory.Application.DTOs.Atributos
{
    public class AtributoDto
    {
        public int IdAtributo { get; set; }
        public string CodigoAtributo { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public string TipoDato { get; set; } = string.Empty;
        public string? Unidad { get; set; }
        public string? Descripcion { get; set; }
        public bool Activo { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaActualizacion { get; set; }
    }
}
