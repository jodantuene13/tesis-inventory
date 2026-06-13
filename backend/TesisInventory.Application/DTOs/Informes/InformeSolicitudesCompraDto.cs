using System;
using System.Collections.Generic;

namespace TesisInventory.Application.DTOs.Informes
{
    public class InformeSolicitudesCompraDto
    {
        // ── KPIs globales ────────────────────────────────────────────────────
        public int TotalSolicitudes { get; set; }
        public int TotalAprobadas { get; set; }
        public int TotalRechazadas { get; set; }
        public int TotalPendientes { get; set; }
        public double TiempoPromedioDecisionDias { get; set; }
        public double TiempoPromedioStockDias { get; set; }
        public double PorcentajeCumplimientoTotal { get; set; }

        // ── Tab 1: Por usuario / Por sede ────────────────────────────────────
        public List<SolicitudesPorEntidadDto> PorUsuario { get; set; } = new();
        public List<SolicitudesPorEntidadDto> PorSede { get; set; } = new();

        // ── Tab 2: Pendientes ────────────────────────────────────────────────
        public List<SolicitudPendienteDto> Pendientes { get; set; } = new();
        public int PendientesHasta5Dias { get; set; }
        public int PendientesHasta10Dias { get; set; }
        public int PendientesHasta30Dias { get; set; }

        // ── Tab 3: Productos más solicitados ─────────────────────────────────
        public List<ProductoSolicitadoDto> ProductosMasSolicitados { get; set; } = new();
    }

    public class SolicitudesPorEntidadDto
    {
        public string Nombre { get; set; } = string.Empty;
        public int Total { get; set; }
        public int Aprobadas { get; set; }
        public int Rechazadas { get; set; }
        public int Pendientes { get; set; }
        public int CumplimientoTotal { get; set; }
        public int CumplimientoParcial { get; set; }
        public int NoConcretadas { get; set; }
        public double TiempoPromedioDecisionDias { get; set; }
        public double TiempoPromedioStockDias { get; set; }
    }

    public class SolicitudPendienteDto
    {
        public int IdSolicitudCompra { get; set; }
        public string Usuario { get; set; } = string.Empty;
        public string Sede { get; set; } = string.Empty;
        public DateTime FechaSolicitud { get; set; }
        public int DiasEsperando { get; set; }
        public string Productos { get; set; } = string.Empty;
        public string MotivoSolicitud { get; set; } = string.Empty;
    }

    public class ProductoSolicitadoDto
    {
        public int IdProducto { get; set; }
        public string Producto { get; set; } = string.Empty;
        public string Sku { get; set; } = string.Empty;
        public string Familia { get; set; } = string.Empty;
        public int TotalUnidades { get; set; }
        public int VecesSolicitado { get; set; }
        public int VecesEnAprobadas { get; set; }
    }
}
