using System;
using System.Collections.Generic;
using TesisInventory.Domain.Enums;

namespace TesisInventory.Domain.Entities
{
    public class Transferencia
    {
        public int IdTransferencia { get; set; }
        public int IdProducto { get; set; }
        public int IdSedeOrigen { get; set; }
        public int IdSedeDestino { get; set; }
        public int Cantidad { get; set; }
        public DateTime FechaSolicitud { get; set; } = DateTime.UtcNow;
        public EstadoTransferencia Estado { get; set; } = EstadoTransferencia.Solicitada;
        public int IdUsuarioSolicita { get; set; }
        public string? Observaciones { get; set; }

        public virtual Producto? Producto { get; set; }
        public virtual Sede? SedeOrigen { get; set; }
        public virtual Sede? SedeDestino { get; set; }
        public virtual Usuario? UsuarioSolicita { get; set; }

        public virtual ICollection<HistorialTransferencia> HistorialTransferencias { get; set; } = new List<HistorialTransferencia>();
    }
}
