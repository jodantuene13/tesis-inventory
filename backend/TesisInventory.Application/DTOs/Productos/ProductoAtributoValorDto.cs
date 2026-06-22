using System.Collections.Generic;
using TesisInventory.Application.DTOs;

namespace TesisInventory.Application.DTOs.Productos
{
    public class ProductoAtributoValorDto
    {
        public int IdAtributo { get; set; }
        public string CodigoAtributo { get; set; } = string.Empty;
        public string NombreAtributo { get; set; } = string.Empty;
        public string TipoDatoAtributo { get; set; } = string.Empty;

        // Unidad seleccionada para este valor en este producto
        public int? IdUnidadMedida { get; set; }
        public string? SimboloUnidad { get; set; }

        // Unidades habilitadas para el atributo (para el selector en el formulario)
        public List<UnidadMedidaDto> UnidadesPermitidas { get; set; } = new();

        public string? ValorTexto { get; set; }
        public int? ValorNumero { get; set; }
        public decimal? ValorDecimal { get; set; }
        public bool? ValorBool { get; set; }
        public string? ValorLista { get; set; }
    }

    public class CreateProductoAtributoValorDto
    {
        public int IdAtributo { get; set; }
        public int? IdUnidadMedida { get; set; }
        public string? ValorTexto { get; set; }
        public int? ValorNumero { get; set; }
        public decimal? ValorDecimal { get; set; }
        public bool? ValorBool { get; set; }
        public string? ValorLista { get; set; }
    }
}
