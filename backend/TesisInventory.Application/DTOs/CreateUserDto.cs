using System;

namespace TesisInventory.Application.DTOs
{
    public class CreateUserDto
    {
        public string NombreUsuario { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public int IdRol { get; set; }
        public int IdSede { get; set; }
    }
}
