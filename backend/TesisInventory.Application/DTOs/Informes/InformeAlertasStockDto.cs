using System;
using System.Collections.Generic;

namespace TesisInventory.Application.DTOs.Informes
{
    public class InformeAlertasStockDto
    {
        public IEnumerable<ProductoAlertaStockDto> BajoStock { get; set; } = new List<ProductoAlertaStockDto>();
        public IEnumerable<ProductoRecurrenciaDto> Recurrencia { get; set; } = new List<ProductoRecurrenciaDto>();
        public IEnumerable<EvolucionSemanalDto> EvolucionSemanal { get; set; } = new List<EvolucionSemanalDto>();
    }
}
