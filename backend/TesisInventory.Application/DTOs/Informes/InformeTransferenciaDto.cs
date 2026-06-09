using System;
using System.Collections.Generic;

namespace TesisInventory.Application.DTOs.Informes
{
    public class InformeTransferenciaDto
    {
        public KpiTransferenciaDto Kpis { get; set; } = new KpiTransferenciaDto();
        public List<TransferenciaHabitualDto> HabitualesPorSede { get; set; } = new List<TransferenciaHabitualDto>();
        public List<TransferenciaHabitualDto> HabitualesPorProducto { get; set; } = new List<TransferenciaHabitualDto>();
        public List<PrestamoPorDiaDto> PrestamosPorDia { get; set; } = new List<PrestamoPorDiaDto>();
        public List<PrestamoActivoDto> PrestamosActivos { get; set; } = new List<PrestamoActivoDto>();
        public List<ProductoRechazoDto> RechazosPorProducto { get; set; } = new List<ProductoRechazoDto>();
        public List<TransferenciaDetalleDto> DetalleMovimientos { get; set; } = new List<TransferenciaDetalleDto>();
    }
}
