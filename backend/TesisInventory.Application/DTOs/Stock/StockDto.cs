using System;
using TesisInventory.Domain.Enums;

namespace TesisInventory.Application.DTOs.Stock
{
    public class StockDto
    {
        public int IdStock { get; set; }
        public int IdProducto { get; set; }
        public string Sku { get; set; } = string.Empty;
        public string NombreProducto { get; set; } = string.Empty;
        public string UnidadMedida { get; set; } = string.Empty;
        public string RubroProducto { get; set; } = string.Empty;
        public string FamiliaProducto { get; set; } = string.Empty;
        public bool EstadoProducto { get; set; }
        
        public int IdSede { get; set; }
        public int CantidadActual { get; set; }
        public int PuntoReposicion { get; set; }
        public DateTime FechaActualizacion { get; set; }
        public bool ConBajoStock => CantidadActual <= PuntoReposicion;

        public List<TesisInventory.Application.DTOs.Productos.ProductoAtributoValorDto> Atributos { get; set; } = new();
    }
}
