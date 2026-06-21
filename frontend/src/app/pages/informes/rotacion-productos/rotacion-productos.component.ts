import {
  Component, OnInit, OnDestroy, AfterViewInit,
  ElementRef, ViewChild, ChangeDetectorRef
} from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { forkJoin } from 'rxjs';
import { Chart, registerables } from 'chart.js';
import {
  InformesService,
  ProductoRotacionDto,
  ProductoMovimientoDto,
  InformeRotacionDto,
  ProductoInmovilizadoDto,
  FamiliaConsumoDto
} from '../../../services/informes.service';
import { FamiliaService } from '../../../services/familia.service';
import { Familia } from '../../../models/familia.model';

Chart.register(...registerables);

// ── Tipos de período ──────────────────────────────────────────────────────────
type PeriodoPreset = '30' | '90' | 'custom';

@Component({
  selector: 'app-rotacion-productos',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './rotacion-productos.component.html',
  styleUrls: ['./rotacion-productos.component.css']
})
export class RotacionProductosComponent implements OnInit, AfterViewInit, OnDestroy {

  @ViewChild('chartRotacion')    chartRotacionRef!:    ElementRef<HTMLCanvasElement>;
  @ViewChild('chartIngresos')    chartIngresosRef!:    ElementRef<HTMLCanvasElement>;
  @ViewChild('chartEgresos')     chartEgresosRef!:     ElementRef<HTMLCanvasElement>;
  @ViewChild('chartInmovilizado') chartInmovilizadoRef!: ElementRef<HTMLCanvasElement>;
  @ViewChild('chartFamilias')    chartFamiliasRef!:    ElementRef<HTMLCanvasElement>;

  // ── Filtros ──────────────────────────────────────────────────────────────────
  filtros = {
    idSede:     null as number | null,
    idFamilia:  null as number | null,
    topN:       10,
    periodoPreset: '30' as PeriodoPreset,
    fechaDesde: this.defaultFechaDesde(30),
    fechaHasta: this.today(),
  };

  sedes: { id: number | null; nombre: string }[] = [
    { id: null, nombre: 'Todas' },
  ];

  familias: Familia[] = [];
  topNOptions = [5, 10, 20];

  // ── Tabs ─────────────────────────────────────────────────────────────────────
  activeTab: 'rotacion' | 'ingresos' | 'egresos' | 'inmovilizado' | 'familias' = 'rotacion';

  // ── Ordenamiento ──────────────────────────────────────────────────────────────
  sortColRot:  keyof ProductoRotacionDto   = 'indiceRotacion';
  sortDirRot: 'asc' | 'desc' = 'desc';
  sortColIng:  keyof ProductoMovimientoDto = 'totalUnidades';
  sortDirIng: 'asc' | 'desc' = 'desc';
  sortColEgr:  keyof ProductoMovimientoDto = 'totalUnidades';
  sortDirEgr: 'asc' | 'desc' = 'desc';

  // ── Búsqueda ──────────────────────────────────────────────────────────────────
  busquedaRot = '';
  busquedaIng = '';
  busquedaEgr = '';
  busquedaInm = '';

  // ── Ver más ───────────────────────────────────────────────────────────────────
  verMasIng = false;
  verMasEgr = false;
  verMasInm = false;
  verMasFam = false;

  // ── Estado ────────────────────────────────────────────────────────────────────
  cargando = false;
  error: string | null = null;

  // ── Datos ─────────────────────────────────────────────────────────────────────
  datosRotacion:    ProductoRotacionDto[]    = [];
  datosMayorIngreso: ProductoMovimientoDto[] = [];
  datosMayorEgreso:  ProductoMovimientoDto[] = [];
  datosInmovilizado: ProductoInmovilizadoDto[] = [];
  datosFamilias:    FamiliaConsumoDto[]      = [];
  rotacionPromedio = 0;
  saldoNetoTotal   = 0;
  fechaDesdeLabel  = '';
  fechaHastaLabel  = '';

