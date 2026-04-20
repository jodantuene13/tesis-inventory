using System.ComponentModel.DataAnnotations;
using TesisInventory.Domain.Enums;

namespace TesisInventory.Application.DTOs.Transferencias
{
    public class CreateTransferenciaDto
    {
        [Required]
        public int IdProducto { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser mayor a 0.")]
        public int Cantidad { get; set; }

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
