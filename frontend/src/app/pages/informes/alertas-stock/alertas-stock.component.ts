import { Component, OnInit, OnDestroy, AfterViewInit, ElementRef, ViewChild, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Chart, registerables } from 'chart.js';

Chart.register(...registerables);

// ─── Interfaces ───────────────────────────────────────────────────────────────

interface ProductoAlertaStock {
  id: number;
  producto: string;
  familia: string;
  sede: string;
  stockActual: number;
  stockMinimo: number;
  diferencia: number;
  diasEnAlerta: number;
  ultimaAlerta: string;
  criticidad: 'Alta' | 'Media' | 'Baja';
}

interface ProductoRecurrencia {
  id: number;
  producto: string;
  familia: string;
  sede: string;
  cantidadAlertas: number;
  diasAcumulados: number;
  stockActual: number;
  stockMinimo: number;
  ultimaAlerta: string;
  estadoActual: string;
  criticidad: 'Alta' | 'Media' | 'Baja';
}

// ─── Datos Mockeados ──────────────────────────────────────────────────────────

const MOCK_ALERTAS_STOCK: ProductoAlertaStock[] = [
  { id: 1, producto: 'Cable UTP Cat6',     familia: 'Cables y conectividad', sede: 'Campus',     stockActual: 2,  stockMinimo: 20, diferencia: -18, diasEnAlerta: 68, ultimaAlerta: '20/05/2025', criticidad: 'Alta' },
  { id: 2, producto: 'Lámpara LED 18W',    familia: 'Iluminación',           sede: 'Centro',     stockActual: 5,  stockMinimo: 25, diferencia: -20, diasEnAlerta: 55, ultimaAlerta: '19/05/2025', criticidad: 'Alta' },
  { id: 3, producto: 'Tóner HP 83A',       familia: 'Impresión',             sede: 'General Paz',stockActual: 1,  stockMinimo: 8,  diferencia: -7,  diasEnAlerta: 49, ultimaAlerta: '20/05/2025', criticidad: 'Alta' },
  { id: 4, producto: 'Pintura blanca 20L', familia: 'Pinturas',              sede: 'Campus',     stockActual: 3,  stockMinimo: 15, diferencia: -12, diasEnAlerta: 44, ultimaAlerta: '18/05/2025', criticidad: 'Media' },
  { id: 5, producto: 'Llave térmica 20A',  familia: 'Eléctrico',             sede: 'Centro',     stockActual: 0,  stockMinimo: 8,  diferencia: -8,  diasEnAlerta: 39, ultimaAlerta: '20/05/2025', criticidad: 'Alta' },
  { id: 6, producto: 'Cinta aisladora',    familia: 'Eléctrico',             sede: 'General Paz',stockActual: 7,  stockMinimo: 20, diferencia: -13, diasEnAlerta: 34, ultimaAlerta: '17/05/2025', criticidad: 'Media' },
  { id: 7, producto: 'Caño PVC 20 mm',    familia: 'Sanitarios',            sede: 'Campus',     stockActual: 6,  stockMinimo: 18, diferencia: -12, diasEnAlerta: 28, ultimaAlerta: '16/05/2025', criticidad: 'Baja' },
  { id: 8, producto: 'Tornillo 6x40',      familia: 'Ferretería',            sede: 'Centro',     stockActual: 12, stockMinimo: 30, diferencia: -18, diasEnAlerta: 22, ultimaAlerta: '14/05/2025', criticidad: 'Baja' },
  { id: 9, producto: 'Disyuntor 40A',      familia: 'Eléctrico',             sede: 'General Paz',stockActual: 1,  stockMinimo: 6,  diferencia: -5,  diasEnAlerta: 18, ultimaAlerta: '13/05/2025', criticidad: 'Media' },
  { id: 10, producto: 'Router Wi-Fi',      familia: 'Redes',                 sede: 'Campus',     stockActual: 2,  stockMinimo: 10, diferencia: -8,  diasEnAlerta: 15, ultimaAlerta: '12/05/2025', criticidad: 'Baja' },
];

