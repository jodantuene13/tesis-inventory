using System;
using System.Collections.Generic;

namespace TesisInventory.Domain.Entities
{
    public class Rol
    {
        public int IdRol { get; set; }
        public string NombreRol { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;

        // Nuevos campos
        public bool TodasLasSedes { get; set; } = false;
        public bool LimitarOperacionSedePrimaria { get; set; } = false;

        public virtual ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
        public virtual ICollection<RolSede> RolesSedes { get; set; } = new List<RolSede>();
        public virtual ICollection<RolPermiso> RolesPermisos { get; set; } = new List<RolPermiso>();
    }
}
