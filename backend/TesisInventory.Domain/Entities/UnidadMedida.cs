using System.Collections.Generic;

namespace TesisInventory.Domain.Entities
{
    public class UnidadMedida
    {
        public int IdUnidadMedida { get; set; }
        public string Simbolo { get; set; } = string.Empty;   // mm, m, W, K, lm, kg
        public string Nombre { get; set; } = string.Empty;    // milímetros, metros, watts
        public bool Activo { get; set; } = true;

        public ICollection<AtributoUnidadMedida> AtributoUnidades { get; set; } = new List<AtributoUnidadMedida>();
        public ICollection<GrupoAtributoItem> GrupoItems { get; set; } = new List<GrupoAtributoItem>();
        public ICollection<ProductoAtributoValor> ValoresProducto { get; set; } = new List<ProductoAtributoValor>();
    }
}
