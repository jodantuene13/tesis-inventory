using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TesisInventory.Domain.Enums;

namespace TesisInventory.Application.DTOs.Transferencias
{
    public class CreateTransferenciaDto
    {
        [Required]
        [MinLength(1, ErrorMessage = "Debe incluir al menos un producto en la transferencia.")]
        public List<CreateTransferenciaDetalleDto> Detalles { get; set; } = new List<CreateTransferenciaDetalleDto>();

        [Required]
        public int IdSedeOrigen { get; set; }

        [Required]
        public int IdSedeDestino { get; set; }

        [Required]
        public MotivoTransferencia Motivo { get; set; }

        // The requesting user is usually taken from the logged in user context
        // but can be included if required
        
        public string? Observaciones { get; set; }
    }
}
