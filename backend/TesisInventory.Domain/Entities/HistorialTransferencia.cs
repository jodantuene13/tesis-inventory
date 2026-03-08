using System;
using TesisInventory.Domain.Enums;

namespace TesisInventory.Domain.Entities
{
    public class HistorialTransferencia
    {
        public int IdHistorialTransferencia { get; set; }
        public int IdTransferencia { get; set; }
        public EstadoTransferencia EstadoAnterior { get; set; }
        public EstadoTransferencia EstadoNuevo { get; set; }
        public DateTime Fecha { get; set; } = DateTime.UtcNow;
        public int IdUsuario { get; set; }
        public string? Observaciones { get; set; }

        public virtual Transferencia? Transferencia { get; set; }
        public virtual Usuario? Usuario { get; set; }
    }
}
