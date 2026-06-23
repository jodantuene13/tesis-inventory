import {
  Component, OnInit, OnDestroy, AfterViewInit,
  ElementRef, ViewChild, ChangeDetectorRef
} from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Chart, registerables } from 'chart.js';
import { InformesService, ProductoAlertaStockDto, ProductoRecurrenciaDto } from '../../../services/informes.service';
import { FamiliaService } from '../../../services/familia.service';
import { Familia } from '../../../models/familia.model';

Chart.register(...registerables);

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
    idSede: null as number | null,
    idFamilia: null as number | null,
    estado: 'Todos',
    topN: 10,
    semanas: 5,
  };

  sedes = [
    { id: null, nombre: 'Todas' },
    { id: 1,    nombre: 'Centro' },
    { id: 2,    nombre: 'Campus' },
    { id: 3,    nombre: 'General Paz' }
  ];

  familias: Familia[] = [];
  topNOptions = [5, 10, 20, 50];

  // ── Tabs ─────────────────────────────────────────────────────────────────
  activeTab: 'bajoStock' | 'recurrencia' = 'bajoStock';

  // ── Ver más ───────────────────────────────────────────────────────────────
  verMasBajoStock = false;
  verMasRecurrencia = false;

  // ── Ordenamiento ──────────────────────────────────────────────────────────
  sortColumnBS: keyof ProductoAlertaStockDto = 'stockActual';
  sortDirBS: 'asc' | 'desc' = 'asc';
  sortColumnRec: keyof ProductoRecurrenciaDto = 'cantidadAlertas';
  sortDirRec: 'asc' | 'desc' = 'desc';

  // ── Búsqueda ──────────────────────────────────────────────────────────────
  busquedaBS = '';
  busquedaRec = '';

  // ── Estado ────────────────────────────────────────────────────────────────
  cargando = false;
  error: string | null = null;

  // ── Datos reales ──────────────────────────────────────────────────────────
  datosBajoStock: ProductoAlertaStockDto[] = [];
  datosRecurrencia: ProductoRecurrenciaDto[] = [];
  evolucionSemanal: { semana: string; alertas: number }[] = [];

  // ── Charts ───────────────────────────────────────────────────────────────
  private chartBajoStock: Chart | null = null;
  private chartRecurrencia: Chart | null = null;
  private chartEvolucion: Chart | null = null;
  private chartsInitialized = false;

  constructor(
    private cdr: ChangeDetectorRef,
    private informesService: InformesService,
    private familiaService: FamiliaService
  ) {}

  ngOnInit(): void {
    this.cargarFamilias();
    this.cargarDatos();
  }

  ngAfterViewInit(): void {
    this.chartsInitialized = true;
  }

  ngOnDestroy(): void {
    this.chartBajoStock?.destroy();
    this.chartRecurrencia?.destroy();
    this.chartEvolucion?.destroy();
  }

  // ── Carga de familias ─────────────────────────────────────────────────────
  private cargarFamilias(): void {
    this.familiaService.getAll().subscribe({
      next: (familias) => { this.familias = familias; },
      error: () => { this.familias = []; }
    });
  }

  // ── Carga de datos reales ─────────────────────────────────────────────────
  cargarDatos(): void {
    this.cargando = true;
    this.error = null;

    this.informesService.getAlertasStock(
      this.filtros.idSede ?? undefined,
      this.filtros.idFamilia ?? undefined,
      this.filtros.semanas
    ).subscribe({
      next: (data) => {
        let bs = [...data.bajoStock];
        let rec = [...data.recurrencia];

        // Filtro de estado (Alta criticidad → "Crítico", resto → "Normal")
        if (this.filtros.estado === 'Crítico') {
          bs  = bs.filter(p => p.criticidad === 'Alta');
          rec = rec.filter(p => p.criticidad === 'Alta');
        } else if (this.filtros.estado === 'Normal') {
          bs  = bs.filter(p => p.criticidad !== 'Alta');
          rec = rec.filter(p => p.criticidad !== 'Alta');
        }

        this.datosBajoStock  = bs;
        this.datosRecurrencia = rec;
        this.evolucionSemanal = data.evolucionSemanal;
        this.verMasBajoStock  = false;
        this.verMasRecurrencia = false;
        this.cargando = false;
        this.cdr.detectChanges();

        if (this.chartsInitialized) {
          setTimeout(() => this.renderCharts(), 100);
        }
      },
      error: (err) => {
        this.error = 'No se pudieron cargar los datos del informe. Verificá la conexión con el servidor.';
        this.cargando = false;
        this.cdr.detectChanges();
      }
    });
  }

  aplicarFiltros(): void {
    this.cargarDatos();
  }

  limpiarFiltros(): void {
    this.filtros = { idSede: null, idFamilia: null, estado: 'Todos', topN: 10, semanas: 5 };
    this.busquedaBS = '';
    this.busquedaRec = '';
    this.cargarDatos();
  }

  // ── KPI Calculados ────────────────────────────────────────────────────────

  get kpiProductosEnAlerta(): number {
    return this.datosBajoStock.length;
  }

  get kpiProductoMenorStock(): string {
    if (!this.datosBajoStock.length) return '—';
    return [...this.datosBajoStock].sort((a, b) => a.stockActual - b.stockActual)[0].producto;
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

  get sedeSeleccionadaNombre(): string {
    if (this.filtros.idSede === null) return 'Todas';
    return this.sedes.find(s => s.id === this.filtros.idSede)?.nombre ?? 'Todas';
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

  get datosBSFiltrados(): ProductoAlertaStockDto[] {
    let data = this.datosBajoStock;
    if (this.busquedaBS) {
      const q = this.busquedaBS.toLowerCase();
      data = data.filter(p => p.producto.toLowerCase().includes(q));
    }
    data = [...data].sort((a, b) => {
      const va = a[this.sortColumnBS];
      const vb = b[this.sortColumnBS];
      const cmp = typeof va === 'number' ? (va as number) - (vb as number) : String(va).localeCompare(String(vb));
      return this.sortDirBS === 'asc' ? cmp : -cmp;
    });
    return this.verMasBajoStock ? data : data.slice(0, this.filtros.topN);
  }

  get datosRecFiltrados(): ProductoRecurrenciaDto[] {
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

  sortBS(col: keyof ProductoAlertaStockDto): void {
    if (this.sortColumnBS === col) this.sortDirBS = this.sortDirBS === 'asc' ? 'desc' : 'asc';
    else { this.sortColumnBS = col; this.sortDirBS = 'asc'; }
  }

  sortRec(col: keyof ProductoRecurrenciaDto): void {
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
              label: (ctx: any) => ` ${ctx.dataset.label}: ${ctx.parsed.x} u.`
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
          tooltip: { callbacks: { label: (ctx: any) => ` ${ctx.parsed.x} alertas` } }
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
        labels: this.evolucionSemanal.map(s => s.semana),
        datasets: [{
          label: 'Alertas por semana',
          data: this.evolucionSemanal.map(s => s.alertas),
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
          tooltip: { callbacks: { label: (ctx: any) => ` ${ctx.parsed.y} alertas` } }
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
      csv = 'Producto,Familia,Sede,Unidad,Stock Actual,Stock Mínimo,Diferencia,Días en Alerta,Última Alerta,Criticidad\n';
      this.datosBSFiltrados.forEach(p => {
        csv += `"${p.producto}","${p.familia}","${p.sede}","${p.unidadMedida}",${p.stockActual},${p.stockMinimo},${p.diferencia},${p.diasEnAlerta},"${p.ultimaAlerta}","${p.criticidad}"\n`;
      });
    } else {
      csv = 'Producto,Familia,Sede,Cantidad Alertas,Días Acumulados,Unidad,Stock Actual,Stock Mínimo,Última Alerta,Estado Actual,Criticidad\n';
      this.datosRecFiltrados.forEach(p => {
        csv += `"${p.producto}","${p.familia}","${p.sede}",${p.cantidadAlertas},${p.diasAcumulados},"${p.unidadMedida}",${p.stockActual},${p.stockMinimo},"${p.ultimaAlerta}","${p.estadoActual}","${p.criticidad}"\n`;
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

  trackById(index: number, item: any): number { return item.idProducto ?? index; }
}
