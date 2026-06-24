using System;
using System.Collections.Generic;
using TesisInventory.Application.DTOs.Sedes;

namespace TesisInventory.Application.Exceptions
{
    public class SedeDeleteBloqueadaException : Exception
    {
        public List<SedeDeleteBloqueante> Bloqueantes { get; }

        public SedeDeleteBloqueadaException(List<SedeDeleteBloqueante> bloqueantes)
            : base("La sede no puede eliminarse porque tiene datos activos asociados.")
        {
            Bloqueantes = bloqueantes;
        }
    }
}
