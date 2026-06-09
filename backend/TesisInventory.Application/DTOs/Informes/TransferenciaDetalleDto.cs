using System;

namespace TesisInventory.Application.DTOs.Informes
{
    public class TransferenciaDetalleDto
    {
        public int IdTransferencia { get; set; }
        public DateTime FechaSolicitud { get; set; }
        public string Tipo { get; set; } = string.Empty;
        public string Productos { get; set; } = string.Empty;
        public string SedeOrigen { get; set; } = string.Empty;
        public string SedeDestino { get; set; } = string.Empty;
        public int CantidadTotal { get; set; }
        public string Estado { get; set; } = string.Empty;
        public string Usuario { get; set; } = string.Empty;
    }
}