const MOCK_RECURRENCIA: ProductoRecurrencia[] = [
  { id: 1, producto: 'Cable UTP Cat6',     familia: 'Cables y conectividad', sede: 'Campus',     cantidadAlertas: 4,  diasAcumulados: 68, stockActual: 2,  stockMinimo: 20, ultimaAlerta: '20/05/2025', estadoActual: 'Bajo stock', criticidad: 'Alta' },
  { id: 2, producto: 'Lámpara LED 18W',    familia: 'Iluminación',           sede: 'Centro',     cantidadAlertas: 3,  diasAcumulados: 55, stockActual: 5,  stockMinimo: 25, ultimaAlerta: '19/05/2025', estadoActual: 'Bajo stock', criticidad: 'Media' },
  { id: 3, producto: 'Tóner HP 83A',       familia: 'Impresión',             sede: 'General Paz',cantidadAlertas: 3,  diasAcumulados: 49, stockActual: 1,  stockMinimo: 8,  ultimaAlerta: '20/05/2025', estadoActual: 'Crítico',    criticidad: 'Alta' },
  { id: 4, producto: 'Pintura blanca 20L', familia: 'Pinturas',              sede: 'Campus',     cantidadAlertas: 3,  diasAcumulados: 44, stockActual: 3,  stockMinimo: 15, ultimaAlerta: '18/05/2025', estadoActual: 'Bajo stock', criticidad: 'Media' },
  { id: 5, producto: 'Llave térmica 20A',  familia: 'Eléctrico',             sede: 'Centro',     cantidadAlertas: 6,  diasAcumulados: 39, stockActual: 0,  stockMinimo: 8,  ultimaAlerta: '20/05/2025', estadoActual: 'Crítico',    criticidad: 'Alta' },
  { id: 6, producto: 'Cinta aisladora',    familia: 'Eléctrico',             sede: 'General Paz',cantidadAlertas: 3,  diasAcumulados: 34, stockActual: 7,  stockMinimo: 20, ultimaAlerta: '17/05/2025', estadoActual: 'Bajo stock', criticidad: 'Media' },
  { id: 7, producto: 'Caño PVC 20 mm',    familia: 'Sanitarios',            sede: 'Campus',     cantidadAlertas: 2,  diasAcumulados: 28, stockActual: 6,  stockMinimo: 18, ultimaAlerta: '16/05/2025', estadoActual: 'Bajo stock', criticidad: 'Baja' },
  { id: 8, producto: 'Tornillo 6x40',      familia: 'Ferretería',            sede: 'Centro',     cantidadAlertas: 2,  diasAcumulados: 22, stockActual: 12, stockMinimo: 30, ultimaAlerta: '14/05/2025', estadoActual: 'Bajo stock', criticidad: 'Baja' },
  { id: 9, producto: 'Disyuntor 40A',      familia: 'Eléctrico',             sede: 'General Paz',cantidadAlertas: 2,  diasAcumulados: 18, stockActual: 1,  stockMinimo: 6,  ultimaAlerta: '13/05/2025', estadoActual: 'Bajo stock', criticidad: 'Media' },
  { id: 10, producto: 'Router Wi-Fi',      familia: 'Redes',                 sede: 'Campus',     cantidadAlertas: 1,  diasAcumulados: 15, stockActual: 2,  stockMinimo: 10, ultimaAlerta: '12/05/2025', estadoActual: 'Bajo stock', criticidad: 'Baja' },
];

const SEMANAS_EVOLUCION = [
  { semana: '22 Abr', alertas: 18 },
  { semana: '29 Abr', alertas: 22 },
  { semana: '6 May',  alertas: 31 },
  { semana: '13 May', alertas: 47 },
  { semana: '20 May', alertas: 39 },
];

// ─── Component ────────────────────────────────────────────────────────────────

@Component({
  selector: 'app-alertas-stock',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './alertas-stock.component.html',
  styleUrls: ['./alertas-stock.component.css']
})
export class AlertasStockComponent implements OnInit, AfterViewInit, OnDestroy {

