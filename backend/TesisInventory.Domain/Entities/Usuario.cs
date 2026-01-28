using System;

namespace TesisInventory.Domain.Entities
{
    public class Usuario
    {
        public int IdUsuario { get; set; }
        public string NombreUsuario { get; set; }
        public string Email { get; set; }
        public string? GoogleId { get; set; } // Nullable in DB, can be null here
        public string? Password { get; set; } // Nullable in DB
        public bool Estado { get; set; }
        public DateTime FechaRegistro { get; set; }
        public int IdRol { get; set; }
        public int IdSede { get; set; }

        public Rol Rol { get; set; }
        // public Sede Sede { get; set; } // Sede entity not created yet, uncomment when ready
    }
}
