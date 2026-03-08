using System.ComponentModel.DataAnnotations;
using TesisInventory.Domain.Enums;

namespace TesisInventory.Application.DTOs.Stock
{
    public class RegistrarConsumoDto
    {
        [Required]
        public int IdProducto { get; set; }
        
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser mayor a 0.")]
        public int Cantidad { get; set; }

        [Required]
        public MotivoMovimiento Motivo { get; set; }
    }
}
