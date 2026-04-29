using System.ComponentModel.DataAnnotations;

namespace TesisInventory.Application.DTOs.Stock
{
    public class RegistrarTransferenciaDetalleDto
    {
        [Required]
        public int IdProducto { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser mayor a 0.")]
        public int Cantidad { get; set; }
    }
}
