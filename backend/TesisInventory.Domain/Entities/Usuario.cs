using System;

namespace TesisInventory.Domain.Entities
{
    public class Usuario
    {
        public int IdUsuario { get; set; }
        public string NombreUsuario { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? GoogleId { get; set; }
        public string? Password { get; set; }
        public bool Estado { get; set; }
        public DateTime FechaRegistro { get; set; }
        public int IdRol { get; set; }
        public int IdSede { get; set; }

        public virtual Rol? Rol { get; set; }
        public virtual Sede? Sede { get; set; }
        public virtual ICollection<Movimiento> Movimientos { get; set; } = new List<Movimiento>();
        public virtual ICollection<Transferencia> TransferenciasSolicitadas { get; set; } = new List<Transferencia>();
        public virtual ICollection<HistorialTransferencia> HistorialTransferencias { get; set; } = new List<HistorialTransferencia>();
        public virtual ICollection<OperacionStock> OperacionesStock { get; set; } = new List<OperacionStock>();
    }
}
