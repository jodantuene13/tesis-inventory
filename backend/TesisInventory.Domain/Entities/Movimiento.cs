using System;
using TesisInventory.Domain.Enums;

namespace TesisInventory.Domain.Entities
{
    public class Movimiento
    {
        public int IdMovimiento { get; set; }
        public int IdProducto { get; set; }
        public int IdSede { get; set; }
        public TipoMovimiento TipoMovimiento { get; set; }
        public int Cantidad { get; set; }
        public int CantidadRestante { get; set; }
        public DateTime Fecha { get; set; } = DateTime.UtcNow;
        public MotivoMovimiento Motivo { get; set; }
        public int IdUsuario { get; set; }
        public string? Observaciones { get; set; }

        // Propiedades de navegación
        public virtual Producto? Producto { get; set; }
        public virtual Sede? Sede { get; set; }
        public virtual Usuario? Usuario { get; set; }
    }
}
