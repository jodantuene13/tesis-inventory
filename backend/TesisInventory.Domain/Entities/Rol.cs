using System;
using System.Collections.Generic;

namespace TesisInventory.Domain.Entities
{
    public class Rol
    {
        public int IdRol { get; set; }
        public string NombreRol { get; set; }
        public string Descripcion { get; set; }

        public ICollection<Usuario> Usuarios { get; set; }
    }
}
