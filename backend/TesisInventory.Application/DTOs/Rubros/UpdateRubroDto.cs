using System.ComponentModel.DataAnnotations;

namespace TesisInventory.Application.DTOs.Rubros
{
    public class UpdateRubroDto
    {
        [Required(ErrorMessage = "El código es obligatorio.")]
        public string CodigoRubro { get; set; } = string.Empty;

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        public string Nombre { get; set; } = string.Empty;

        public bool Activo { get; set; }
    }
}
