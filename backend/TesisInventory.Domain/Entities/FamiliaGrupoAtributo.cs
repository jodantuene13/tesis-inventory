using System;

namespace TesisInventory.Domain.Entities
{
    public class FamiliaGrupoAtributo
    {
        public int IdFamiliaGrupoAtributo { get; set; }
        public int IdFamilia { get; set; }
        public int IdGrupoAtributo { get; set; }
        public bool Obligatorio { get; set; } = false;
        public bool Activo { get; set; } = true;
        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;
        public DateTime FechaActualizacion { get; set; } = DateTime.UtcNow;

        public Familia? Familia { get; set; }
        public GrupoAtributo? GrupoAtributo { get; set; }
    }
}
