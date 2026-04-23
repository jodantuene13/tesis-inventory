using System;
using System.Collections.Generic;
using TesisInventory.Domain.Enums;

namespace TesisInventory.Domain.Entities
{
    public class OperacionStock
    {
        public int IdOperacion { get; set; }
        public int IdSede { get; set; }
        public int IdUsuario { get; set; }
        public TipoMovimiento TipoOperacion { get; set; }
        public DateTime Fecha { get; set; } = DateTime.UtcNow;
        public MotivoMovimiento Motivo { get; set; }
        
        // Campos nuevos
        public string? OrdenTrabajo { get; set; }
        public string? OrdenCompra { get; set; }
        public string? TicketSolicitud { get; set; }
        public string? Observaciones { get; set; }

        public virtual Sede? Sede { get; set; }
        public virtual Usuario? Usuario { get; set; }
        public virtual ICollection<Movimiento> Movimientos { get; set; } = new List<Movimiento>();
    }
}