  @ViewChild('chartBajoStock') chartBajoStockRef!: ElementRef<HTMLCanvasElement>;
  @ViewChild('chartRecurrencia') chartRecurrenciaRef!: ElementRef<HTMLCanvasElement>;
  @ViewChild('chartEvolucion') chartEvolucionRef!: ElementRef<HTMLCanvasElement>;

  // ── Filtros ──────────────────────────────────────────────────────────────
  filtros = {
    sede: 'Todas',
    familia: 'Todas',
    periodo: '30d',
    fechaDesde: '',
    fechaHasta: '',
    estado: 'Todos',
    topN: 10,
  };

  sedes = ['Todas', 'Centro', 'Campus', 'General Paz'];
  familias = ['Todas', 'Cables y conectividad', 'Iluminación', 'Impresión', 'Pinturas', 'Eléctrico', 'Sanitarios', 'Ferretería', 'Redes'];
  topNOptions = [5, 10, 20, 50];

  // ── Tabs ─────────────────────────────────────────────────────────────────
  activeTab: 'bajoStock' | 'recurrencia' = 'bajoStock';

  // ── Ver más ───────────────────────────────────────────────────────────────
  verMasBajoStock = false;
  verMasRecurrencia = false;

  // ── Ordenamiento tablas ───────────────────────────────────────────────────
  sortColumnBS: keyof ProductoAlertaStock = 'stockActual';
  sortDirBS: 'asc' | 'desc' = 'asc';
  sortColumnRec: keyof ProductoRecurrencia = 'cantidadAlertas';
  sortDirRec: 'asc' | 'desc' = 'desc';

  // ── Búsqueda ──────────────────────────────────────────────────────────────
  busquedaBS = '';
  busquedaRec = '';

  // ── Datos filtrados ───────────────────────────────────────────────────────
  datosBajoStock: ProductoAlertaStock[] = [];
  datosRecurrencia: ProductoRecurrencia[] = [];

  // ── Charts ───────────────────────────────────────────────────────────────
  private chartBajoStock: Chart | null = null;
  private chartRecurrencia: Chart | null = null;
  private chartEvolucion: Chart | null = null;

  private chartsInitialized = false;

  constructor(private cdr: ChangeDetectorRef) {}

  ngOnInit(): void {
    this.aplicarFiltros();
  }

  ngAfterViewInit(): void {
    this.chartsInitialized = true;
    setTimeout(() => this.renderCharts(), 100);
  }

  ngOnDestroy(): void {
    this.chartBajoStock?.destroy();
    this.chartRecurrencia?.destroy();
    this.chartEvolucion?.destroy();
  }

  // ── Filtrado ──────────────────────────────────────────────────────────────

  aplicarFiltros(): void {
    let bs = [...MOCK_ALERTAS_STOCK];
    let rec = [...MOCK_RECURRENCIA];

    if (this.filtros.sede !== 'Todas') {
      bs  = bs.filter(p => p.sede === this.filtros.sede);
      rec = rec.filter(p => p.sede === this.filtros.sede);
    }
    if (this.filtros.familia !== 'Todas') {
      bs  = bs.filter(p => p.familia === this.filtros.familia);
      rec = rec.filter(p => p.familia === this.filtros.familia);
    }
    if (this.filtros.estado !== 'Todos') {
      const esAltaMedia = this.filtros.estado === 'Crítico';
      bs  = bs.filter(p => esAltaMedia ? p.criticidad === 'Alta' : p.criticidad !== 'Alta');
      rec = rec.filter(p => esAltaMedia ? p.criticidad === 'Alta' : p.criticidad !== 'Alta');
    }

    // Ordenar
    bs.sort((a, b) => a.stockActual - b.stockActual);
    rec.sort((a, b) => b.cantidadAlertas - a.cantidadAlertas);

    this.datosBajoStock = bs;
    this.datosRecurrencia = rec;

    this.verMasBajoStock = false;
    this.verMasRecurrencia = false;

    if (this.chartsInitialized) {
      setTimeout(() => this.renderCharts(), 50);
    }
  }

