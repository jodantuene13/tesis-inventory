namespace TesisInventory.Domain.Entities
{
    public class RolPermiso
    {
        public int IdRol { get; set; }
        public int IdPermiso { get; set; }

        public virtual Rol? Rol { get; set; }
        public virtual Permiso? Permiso { get; set; }
    }
}
