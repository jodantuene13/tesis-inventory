using System;
using TesisInventory.Domain.Enums;

namespace TesisInventory.Application.DTOs.SolicitudesCompra
{
    public class SolicitudCompraDto
    {
        public int IdSolicitudCompra { get; set; }
        public int IdSede { get; set; }
        public string NombreSede { get; set; } = string.Empty;
        public int IdUsuarioSolicitante { get; set; }
        public string NombreSolicitante { get; set; } = string.Empty;
        public int? IdUsuarioAprobador { get; set; }
        public string? NombreAprobador { get; set; }
        
        public string? MotivoSolicitud { get; set; }
        public string? OrdenTrabajo { get; set; }
        public string? TicketSolicitud { get; set; }
        public string? TareaARealizar { get; set; }

        public EstadoSolicitudCompra Estado { get; set; }
        public DateTime FechaSolicitud { get; set; }
        public DateTime? FechaDecision { get; set; }
        public string? Observaciones { get; set; }
        public string? MotivoRechazo { get; set; }

        public List<SolicitudCompraDetalleDto> Detalles { get; set; } = new List<SolicitudCompraDetalleDto>();
    }

    public class SolicitudCompraDetalleDto
    {
        public int IdProducto { get; set; }
        public string NombreProducto { get; set; } = string.Empty;
        public string SkuProducto { get; set; } = string.Empty;
        public int Cantidad { get; set; }
    }

    public class CreateSolicitudCompraDto
    {
        public string? MotivoSolicitud { get; set; }
        public string? OrdenTrabajo { get; set; }
        public string? TicketSolicitud { get; set; }
        public string? TareaARealizar { get; set; }
        public string? Observaciones { get; set; }

        public List<CreateSolicitudCompraDetalleDto> Detalles { get; set; } = new List<CreateSolicitudCompraDetalleDto>();
    }

    public class CreateSolicitudCompraDetalleDto
    {
        public int IdProducto { get; set; }
        public int Cantidad { get; set; }
    }

    public class UpdateSolicitudCompraEstadoDto
    {
        public EstadoSolicitudCompra NuevoEstado { get; set; }
        public string? MotivoRechazo { get; set; }
    }
}