  limpiarFiltros(): void {
    this.filtros = { sede: 'Todas', familia: 'Todas', periodo: '30d', fechaDesde: '', fechaHasta: '', estado: 'Todos', topN: 10 };
    this.busquedaBS = '';
    this.busquedaRec = '';
    this.aplicarFiltros();
  }

  // ── KPI Calculados ────────────────────────────────────────────────────────

  get kpiProductosEnAlerta(): number {
    return this.datosBajoStock.length;
  }

  get kpiProductoMenorStock(): string {
    if (!this.datosBajoStock.length) return '—';
    return this.datosBajoStock[0].producto;
  }

  get kpiPromedioUnidades(): string {
    if (!this.datosBajoStock.length) return '0';
    const avg = this.datosBajoStock.reduce((s, p) => s + p.stockActual, 0) / this.datosBajoStock.length;
    return avg.toFixed(1);
  }

  get kpiDiasPromedioAlerta(): string {
    if (!this.datosBajoStock.length) return '0';
    const avg = this.datosBajoStock.reduce((s, p) => s + p.diasEnAlerta, 0) / this.datosBajoStock.length;
    return avg.toFixed(1);
  }

  get kpiMayorRecurrencia(): string {
    if (!this.datosRecurrencia.length) return '—';
    return this.datosRecurrencia[0].producto;
  }

  get kpiTotalAlertas(): number {
    return this.datosRecurrencia.reduce((s, p) => s + p.cantidadAlertas, 0);
  }

  get kpiPromedioAlertas(): string {
    if (!this.datosRecurrencia.length) return '0';
    const avg = this.datosRecurrencia.reduce((s, p) => s + p.cantidadAlertas, 0) / this.datosRecurrencia.length;
    return avg.toFixed(1);
  }

  get kpiFamiliaMasAlertas(): string {
    if (!this.datosRecurrencia.length) return '—';
    const map = new Map<string, number>();
    this.datosRecurrencia.forEach(p => map.set(p.familia, (map.get(p.familia) || 0) + p.cantidadAlertas));
    let max = 0, fam = '—';
    map.forEach((v, k) => { if (v > max) { max = v; fam = k; } });
    return fam;
  }

  // ── Datos visibles en tabla ───────────────────────────────────────────────

  get datosBSFiltrados(): ProductoAlertaStock[] {
    let data = this.datosBajoStock;
    if (this.busquedaBS) {
      const q = this.busquedaBS.toLowerCase();
      data = data.filter(p => p.producto.toLowerCase().includes(q));
    }
    // Ordenamiento de columnas
    data = [...data].sort((a, b) => {
      const va = a[this.sortColumnBS];
      const vb = b[this.sortColumnBS];
      const cmp = typeof va === 'number' ? (va as number) - (vb as number) : String(va).localeCompare(String(vb));
      return this.sortDirBS === 'asc' ? cmp : -cmp;
    });
    return this.verMasBajoStock ? data : data.slice(0, this.filtros.topN);
  }

  get datosRecFiltrados(): ProductoRecurrencia[] {
    let data = this.datosRecurrencia;
    if (this.busquedaRec) {
      const q = this.busquedaRec.toLowerCase();
      data = data.filter(p => p.producto.toLowerCase().includes(q));
    }
    data = [...data].sort((a, b) => {
      const va = a[this.sortColumnRec];
      const vb = b[this.sortColumnRec];
      const cmp = typeof va === 'number' ? (va as number) - (vb as number) : String(va).localeCompare(String(vb));
      return this.sortDirRec === 'asc' ? cmp : -cmp;
    });
    return this.verMasRecurrencia ? data : data.slice(0, this.filtros.topN);
  }

  // ── Ordenamiento ──────────────────────────────────────────────────────────

