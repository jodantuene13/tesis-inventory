using System;
using System.Collections.Generic;

namespace TesisInventory.Domain.Entities
{
    public class GrupoAtributo
    {
        public int IdGrupoAtributo { get; set; }
        public string CodigoGrupo { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public string Separador { get; set; } = "*";
        public string? UnidadSufijo { get; set; }
        public bool Activo { get; set; } = true;
        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;
        public DateTime FechaActualizacion { get; set; } = DateTime.UtcNow;

        public ICollection<GrupoAtributoItem> Items { get; set; } = new List<GrupoAtributoItem>();
        public ICollection<FamiliaGrupoAtributo> FamiliaGrupoAtributos { get; set; } = new List<FamiliaGrupoAtributo>();
    }
}
