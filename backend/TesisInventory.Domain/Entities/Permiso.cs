using System.Collections.Generic;

namespace TesisInventory.Domain.Entities
{
    public class Permiso
    {
        public int IdPermiso { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Modulo { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;

        public virtual ICollection<RolPermiso> RolesPermisos { get; set; } = new List<RolPermiso>();
    }
}
