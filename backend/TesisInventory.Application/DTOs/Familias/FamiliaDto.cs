using System;

namespace TesisInventory.Application.DTOs.Familias
{
    public class FamiliaDto
    {
        public int IdFamilia { get; set; }
        public int IdRubro { get; set; }
        public string NombreRubro { get; set; } = string.Empty;
        public string CodigoFamilia { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public bool Activo { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaActualizacion { get; set; }
    }
}
