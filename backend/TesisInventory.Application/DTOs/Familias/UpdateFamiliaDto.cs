using System.ComponentModel.DataAnnotations;

namespace TesisInventory.Application.DTOs.Familias
{
    public class UpdateFamiliaDto
    {
        [Required(ErrorMessage = "El rubro es obligatorio.")]
        public int IdRubro { get; set; }

        [Required(ErrorMessage = "El código de familia es obligatorio.")]
        public string CodigoFamilia { get; set; } = string.Empty;

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        public string Nombre { get; set; } = string.Empty;

        public bool Activo { get; set; }
    }
}
