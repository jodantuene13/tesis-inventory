using System;
using System.Collections.Generic;

namespace TesisInventory.Application.DTOs
{
    public class UserDto
    {
        public int IdUsuario { get; set; }
        public string NombreUsuario { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public bool Estado { get; set; }
        public DateTime FechaRegistro { get; set; }
        public int IdRol { get; set; }
        public string NombreRol { get; set; } = string.Empty;
        public int IdSede { get; set; }
        public string NombreSede { get; set; } = string.Empty;
        public bool TodasLasSedes { get; set; }
        public bool LimitarOperacionSedePrimaria { get; set; }
        public List<int> SedesPermitidas { get; set; } = new List<int>();
    }
}
