using System.ComponentModel.DataAnnotations;

namespace TesisInventory.Application.DTOs.Sedes
{
    public class CreateSedeDto
    {
        [Required(ErrorMessage = "El nombre de la sede es obligatorio")]
        public string NombreSede { get; set; } = string.Empty;
        public string Direccion { get; set; } = string.Empty;
    }
}