  // ── Charts ────────────────────────────────────────────────────────────────────
  private chartRotacion:    Chart | null = null;
  private chartIngresos:    Chart | null = null;
  private chartEgresos:     Chart | null = null;
  private chartInmovilizado: Chart | null = null;
  private chartFamilias:    Chart | null = null;
  private chartsReady = false;

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
    this.chartsReady = true;
  }

  ngOnDestroy(): void {
    this.chartRotacion?.destroy();
    this.chartIngresos?.destroy();
    this.chartEgresos?.destroy();
    this.chartInmovilizado?.destroy();
    this.chartFamilias?.destroy();
  }

  // ── Helpers de fecha ─────────────────────────────────────────────────────────

  private today(): string {
    return new Date().toISOString().slice(0, 10);
  }

  private defaultFechaDesde(dias: number): string {
    const d = new Date();
    d.setDate(d.getDate() - dias);
    return d.toISOString().slice(0, 10);
  }

  seleccionarPeriodo(preset: PeriodoPreset): void {
    this.filtros.periodoPreset = preset;
    if (preset === '30') {
      this.filtros.fechaDesde = this.defaultFechaDesde(30);
      this.filtros.fechaHasta = this.today();
      this.aplicarFiltros();
    } else if (preset === '90') {
      this.filtros.fechaDesde = this.defaultFechaDesde(90);
      this.filtros.fechaHasta = this.today();
      this.aplicarFiltros();
    }
    // 'custom': el usuario elige fechas manualmente y hace clic en "Aplicar"
  }

  // ── Carga ─────────────────────────────────────────────────────────────────────

  private cargarFamilias(): void {
    this.familiaService.getAll().subscribe({
      next: (f) => { this.familias = f; },
      error: () => { this.familias = []; }
    });
  }

  cargarDatos(): void {
    this.cargando = true;
    this.error = null;

    const sede     = this.filtros.idSede ?? undefined;
    const familia  = this.filtros.idFamilia ?? undefined;
    const desde    = this.filtros.fechaDesde;
    const hasta    = this.filtros.fechaHasta;

    forkJoin({
      rotacion:     this.informesService.getRotacionProductos(sede, familia, desde, hasta),
      inmovilizado: this.informesService.getStockInmovilizado(sede, familia, desde, hasta),
      familias:     this.informesService.getFamiliasConsumo(sede, familia, desde, hasta)
    }).subscribe({
      next: ({ rotacion, inmovilizado, familias }) => {
        this.datosRotacion     = rotacion.rotacion;
        this.datosMayorIngreso = rotacion.mayorIngreso;
        this.datosMayorEgreso  = rotacion.mayorEgreso;
        this.rotacionPromedio  = rotacion.rotacionPromedio;
        this.saldoNetoTotal    = rotacion.saldoNetoTotal;
        this.fechaDesdeLabel   = rotacion.fechaDesde;
        this.fechaHastaLabel   = rotacion.fechaHasta;
        this.datosInmovilizado = inmovilizado;
        this.datosFamilias     = familias;
        this.verMasIng = false;
        this.verMasEgr = false;
        this.verMasInm = false;
        this.verMasFam = false;
        this.cargando = false;
        this.cdr.detectChanges();

        if (this.chartsReady)
          setTimeout(() => this.renderCharts(), 100);
      },
      error: () => {
        this.error = 'No se pudieron cargar los datos del informe. Verificá la conexión con el servidor.';
        this.cargando = false;
        this.cdr.detectChanges();
      }
    });
  }

  aplicarFiltros(): void { this.cargarDatos(); }

  limpiarFiltros(): void {
    this.filtros = {
      idSede: null, idFamilia: null, topN: 10,
      periodoPreset: '30',
      fechaDesde: this.defaultFechaDesde(30),
      fechaHasta: this.today()
    };
    this.busquedaRot = ''; this.busquedaIng = ''; this.busquedaEgr = ''; this.busquedaInm = '';
    this.cargarDatos();
  }

  // ── KPIs ─────────────────────────────────────────────────────────────────────

  get kpiMayorRotacion(): string {
    return this.datosRotacion.length ? this.datosRotacion[0].producto : '—';
  }
  get kpiMayorRotacionIndice(): number {
    return this.datosRotacion.length ? this.datosRotacion[0].indiceRotacion : 0;
  }
  get kpiMayorIngreso(): string {
    return this.datosMayorIngreso.length ? this.datosMayorIngreso[0].producto : '—';
  }
  get kpiMayorIngresoUnidades(): number {
    return this.datosMayorIngreso.length ? this.datosMayorIngreso[0].totalUnidades : 0;
  }
  get kpiMayorEgreso(): string {
    return this.datosMayorEgreso.length ? this.datosMayorEgreso[0].producto : '—';
  }
  get kpiMayorEgresoUnidades(): number {
    return this.datosMayorEgreso.length ? this.datosMayorEgreso[0].totalUnidades : 0;
  }

  get sedeSeleccionadaNombre(): string {
    if (!this.filtros.idSede) return 'Todas';
    return this.sedes.find(s => s.id === this.filtros.idSede)?.nombre ?? 'Todas';
  }

  saldoNetoClass(): string {
    return this.saldoNetoTotal >= 0 ? 'valor-positivo' : 'valor-negativo';
  }

  // ── Datos filtrados ───────────────────────────────────────────────────────────

  get datosRotFiltrados(): ProductoRotacionDto[] {
    let data = this.datosRotacion;
    if (this.busquedaRot) {
      const q = this.busquedaRot.toLowerCase();
      data = data.filter(p => p.producto.toLowerCase().includes(q));
    }
    data = [...data].sort((a, b) => {
      const va = a[this.sortColRot], vb = b[this.sortColRot];
      const cmp = typeof va === 'number' ? (va as number) - (vb as number) : String(va).localeCompare(String(vb));
      return this.sortDirRot === 'asc' ? cmp : -cmp;
    });
    return data.slice(0, this.filtros.topN);
  }

  get datosIngFiltrados(): ProductoMovimientoDto[] {
    let data = this.datosMayorIngreso;
    if (this.busquedaIng) {
      const q = this.busquedaIng.toLowerCase();
      data = data.filter(p => p.producto.toLowerCase().includes(q));
    }
    data = [...data].sort((a, b) => {
      const va = a[this.sortColIng], vb = b[this.sortColIng];
      const cmp = typeof va === 'number' ? (va as number) - (vb as number) : String(va).localeCompare(String(vb));
      return this.sortDirIng === 'asc' ? cmp : -cmp;
    });
    return this.verMasIng ? data : data.slice(0, 20);
  }

  get datosEgrFiltrados(): ProductoMovimientoDto[] {
    let data = this.datosMayorEgreso;
    if (this.busquedaEgr) {
      const q = this.busquedaEgr.toLowerCase();
      data = data.filter(p => p.producto.toLowerCase().includes(q));
    }
    data = [...data].sort((a, b) => {
      const va = a[this.sortColEgr], vb = b[this.sortColEgr];
      const cmp = typeof va === 'number' ? (va as number) - (vb as number) : String(va).localeCompare(String(vb));
      return this.sortDirEgr === 'asc' ? cmp : -cmp;
    });
    return this.verMasEgr ? data : data.slice(0, 20);
  }

  get datosInmFiltrados(): ProductoInmovilizadoDto[] {
    let data = this.datosInmovilizado;
    if (this.busquedaInm) {
      const q = this.busquedaInm.toLowerCase();
      data = data.filter(p => p.producto.toLowerCase().includes(q) || p.familia.toLowerCase().includes(q));
    }
    return this.verMasInm ? data : data.slice(0, 20);
  }

  get datosFamiliasFiltrados(): FamiliaConsumoDto[] {
    return this.verMasFam ? this.datosFamilias : this.datosFamilias.slice(0, 10);
  }

  // ── Ordenamiento ──────────────────────────────────────────────────────────────

  sortRot(col: keyof ProductoRotacionDto): void {
    if (this.sortColRot === col) this.sortDirRot = this.sortDirRot === 'asc' ? 'desc' : 'asc';
    else { this.sortColRot = col; this.sortDirRot = 'desc'; }
  }
  sortIng(col: keyof ProductoMovimientoDto): void {
    if (this.sortColIng === col) this.sortDirIng = this.sortDirIng === 'asc' ? 'desc' : 'asc';
    else { this.sortColIng = col; this.sortDirIng = 'desc'; }
  }
  sortEgr(col: keyof ProductoMovimientoDto): void {
    if (this.sortColEgr === col) this.sortDirEgr = this.sortDirEgr === 'asc' ? 'desc' : 'asc';
    else { this.sortColEgr = col; this.sortDirEgr = 'desc'; }
  }

  sortIcon(col: string, active: string, dir: string): string {
    if (col !== active) return '↕';
    return dir === 'asc' ? '↑' : '↓';
  }

  // ── Tabs ─────────────────────────────────────────────────────────────────────

  setTab(tab: 'rotacion' | 'ingresos' | 'egresos' | 'inmovilizado' | 'familias'): void {
    this.activeTab = tab;
    setTimeout(() => this.renderCharts(), 100);
  }

  // ── Charts ────────────────────────────────────────────────────────────────────

  private renderCharts(): void {
    if (this.activeTab === 'rotacion')    this.renderChartRotacion();
    if (this.activeTab === 'ingresos')    this.renderChartIngresos();
    if (this.activeTab === 'egresos')     this.renderChartEgresos();
    if (this.activeTab === 'inmovilizado') this.renderChartInmovilizado();
    if (this.activeTab === 'familias')    this.renderChartFamilias();
  }

  private renderChartRotacion(): void {
    const canvas = this.chartRotacionRef?.nativeElement;
    if (!canvas) return;
    this.chartRotacion?.destroy();

    const top = this.datosRotFiltrados.slice(0, this.filtros.topN);
    const colors = top.map(p =>
      p.tendencia === 'Alta'  ? 'rgba(99,102,241,0.85)'  :
      p.tendencia === 'Media' ? 'rgba(139,92,246,0.75)'  :
                                'rgba(167,139,250,0.6)'
    );

    this.chartRotacion = new Chart(canvas, {
      type: 'bar',
      data: {
        labels: top.map(p => p.producto),
        datasets: [{
          label: 'Índice de rotación',
          data: top.map(p => p.indiceRotacion),
          backgroundColor: colors,
          borderColor: colors.map(c => c.replace('0.85', '1').replace('0.75', '1').replace('0.6', '1')),
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
          tooltip: { callbacks: { label: (ctx: any) => ` Rotación: ${ctx.parsed.x} veces` } }
        },
        scales: {
          x: { beginAtZero: true, grid: { color: 'rgba(0,0,0,0.04)' }, ticks: { font: { size: 11 } } },
          y: { grid: { display: false }, ticks: { font: { size: 11 } } }
        }
      }
    });
  }

  private renderChartIngresos(): void {
    const canvas = this.chartIngresosRef?.nativeElement;
    if (!canvas) return;
    this.chartIngresos?.destroy();

    const top = this.datosMayorIngreso.slice(0, this.filtros.topN);

    this.chartIngresos = new Chart(canvas, {
      type: 'bar',
      data: {
        labels: top.map(p => p.producto),
        datasets: [{
          label: 'Unidades ingresadas',
          data: top.map(p => p.totalUnidades),
          backgroundColor: 'rgba(16,185,129,0.8)',
          borderColor: 'rgba(16,185,129,1)',
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
          tooltip: { callbacks: { label: (ctx: any) => ` ${ctx.parsed.x} u. ingresadas` } }
        },
        scales: {
          x: { beginAtZero: true, grid: { color: 'rgba(0,0,0,0.04)' }, ticks: { font: { size: 11 } } },
          y: { grid: { display: false }, ticks: { font: { size: 11 } } }
        }
      }
    });
  }

  private renderChartEgresos(): void {
    const canvas = this.chartEgresosRef?.nativeElement;
    if (!canvas) return;
    this.chartEgresos?.destroy();

    const top = this.datosMayorEgreso.slice(0, this.filtros.topN);

    this.chartEgresos = new Chart(canvas, {
      type: 'bar',
      data: {
        labels: top.map(p => p.producto),
        datasets: [{
          label: 'Unidades egresadas',
          data: top.map(p => p.totalUnidades),
          backgroundColor: 'rgba(245,158,11,0.8)',
          borderColor: 'rgba(245,158,11,1)',
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
          tooltip: { callbacks: { label: (ctx: any) => ` ${ctx.parsed.x} u. egresadas` } }
        },
        scales: {
          x: { beginAtZero: true, grid: { color: 'rgba(0,0,0,0.04)' }, ticks: { font: { size: 11 } } },
          y: { grid: { display: false }, ticks: { font: { size: 11 } } }
        }
      }
    });
  }

  private renderChartInmovilizado(): void {
    const canvas = this.chartInmovilizadoRef?.nativeElement;
    if (!canvas) return;
    this.chartInmovilizado?.destroy();

    const top = this.datosInmFiltrados.slice(0, this.filtros.topN);

    this.chartInmovilizado = new Chart(canvas, {
      type: 'bar',
      data: {
        labels: top.map(p => p.producto),
        datasets: [{
          label: 'Stock inmovilizado',
          data: top.map(p => p.stockActual),
          backgroundColor: 'rgba(239,68,68,0.75)',
          borderColor: 'rgba(239,68,68,1)',
          borderWidth: 1.5,
          borderRadius: 6,
          barThickness: 22
        }]
      },
      options: {
        indexAxis: 'y',
        responsive: true,
        maintainAspectRatio: false,
        plugins: {
          legend: { display: false },
          tooltip: { callbacks: { label: (ctx: any) => ` ${ctx.parsed.x} u. sin salida` } }
        },
        scales: {
          x: { beginAtZero: true, grid: { color: 'rgba(0,0,0,0.04)' }, ticks: { font: { size: 11 } } },
          y: { grid: { display: false }, ticks: { font: { size: 11 } } }
        }
      }
    });
  }

  private renderChartFamilias(): void {
    const canvas = this.chartFamiliasRef?.nativeElement;
    if (!canvas) return;
    this.chartFamilias?.destroy();

    const top = this.datosFamilias.slice(0, 10);

    this.chartFamilias = new Chart(canvas, {
      type: 'bar',
      data: {
        labels: top.map(f => f.familia),
        datasets: [
          {
            label: 'Egresos (consumo)',
            data: top.map(f => f.totalEgresos),
            backgroundColor: 'rgba(20,184,166,0.8)',
            borderColor: 'rgba(20,184,166,1)',
            borderWidth: 1.5,
            borderRadius: 6,
            barThickness: 16
          },
          {
            label: 'Ingresos',
            data: top.map(f => f.totalIngresos),
            backgroundColor: 'rgba(99,102,241,0.5)',
            borderColor: 'rgba(99,102,241,1)',
            borderWidth: 1.5,
            borderRadius: 6,
            barThickness: 16
          }
        ]
      },
      options: {
        indexAxis: 'y',
        responsive: true,
        maintainAspectRatio: false,
        plugins: {
          legend: { display: true, position: 'top' },
          tooltip: { callbacks: { label: (ctx: any) => ` ${ctx.dataset.label}: ${ctx.parsed.x} u.` } }
        },
        scales: {
          x: { beginAtZero: true, grid: { color: 'rgba(0,0,0,0.04)' }, ticks: { font: { size: 11 } } },
          y: { grid: { display: false }, ticks: { font: { size: 11 } } }
        }
      }
    });
  }

  // ── Exportar CSV ──────────────────────────────────────────────────────────────

  exportarCSV(): void {
    let csv = '';
    let filename = '';

    if (this.activeTab === 'rotacion') {
      csv = 'Producto,SKU,Familia,Sede,Ingresos,Egresos,Stock Promedio,Índice Rotación,Último Ingreso,Último Egreso,Tendencia\n';
      this.datosRotFiltrados.forEach(p => {
        csv += `"${p.producto}","${p.sku}","${p.familia}","${p.sede}",${p.totalIngresos},${p.totalEgresos},${p.stockPromedioPonderado},${p.indiceRotacion},"${p.ultimoIngreso ?? ''}","${p.ultimoEgreso ?? ''}","${p.tendencia}"\n`;
      });
      filename = 'rotacion';
    } else if (this.activeTab === 'ingresos') {
      csv = 'Producto,SKU,Familia,Sede,Total Unidades,Cantidad Operaciones,Última Fecha\n';
      this.datosIngFiltrados.forEach(p => {
        csv += `"${p.producto}","${p.sku}","${p.familia}","${p.sede}",${p.totalUnidades},${p.cantidadOperaciones},"${p.ultimaFecha ?? ''}"\n`;
      });
      filename = 'ingresos';
    } else if (this.activeTab === 'inmovilizado') {
      csv = 'Producto,SKU,Familia,Sede,Stock Actual,Último Ingreso,Días sin Egreso\n';
      this.datosInmFiltrados.forEach(p => {
        csv += `"${p.producto}","${p.sku}","${p.familia}","${p.sede}",${p.stockActual},"${p.ultimoIngreso ? this.formatFecha(p.ultimoIngreso) : ''}",${p.diasSinEgreso}\n`;
      });
      filename = 'inmovilizado';
    } else if (this.activeTab === 'familias') {
      csv = 'Familia,Ingresos,Egresos,Ratio Consumo,Productos\n';
      this.datosFamiliasFiltrados.forEach(f => {
        csv += `"${f.familia}",${f.totalIngresos},${f.totalEgresos},${f.ratioConsumo},${f.cantidadProductos}\n`;
      });
      filename = 'familias-consumo';
    } else {
      csv = 'Producto,SKU,Familia,Sede,Total Unidades,Cantidad Operaciones,Última Fecha\n';
      this.datosEgrFiltrados.forEach(p => {
        csv += `"${p.producto}","${p.sku}","${p.familia}","${p.sede}",${p.totalUnidades},${p.cantidadOperaciones},"${p.ultimaFecha ?? ''}"\n`;
      });
      filename = 'egresos';
    }

    const blob = new Blob(['\ufeff' + csv], { type: 'text/csv;charset=utf-8;' });
    const url = URL.createObjectURL(blob);
    const a = document.createElement('a');
    a.href = url;
    a.download = `rotacion-${filename}-${new Date().toISOString().slice(0, 10)}.csv`;
    a.click();
    URL.revokeObjectURL(url);
  }

  // ── Helpers ───────────────────────────────────────────────────────────────────

  badgeTendencia(t: string): string {
    return t === 'Alta'  ? 'badge-tend-alta'  :
           t === 'Media' ? 'badge-tend-media' : 'badge-tend-baja';
  }

  trackById(index: number, item: any): number { return item.idProducto ?? index; }

  formatFecha(fecha: string | null | undefined): string {
    if (!fecha) return '—';
    return new Date(fecha).toLocaleDateString('es-AR', { day: '2-digit', month: '2-digit', year: 'numeric' });
  }
}
