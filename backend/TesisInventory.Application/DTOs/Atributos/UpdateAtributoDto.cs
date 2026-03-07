using System.ComponentModel.DataAnnotations;

namespace TesisInventory.Application.DTOs.Atributos
{
    public class UpdateAtributoDto
    {
        [Required(ErrorMessage = "El código de atributo es obligatorio.")]
        public string CodigoAtributo { get; set; } = string.Empty;

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "El tipo de dato es obligatorio.")]
        public string TipoDato { get; set; } = string.Empty;

        public string? Unidad { get; set; }
        public string? Descripcion { get; set; }
        public bool Activo { get; set; }
    }
}
