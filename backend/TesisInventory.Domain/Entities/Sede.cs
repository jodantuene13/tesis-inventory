using System;
using System.Collections.Generic;

namespace TesisInventory.Domain.Entities
{
    public class Sede
    {
        public int IdSede { get; set; }
        public string NombreSede { get; set; } = string.Empty;
        public string Direccion { get; set; } = string.Empty;

        // Navigation property
        public ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
    }
}
