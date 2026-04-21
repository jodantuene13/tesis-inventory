using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TesisInventory.Domain.Enums;

namespace TesisInventory.Application.DTOs.Stock
{
    public class OperacionStockMultipleDto
    {
        [Required]
        public TipoMovimiento TipoOperacion { get; set; }
        
        [Required]
        public MotivoMovimiento Motivo { get; set; }

        public string? OrdenTrabajo { get; set; }
        public string? OrdenCompra { get; set; }
        public string? TicketSolicitud { get; set; }
        public string? Observaciones { get; set; }

        [Required]
        [MinLength(1, ErrorMessage = "Debe enviar al menos un producto en la operación.")]
        public List<DetalleOperacionStockDto> Detalles { get; set; } = new List<DetalleOperacionStockDto>();
    }

    public class DetalleOperacionStockDto
    {
        [Required]
        public int IdProducto { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser mayor a 0.")]
        public int Cantidad { get; set; }
    }
}
