using System;

namespace TesisInventory.Application.DTOs.Rubros
{
    public class RubroDto
    {
        public int IdRubro { get; set; }
        public string CodigoRubro { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public bool Activo { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaActualizacion { get; set; }
    }
}
