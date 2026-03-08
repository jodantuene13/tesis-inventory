using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TesisInventory.Application.DTOs.Productos
{
    public class CreateProductoDto
    {
        [Required]
        public int IdRubro { get; set; }

        [Required]
        public int IdFamilia { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "La unidad de medida es obligatoria.")]
        public string UnidadMedida { get; set; } = string.Empty;

        public bool Activo { get; set; } = true;

        public List<CreateProductoAtributoValorDto> Atributos { get; set; } = new List<CreateProductoAtributoValorDto>();
    }
}
