using System;
using TesisInventory.Domain.Enums;

namespace TesisInventory.Domain.Entities
{
    public class SolicitudCompra
    {
        public int IdSolicitudCompra { get; set; }
        public int IdSede { get; set; }
        public int IdUsuarioSolicitante { get; set; }
        public int? IdUsuarioAprobador { get; set; }
        
        public string? MotivoSolicitud { get; set; }
        public string? OrdenTrabajo { get; set; }
        public string? TicketSolicitud { get; set; }
        public string? TareaARealizar { get; set; }

        public EstadoSolicitudCompra Estado { get; set; } = EstadoSolicitudCompra.Pendiente;
        public DateTime FechaSolicitud { get; set; } = DateTime.UtcNow;
        public DateTime? FechaDecision { get; set; }
        public string? Observaciones { get; set; }
        public string? MotivoRechazo { get; set; }

        public virtual Sede? Sede { get; set; }
        public virtual Usuario? UsuarioSolicitante { get; set; }
        public virtual Usuario? UsuarioAprobador { get; set; }

        public virtual ICollection<SolicitudCompraDetalle> Detalles { get; set; } = new List<SolicitudCompraDetalle>();
    }
}
