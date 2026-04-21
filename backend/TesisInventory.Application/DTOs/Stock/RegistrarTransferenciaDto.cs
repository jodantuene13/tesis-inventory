using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TesisInventory.Application.DTOs.Stock
{
    public class RegistrarTransferenciaDto
    {
        [Required]
        public int IdSedeDestino { get; set; }

        [Required]
        [MinLength(1, ErrorMessage = "Debe incluir al menos un producto en la transferencia.")]
        public List<RegistrarTransferenciaDetalleDto> Detalles { get; set; } = new List<RegistrarTransferenciaDetalleDto>();

        public string? Observaciones { get; set; }
    }
}
