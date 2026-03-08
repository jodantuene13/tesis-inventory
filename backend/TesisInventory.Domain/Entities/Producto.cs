using System;
using System.Collections.Generic;

namespace TesisInventory.Domain.Entities
{
    public class Producto
    {
        public int IdProducto { get; set; }
        public int IdFamilia { get; set; }
        public string Sku { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public string UnidadMedida { get; set; } = string.Empty;
        public bool Activo { get; set; } = true;
        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;
        public DateTime FechaActualizacion { get; set; } = DateTime.UtcNow;

        // Propiedades de navegación
        public Familia? Familia { get; set; }
        public ICollection<ProductoAtributoValor> ProductoAtributoValores { get; set; } = new List<ProductoAtributoValor>();
        public ICollection<Stock> Stocks { get; set; } = new List<Stock>();
        public ICollection<Movimiento> Movimientos { get; set; } = new List<Movimiento>();
        public ICollection<Transferencia> Transferencias { get; set; } = new List<Transferencia>();
    }
}
