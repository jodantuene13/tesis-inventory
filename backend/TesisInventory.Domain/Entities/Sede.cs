using System;
using System.Collections.Generic;

namespace TesisInventory.Domain.Entities
{
    public class Sede
    {
        public int IdSede { get; set; }
        public string NombreSede { get; set; } = string.Empty;
        public string CodigoSede { get; set; } = string.Empty;
        public string Direccion { get; set; } = string.Empty;
        public bool Activo { get; set; } = true;

        // Navigation property
        public ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
        public ICollection<Stock> Stocks { get; set; } = new List<Stock>();
        public ICollection<Movimiento> Movimientos { get; set; } = new List<Movimiento>();
        public ICollection<Transferencia> TransferenciasOrigen { get; set; } = new List<Transferencia>();
        public ICollection<Transferencia> TransferenciasDestino { get; set; } = new List<Transferencia>();
        public ICollection<OperacionStock> OperacionesStock { get; set; } = new List<OperacionStock>();
    }
}
