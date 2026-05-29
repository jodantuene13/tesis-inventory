namespace TesisInventory.Domain.Entities
{
    public class RolSede
    {
        public int IdRol { get; set; }
        public int IdSede { get; set; }

        public virtual Rol? Rol { get; set; }
        public virtual Sede? Sede { get; set; }
    }
}
