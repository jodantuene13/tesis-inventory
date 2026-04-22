using System;
using TesisInventory.Domain.Enums;

namespace TesisInventory.Application.DTOs.SolicitudesCompra
{
    public class SolicitudCompraDto
    {
        public int IdSolicitudCompra { get; set; }
        public int IdProducto { get; set; }
        public string NombreProducto { get; set; } = string.Empty;
        public string SkuProducto { get; set; } = string.Empty;
        public int IdSede { get; set; }
        public string NombreSede { get; set; } = string.Empty;
        public int IdUsuarioSolicitante { get; set; }
        public string NombreSolicitante { get; set; } = string.Empty;
        public int? IdUsuarioAprobador { get; set; }
        public string? NombreAprobador { get; set; }
        public int Cantidad { get; set; }
        public EstadoSolicitudCompra Estado { get; set; }
        public DateTime FechaSolicitud { get; set; }
        public DateTime? FechaDecision { get; set; }
        public string? Observaciones { get; set; }
        public string? MotivoRechazo { get; set; }
    }

    public class CreateSolicitudCompraDto
    {
        public int IdProducto { get; set; }
        public int Cantidad { get; set; }
        public string? Observaciones { get; set; }
    }

    public class UpdateSolicitudCompraEstadoDto
    {
        public EstadoSolicitudCompra NuevoEstado { get; set; }
        public string? MotivoRechazo { get; set; }
    }
}
