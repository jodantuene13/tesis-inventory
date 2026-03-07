using System;

namespace TesisInventory.Domain.Entities
{
    public class ProductoAtributoValor
    {
        public int IdProductoAtributoValor { get; set; }
        public int IdProducto { get; set; }
        public int IdAtributo { get; set; }
        
        // Almacenamiento flexible según el tipo
        public string? ValorTexto { get; set; }
        public int? ValorNumero { get; set; }
        public decimal? ValorDecimal { get; set; }
        public bool? ValorBool { get; set; }
        public string? ValorLista { get; set; } // Podría guardar el código de la opción

        public DateTime FechaActualizacion { get; set; } = DateTime.UtcNow;

        // Propiedades de navegación
        public Producto? Producto { get; set; }
        public Atributo? Atributo { get; set; }
    }
}
