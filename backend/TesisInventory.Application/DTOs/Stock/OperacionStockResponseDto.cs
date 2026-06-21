using System;
using System.Collections.Generic;
using TesisInventory.Domain.Enums;

namespace TesisInventory.Application.DTOs.Stock
{
    public class OperacionStockResponseDto
    {
        public int IdOperacion { get; set; }
        public int IdSede { get; set; }
        public int IdUsuario { get; set; }
        public string NombreUsuario { get; set; } = string.Empty;
        public TipoMovimiento TipoOperacion { get; set; }
        public DateTime Fecha { get; set; }
        public MotivoMovimiento Motivo { get; set; }
        public string? OrdenTrabajo { get; set; }
        public string? OrdenCompra { get; set; }
        public string? TicketSolicitud { get; set; }
        public string? Observaciones { get; set; }

        public List<MovimientoDto> Detalles { get; set; } = new List<MovimientoDto>();
    }
}
