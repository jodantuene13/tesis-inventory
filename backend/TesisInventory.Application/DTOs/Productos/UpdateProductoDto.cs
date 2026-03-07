using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TesisInventory.Application.DTOs.Productos
{
    public class UpdateProductoDto
    {
        [Required(ErrorMessage = "El nombre es obligatorio.")]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "La unidad de medida es obligatoria.")]
        public string UnidadMedida { get; set; } = string.Empty;

        public bool Activo { get; set; }

        public List<CreateProductoAtributoValorDto> Atributos { get; set; } = new List<CreateProductoAtributoValorDto>();
    }
}
