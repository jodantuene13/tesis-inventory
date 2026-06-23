namespace TesisInventory.Domain.Entities
{
    public class AtributoUnidadMedida
    {
        public int IdAtributo { get; set; }
        public int IdUnidadMedida { get; set; }

        public Atributo? Atributo { get; set; }
        public UnidadMedida? UnidadMedida { get; set; }
    }
}
