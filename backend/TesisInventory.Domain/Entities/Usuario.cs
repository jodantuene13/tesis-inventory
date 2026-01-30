using System;

namespace TesisInventory.Domain.Entities
{
    public class Usuario
    {
        public int IdUsuario { get; set; }
        public string NombreUsuario { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? GoogleId { get; set; }
        public string? Password { get; set; }
        public bool Estado { get; set; }
        public DateTime FechaRegistro { get; set; }
        public int IdRol { get; set; }
        public int IdSede { get; set; }

        public virtual Rol? Rol { get; set; }
        public virtual Sede? Sede { get; set; }
    }
}
