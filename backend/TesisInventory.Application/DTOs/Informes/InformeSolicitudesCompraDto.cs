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
        public int PendientesHasta5Dias   { get; set; }   // <= 5 días (recientes)
        public int Pendientes6a10Dias     { get; set; }   // > 5 y <= 10 días
        public int Pendientes11a30Dias    { get; set; }   // > 10 y <= 30 días
        public int PendientesMasDe30Dias  { get; set; }   // > 30 días (críticos)

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
        public string UnidadMedida { get; set; } = "u.";
        public int TotalUnidades { get; set; }
        public int VecesSolicitado { get; set; }
        public int VecesEnAprobadas { get; set; }
    }
}
