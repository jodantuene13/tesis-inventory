using System;
using TesisInventory.Domain.Enums;

namespace TesisInventory.Application.DTOs.Stock
{
    public class MovimientoDto
    {
        public int IdMovimiento { get; set; }
        public int IdProducto { get; set; }
        public string Sku { get; set; } = string.Empty;
        public string NombreProducto { get; set; } = string.Empty;
        public string UnidadMedida { get; set; } = string.Empty;
        public string RubroProducto { get; set; } = string.Empty;
        public string FamiliaProducto { get; set; } = string.Empty;
        public int IdSede { get; set; }
        public TipoMovimiento TipoMovimiento { get; set; }
        public string TipoMovimientoDescripcion => TipoMovimiento.ToString();
        public int Cantidad { get; set; }
        public int CantidadRestante { get; set; }
        public DateTime Fecha { get; set; }
        public MotivoMovimiento Motivo { get; set; }
        public string MotivoDescripcion => Motivo.ToString();
        public int IdUsuario { get; set; }
        public string NombreUsuario { get; set; } = string.Empty;
        public string? Observaciones { get; set; }
    }
}
