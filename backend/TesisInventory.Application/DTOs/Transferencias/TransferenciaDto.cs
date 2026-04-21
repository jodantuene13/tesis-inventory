using System;
using System.Collections.Generic;
using TesisInventory.Domain.Enums;

namespace TesisInventory.Application.DTOs.Transferencias
{
    public class TransferenciaDto
    {
        public int IdTransferencia { get; set; }
        public string CodigoTracking => $"TR-{FechaSolicitud:ddMMyy}-{IdTransferencia}";
        public int IdSedeOrigen { get; set; }
        public string NombreSedeOrigen { get; set; } = string.Empty;
        public int IdSedeDestino { get; set; }
        public string NombreSedeDestino { get; set; } = string.Empty;
        public List<TransferenciaDetalleDto> Detalles { get; set; } = new List<TransferenciaDetalleDto>();
        public DateTime FechaSolicitud { get; set; }
        public EstadoTransferencia Estado { get; set; }
        public MotivoTransferencia Motivo { get; set; }
        public int IdUsuarioSolicita { get; set; }
        public string NombreUsuarioSolicita { get; set; } = string.Empty;
        public string? Observaciones { get; set; }
    }
}
