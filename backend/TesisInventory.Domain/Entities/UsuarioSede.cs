namespace TesisInventory.Domain.Entities
{
    public class UsuarioSede
    {
        public int IdUsuario { get; set; }
        public int IdSede { get; set; }

        public virtual Usuario? Usuario { get; set; }
        public virtual Sede? Sede { get; set; }
    }
}
