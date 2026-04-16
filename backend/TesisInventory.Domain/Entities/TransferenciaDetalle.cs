using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TesisInventory.Domain.Entities
{
    public class TransferenciaDetalle
    {
        public int IdTransferenciaDetalle { get; set; }
        public int IdTransferencia { get; set; }
        public int IdProducto { get; set; }
        public int Cantidad { get; set; }
        public int? StockOrigenSnapshot { get; set; }

        public virtual Transferencia? Transferencia { get; set; }
        public virtual Producto? Producto { get; set; }
    }
}
