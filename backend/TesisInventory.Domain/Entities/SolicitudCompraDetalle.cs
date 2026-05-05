using System;

namespace TesisInventory.Domain.Entities
{
    public class SolicitudCompraDetalle
    {
        public int IdSolicitudCompraDetalle { get; set; }
        public int IdSolicitudCompra { get; set; }
        public int IdProducto { get; set; }
        public int Cantidad { get; set; }

        public virtual SolicitudCompra? SolicitudCompra { get; set; }
        public virtual Producto? Producto { get; set; }
    }
}
