using System;
using TesisInventory.Domain.Enums;

namespace TesisInventory.Application.DTOs.Stock
{
    public class TransferenciaDto
    {
        public int IdTransferencia { get; set; }
        public int IdProducto { get; set; }
        public string NombreProducto { get; set; } = string.Empty;
        public int IdSedeOrigen { get; set; }
        public string NombreSedeOrigen { get; set; } = string.Empty;
        public int IdSedeDestino { get; set; }
        public string NombreSedeDestino { get; set; } = string.Empty;
        public int Cantidad { get; set; }
        public int? StockOrigenSnapshot { get; set; }
        public DateTime FechaSolicitud { get; set; }
        public EstadoTransferencia Estado { get; set; }
        public string EstadoDescripcion => Estado.ToString();
        public int IdUsuarioSolicita { get; set; }
        public string NombreUsuarioSolicita { get; set; } = string.Empty;
        public string? Observaciones { get; set; }
    }
}
