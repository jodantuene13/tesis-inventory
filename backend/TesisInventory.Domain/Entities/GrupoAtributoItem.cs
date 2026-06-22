namespace TesisInventory.Domain.Entities
{
    public class GrupoAtributoItem
    {
        public int IdGrupoAtributoItem { get; set; }
        public int IdGrupoAtributo { get; set; }
        public int IdAtributo { get; set; }
        public int Orden { get; set; } = 0;
        public int? IdUnidadMedida { get; set; }
        public bool Activo { get; set; } = true;

        public GrupoAtributo? GrupoAtributo { get; set; }
        public Atributo? Atributo { get; set; }
        public UnidadMedida? UnidadMedida { get; set; }
    }
}
