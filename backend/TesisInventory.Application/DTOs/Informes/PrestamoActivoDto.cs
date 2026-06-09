using System;

namespace TesisInventory.Application.DTOs.Informes
{
    public class PrestamoActivoDto
    {
        public int IdTransferencia { get; set; }
        public string Productos { get; set; } = string.Empty;
        public string SedeOrigen { get; set; } = string.Empty;
        public string SedeDestino { get; set; } = string.Empty;
        public DateTime FechaSolicitud { get; set; }
        public DateTime? FechaDevolucionEsperada { get; set; }
        public int DiasTranscurridos { get; set; }
        public string Estado { get; set; } = string.Empty;
    }
}