  sortBS(col: keyof ProductoAlertaStock): void {
    if (this.sortColumnBS === col) this.sortDirBS = this.sortDirBS === 'asc' ? 'desc' : 'asc';
    else { this.sortColumnBS = col; this.sortDirBS = 'asc'; }
  }

  sortRec(col: keyof ProductoRecurrencia): void {
    if (this.sortColumnRec === col) this.sortDirRec = this.sortDirRec === 'asc' ? 'desc' : 'asc';
    else { this.sortColumnRec = col; this.sortDirRec = 'asc'; }
  }

  sortIcon(col: string, active: string, dir: string): string {
    if (col !== active) return '↕';
    return dir === 'asc' ? '↑' : '↓';
  }

  // ── Tabs ─────────────────────────────────────────────────────────────────

  setTab(tab: 'bajoStock' | 'recurrencia'): void {
    this.activeTab = tab;
    setTimeout(() => this.renderCharts(), 100);
  }

  // ── Charts ───────────────────────────────────────────────────────────────

  private renderCharts(): void {
    if (this.activeTab === 'bajoStock') {
      this.renderChartBajoStock();
    } else {
      this.renderChartRecurrencia();
      this.renderChartEvolucion();
    }
  }

  private renderChartBajoStock(): void {
    const canvas = this.chartBajoStockRef?.nativeElement;
    if (!canvas) return;

    this.chartBajoStock?.destroy();

    const top = [...this.datosBajoStock]
      .sort((a, b) => a.stockActual - b.stockActual)
      .slice(0, this.filtros.topN);

    const labels = top.map(p => p.producto);
    const values = top.map(p => p.stockActual);
    const mins   = top.map(p => p.stockMinimo);

    const colors = top.map(p =>
      p.criticidad === 'Alta'  ? 'rgba(239,68,68,0.85)'  :
      p.criticidad === 'Media' ? 'rgba(245,158,11,0.85)' :
                                 'rgba(34,197,94,0.85)'
    );

    this.chartBajoStock = new Chart(canvas, {
      type: 'bar',
      data: {
        labels,
        datasets: [
          {
            label: 'Stock actual',
            data: values,
            backgroundColor: colors,
            borderColor: colors.map(c => c.replace('0.85', '1')),
            borderWidth: 1.5,
            borderRadius: 6,
            barThickness: 22,
          },
          {
            label: 'Stock mínimo',
            data: mins,
            backgroundColor: 'rgba(99,102,241,0.12)',
            borderColor: 'rgba(99,102,241,0.6)',
            borderWidth: 1.5,
            borderRadius: 4,
            barThickness: 22,
            type: 'bar',
          }
        ]
      },
      options: {
        indexAxis: 'y',
        responsive: true,
        maintainAspectRatio: false,
        plugins: {
          legend: { position: 'top', labels: { font: { size: 11 }, boxWidth: 12 } },
          tooltip: {
            callbacks: {
              label: ctx => ` ${ctx.dataset.label}: ${ctx.parsed.x} u.`
            }
          }
        },
        scales: {
          x: { beginAtZero: true, grid: { color: 'rgba(0,0,0,0.04)' }, ticks: { font: { size: 11 } } },
          y: { grid: { display: false }, ticks: { font: { size: 11 } } }
        }
      }
    });
  }

