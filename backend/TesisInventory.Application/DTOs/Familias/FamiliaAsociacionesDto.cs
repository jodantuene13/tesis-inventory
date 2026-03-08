using System.Collections.Generic;

namespace TesisInventory.Application.DTOs.Familias
{
    public class FamiliaAsociacionesDto
    {
        public IEnumerable<string> Productos { get; set; } = new List<string>();
        public IEnumerable<string> Atributos { get; set; } = new List<string>();
    }
}
