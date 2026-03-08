using System.ComponentModel.DataAnnotations;

namespace TesisInventory.Application.DTOs.Stock
{
    public class RegistrarTransferenciaDto
    {
        [Required]
        public int IdProducto { get; set; }

        [Required]
        public int IdSedeDestino { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser mayor a 0.")]
        public int Cantidad { get; set; }

        public string? Observaciones { get; set; }
    }
}