  private renderChartRecurrencia(): void {
    const canvas = this.chartRecurrenciaRef?.nativeElement;
    if (!canvas) return;

    this.chartRecurrencia?.destroy();

    const top = [...this.datosRecurrencia]
      .sort((a, b) => b.cantidadAlertas - a.cantidadAlertas)
      .slice(0, this.filtros.topN);

    const labels = top.map(p => p.producto);
    const values = top.map(p => p.cantidadAlertas);

    const colors = top.map(p =>
      p.criticidad === 'Alta'  ? 'rgba(239,68,68,0.85)'  :
      p.criticidad === 'Media' ? 'rgba(245,158,11,0.85)' :
                                 'rgba(34,197,94,0.85)'
    );

    this.chartRecurrencia = new Chart(canvas, {
      type: 'bar',
      data: {
        labels,
        datasets: [{
          label: 'Cantidad de alertas',
          data: values,
          backgroundColor: colors,
          borderColor: colors.map(c => c.replace('0.85', '1')),
          borderWidth: 1.5,
          borderRadius: 6,
          barThickness: 22,
        }]
      },
      options: {
        indexAxis: 'y',
        responsive: true,
        maintainAspectRatio: false,
        plugins: {
          legend: { display: false },
          tooltip: { callbacks: { label: ctx => ` ${ctx.parsed.x} alertas` } }
        },
        scales: {
          x: { beginAtZero: true, ticks: { stepSize: 1, font: { size: 11 } }, grid: { color: 'rgba(0,0,0,0.04)' } },
          y: { grid: { display: false }, ticks: { font: { size: 11 } } }
        }
      }
    });
  }

  private renderChartEvolucion(): void {
    const canvas = this.chartEvolucionRef?.nativeElement;
    if (!canvas) return;

    this.chartEvolucion?.destroy();

    this.chartEvolucion = new Chart(canvas, {
      type: 'line',
      data: {
        labels: SEMANAS_EVOLUCION.map(s => s.semana),
        datasets: [{
          label: 'Alertas por semana',
          data: SEMANAS_EVOLUCION.map(s => s.alertas),
          borderColor: '#6366f1',
          backgroundColor: 'rgba(99,102,241,0.1)',
          borderWidth: 2.5,
          fill: true,
          tension: 0.4,
          pointBackgroundColor: '#6366f1',
          pointBorderColor: '#fff',
          pointBorderWidth: 2,
          pointRadius: 5,
        }]
      },
      options: {
        responsive: true,
        maintainAspectRatio: false,
        plugins: {
          legend: { display: false },
          tooltip: { callbacks: { label: ctx => ` ${ctx.parsed.y} alertas` } }
        },
        scales: {
          y: { beginAtZero: true, grid: { color: 'rgba(0,0,0,0.04)' }, ticks: { font: { size: 11 } } },
          x: { grid: { display: false }, ticks: { font: { size: 11 } } }
        }
      }
    });
  }

  // ── Exportar CSV ──────────────────────────────────────────────────────────

  exportarCSV(): void {
    let csv = '';
    if (this.activeTab === 'bajoStock') {
      csv = 'Producto,Familia,Sede,Stock Actual,Stock Mínimo,Diferencia,Días en Alerta,Última Alerta,Criticidad\n';
      this.datosBSFiltrados.forEach(p => {
        csv += `"${p.producto}","${p.familia}","${p.sede}",${p.stockActual},${p.stockMinimo},${p.diferencia},${p.diasEnAlerta},"${p.ultimaAlerta}","${p.criticidad}"\n`;
      });
    } else {
      csv = 'Producto,Familia,Sede,Cantidad Alertas,Días Acumulados,Stock Actual,Stock Mínimo,Última Alerta,Estado Actual,Criticidad\n';
      this.datosRecFiltrados.forEach(p => {
        csv += `"${p.producto}","${p.familia}","${p.sede}",${p.cantidadAlertas},${p.diasAcumulados},${p.stockActual},${p.stockMinimo},"${p.ultimaAlerta}","${p.estadoActual}","${p.criticidad}"\n`;
      });
    }

    const blob = new Blob(['\ufeff' + csv], { type: 'text/csv;charset=utf-8;' });
    const url = URL.createObjectURL(blob);
    const a = document.createElement('a');
    a.href = url;
    a.download = `informe-${this.activeTab}-${new Date().toISOString().slice(0, 10)}.csv`;
    a.click();
    URL.revokeObjectURL(url);
  }

  // ── Helpers ───────────────────────────────────────────────────────────────

  badgeClass(criticidad: string): string {
    return criticidad === 'Alta'  ? 'badge-crit-alta'  :
           criticidad === 'Media' ? 'badge-crit-media' :
                                    'badge-crit-baja';
  }

  trackById(index: number, item: any): number { return item.id; }
}
