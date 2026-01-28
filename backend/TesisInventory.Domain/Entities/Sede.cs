using System;
using System.Collections.Generic;

namespace TesisInventory.Domain.Entities
{
    public class Sede
    {
        public int IdSede { get; set; }
        public string NombreSede { get; set; } = string.Empty;

        // Navigation property if needed, though strictly not required for the dropdown
        // public ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
    }
}
