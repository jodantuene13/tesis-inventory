namespace TesisInventory.Application.DTOs.Productos
{
    public class ProductoAtributoValorDto
    {
        public int IdAtributo { get; set; }
        public string CodigoAtributo { get; set; } = string.Empty;
        public string NombreAtributo { get; set; } = string.Empty;
        public string TipoDatoAtributo { get; set; } = string.Empty;

        public string? ValorTexto { get; set; }
        public int? ValorNumero { get; set; }
        public decimal? ValorDecimal { get; set; }
        public bool? ValorBool { get; set; }
        public string? ValorLista { get; set; }
    }

    public class CreateProductoAtributoValorDto
    {
        public int IdAtributo { get; set; }
        public string? ValorTexto { get; set; }
        public int? ValorNumero { get; set; }
        public decimal? ValorDecimal { get; set; }
        public bool? ValorBool { get; set; }
        public string? ValorLista { get; set; }
    }
}
