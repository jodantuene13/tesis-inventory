import { Component, OnInit, OnDestroy } from '@angular/core';
import { CommonModule, DecimalPipe } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import {
  InformesService,
  InformeTransferenciaDto,
  TransferenciaDetalleDto
} from '../../../services/informes.service';
import { Sede } from '../../../models/sede.model';
import { SedeService } from '../../../services/sede.service';
import { Familia } from '../../../models/familia.model';
import { FamiliaService } from '../../../services/familia.service';
import { AuthService } from '../../../services/auth';
import {
  Chart,
  BarController, BarElement,
  LineController, LineElement, PointElement,
  DoughnutController, ArcElement,
  CategoryScale, LinearScale, Tooltip, Legend, Title
} from 'chart.js';

Chart.register(
  BarController, BarElement,
  LineController, LineElement, PointElement,
  DoughnutController, ArcElement,
  CategoryScale, LinearScale, Tooltip, Legend, Title
);

@Component({
  selector: 'app-transferencias',
  standalone: true,
  imports: [CommonModule, FormsModule, DecimalPipe],
  templateUrl: './transferencias.component.html',
  styleUrls: ['./transferencias.component.css']
})
export class TransferenciasComponent implements OnInit, OnDestroy {

  // ── Filtros ───────────────────────────────────────────────────────────────
  periodoPreset: '30' | '90' | 'custom' = '30';
  fechaDesde: string = '';
  fechaHasta: string = '';

  sedes: Sede[] = [];
  sedeOrigenId: number | null = null;
  sedeDestinoId: number | null = null;
  isAdmin: boolean = false;

  familias: Familia[] = [];
  familiaId: number | null = null;

  motivos = [
    { value: null,  label: 'Todos' },
    { value: 0,     label: 'Transferencia Definitiva' },
    { value: 1,     label: 'Préstamo' }
  ];
  motivoSeleccionado: number | null = null;

  estados = [
    { value: null, label: 'Todos' },
    { value: 0,    label: 'Solicitada' },
    { value: 1,    label: 'Aprobada' },
    { value: 2,    label: 'Rechazada' },
    { value: 3,    label: 'En Tránsito' },
    { value: 4,    label: 'Recibida' },
    { value: 5,    label: 'Pendiente Devolución' },
    { value: 6,    label: 'Devuelta' },
    { value: 7,    label: 'Completada' }
  ];
  estadoSeleccionado: number | null = null;

  // ── Tabs ─────────────────────────────────────────────────────────────────
  activeTab: number = 0;

  // ── Estado ───────────────────────────────────────────────────────────────
  informe: InformeTransferenciaDto | null = null;
  loading: boolean = false;
  error: string = '';

  // ── Paginación ────────────────────────────────────────────────────────────
  currentPage: number = 1;
  pageSize: number = 50;

  // ── Charts (referencias por id) ───────────────────────────────────────────
  private charts: Map<string, Chart> = new Map();

  private destroy$ = new Subject<void>();

  constructor(
    private informesService: InformesService,
    private sedeService: SedeService,
    private familiaService: FamiliaService,
    private authService: AuthService
  ) {}

  ngOnInit(): void {
    this.initFechas();
    this.authService.user$.pipe(takeUntil(this.destroy$)).subscribe(user => {
      this.isAdmin = user?.nombreRol === 'Admin';
    });
    this.sedeService.getAll().pipe(takeUntil(this.destroy$))
      .subscribe((res: Sede[]) => { this.sedes = res; });
    this.familiaService.getAll().pipe(takeUntil(this.destroy$))
      .subscribe((res: Familia[]) => { this.familias = res; });
    this.loadInforme();
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
    this.destroyAllCharts();
  }

  // ── Fechas ────────────────────────────────────────────────────────────────
  private initFechas(): void {
    const hoy = new Date();
    const desde = new Date();
    desde.setDate(hoy.getDate() - 30);
    this.fechaDesde = desde.toISOString().split('T')[0];
    this.fechaHasta = hoy.toISOString().split('T')[0];
  }

  setPeriodo(preset: '30' | '90'): void {
    this.periodoPreset = preset;
    const hoy = new Date();
    const desde = new Date();
    desde.setDate(hoy.getDate() - parseInt(preset));
    this.fechaDesde = desde.toISOString().split('T')[0];
    this.fechaHasta = hoy.toISOString().split('T')[0];
    this.loadInforme();
  }

  // ── Carga ─────────────────────────────────────────────────────────────────
  aplicarFiltros(): void {
    this.periodoPreset = 'custom';
    this.loadInforme();
  }

  private loadInforme(): void {
    this.loading = true;
    this.error = '';

    this.informesService.getInformeTransferencias(
      this.sedeOrigenId ?? undefined,
      this.sedeDestinoId ?? undefined,
      this.familiaId ?? undefined,
      this.motivoSeleccionado !== null ? this.motivoSeleccionado : undefined,
      this.estadoSeleccionado !== null ? this.estadoSeleccionado : undefined,
      this.fechaDesde,
      this.fechaHasta,
      10
    ).pipe(takeUntil(this.destroy$)).subscribe({
      next: (res) => {
        this.informe = res;
        this.loading = false;
        this.currentPage = 1;
        setTimeout(() => this.renderChartsForTab(this.activeTab), 60);
      },
      error: () => {
        this.error = 'No se pudo cargar el informe de transferencias.';
        this.loading = false;
      }
    });
  }

  // ── Tabs ──────────────────────────────────────────────────────────────────
  setTab(index: number): void {
    this.activeTab = index;
    setTimeout(() => this.renderChartsForTab(index), 60);
  }

  // ── Paginación ────────────────────────────────────────────────────────────
  get paginatedDetalles(): TransferenciaDetalleDto[] {
    if (!this.informe?.detalleMovimientos) return [];
    const start = (this.currentPage - 1) * this.pageSize;
    return this.informe.detalleMovimientos.slice(start, start + this.pageSize);
  }

  get totalPages(): number {
    if (!this.informe?.detalleMovimientos) return 1;
    return Math.max(1, Math.ceil(this.informe.detalleMovimientos.length / this.pageSize));
  }

  get paginacionDesde(): number {
    return (this.currentPage - 1) * this.pageSize + 1;
  }

  get paginacionHasta(): number {
    return Math.min(this.currentPage * this.pageSize, this.informe?.detalleMovimientos?.length ?? 0);
  }

  prevPage(): void { if (this.currentPage > 1) this.currentPage--; }
  nextPage(): void { if (this.currentPage < this.totalPages) this.currentPage++; }

  // ── Renderizado de gráficos ────────────────────────────────────────────────
  private destroyAllCharts(): void {
    this.charts.forEach(c => c.destroy());
    this.charts.clear();
  }

  private destroyChart(id: string): void {
    const c = this.charts.get(id);
    if (c) { c.destroy(); this.charts.delete(id); }
  }

  private renderChartsForTab(tab: number): void {
    if (!this.informe) return;

    if (tab === 0) {
      this.renderSedesChart('g-sedes-global', true);
      this.renderProductosChart('g-prods-global', true);
      this.renderDoughnutChart('g-tipo-global');
      this.renderPrestamosChart('g-prestamos-global');
    } else if (tab === 1) {
      this.renderSedesChart('g-sedes-tab', false);
      this.renderProductosChart('g-prods-tab', false);
    } else if (tab === 2) {
      this.renderPrestamosChart('g-prestamos-tab');
    } else if (tab === 3) {
      this.renderRechazosChart('g-rechazos-tab');
    }
  }

  private renderSedesChart(id: string, horizontal: boolean): void {
    this.destroyChart(id);
    const canvas = document.getElementById(id) as HTMLCanvasElement;
    if (!canvas || !this.informe?.habitualesPorSede?.length) return;

    const data = this.informe.habitualesPorSede;
    const chart = new Chart(canvas, {
      type: 'bar',
      data: {
        labels: data.map(s => s.nombre),
        datasets: [{
          label: 'Transferencias',
          data: data.map(s => s.cantidad),
          backgroundColor: '#2563eb',
          borderRadius: 5,
          barThickness: horizontal ? 16 : 22,
        }]
      },
      options: {
        indexAxis: horizontal ? 'y' : 'x',
        responsive: true,
        maintainAspectRatio: false,
        plugins: {
          legend: { display: false },
          tooltip: { callbacks: { label: (ctx: any) => ` ${ctx.parsed[horizontal ? 'x' : 'y']} transferencias` } }
        },
        scales: {
          x: { beginAtZero: true, grid: { color: '#f1f5f9' }, ticks: { font: { size: 11 } } },
          y: { grid: { display: false }, ticks: { font: { size: 11 } } }
        }
      }
    });
    this.charts.set(id, chart);
  }

  private renderProductosChart(id: string, horizontal: boolean): void {
    this.destroyChart(id);
    const canvas = document.getElementById(id) as HTMLCanvasElement;
    if (!canvas || !this.informe?.habitualesPorProducto?.length) return;

    const data = this.informe.habitualesPorProducto;
    const chart = new Chart(canvas, {
      type: 'bar',
      data: {
        labels: data.map(p => p.nombre.length > 18 ? p.nombre.slice(0, 17) + '…' : p.nombre),
        datasets: [{
          label: 'Transferencias',
          data: data.map(p => p.cantidad),
          backgroundColor: '#2563eb',
          borderRadius: 5,
          barThickness: 22,
        }]
      },
      options: {
        indexAxis: 'x',
        responsive: true,
        maintainAspectRatio: false,
        plugins: {
          legend: { display: false },
          tooltip: { callbacks: { label: (ctx: any) => ` ${ctx.parsed.y} transferencias` } }
        },
        scales: {
          y: { beginAtZero: true, grid: { color: '#f1f5f9' }, ticks: { font: { size: 11 } } },
          x: { grid: { display: false }, ticks: { font: { size: 10 }, maxRotation: 35 } }
        }
      }
    });
    this.charts.set(id, chart);
  }

  private renderDoughnutChart(id: string): void {
    this.destroyChart(id);
    const canvas = document.getElementById(id) as HTMLCanvasElement;
    if (!canvas || !this.informe) return;

    const total = this.informe.kpis.totalTransferencias + this.informe.kpis.totalPrestamos;
    const chart = new Chart(canvas, {
      type: 'doughnut',
      data: {
        labels: ['Transferencia', 'Préstamo'],
        datasets: [{
          data: [this.informe.kpis.totalTransferencias, this.informe.kpis.totalPrestamos],
          backgroundColor: ['#2563eb', '#10b981'],
          borderWidth: 0,
          hoverOffset: 4
        }]
      },
      options: {
        responsive: true,
        maintainAspectRatio: false,
        cutout: '72%',
        plugins: {
          legend: { display: false },
          tooltip: {
            callbacks: {
              label: (ctx: any) => {
                const val = ctx.parsed as number;
                const pct = total > 0 ? ((val / total) * 100).toFixed(1) : '0';
                return ` ${ctx.label}: ${val} (${pct}%)`;
              }
            }
          }
        }
      }
    });
    this.charts.set(id, chart);
  }

  private renderPrestamosChart(id: string): void {
    this.destroyChart(id);
    const canvas = document.getElementById(id) as HTMLCanvasElement;
    if (!canvas || !this.informe?.prestamosPorDia?.length) return;

    const data = this.informe.prestamosPorDia;
    const chart = new Chart(canvas, {
      type: 'line',
      data: {
        labels: data.map(p => {
          const d = new Date(p.fecha);
          return `${d.getDate()} ${d.toLocaleString('es-AR', { month: 'short' })}`;
        }),
        datasets: [{
          label: 'Préstamos activos',
          data: data.map(p => p.cantidadActivos),
          borderColor: '#2563eb',
          backgroundColor: 'rgba(37,99,235,0.08)',
          fill: true,
          tension: 0.35,
          pointRadius: 4,
          pointBackgroundColor: '#2563eb',
          pointBorderColor: '#fff',
          pointBorderWidth: 2,
          borderWidth: 2.5
        }]
      },
      options: {
        responsive: true,
        maintainAspectRatio: false,
        plugins: {
          legend: { position: 'bottom', labels: { font: { size: 11 }, boxWidth: 14 } },
          tooltip: { callbacks: { label: (ctx: any) => ` ${ctx.parsed.y} préstamos activos` } }
        },
        scales: {
          y: { beginAtZero: true, grid: { color: '#f1f5f9' }, ticks: { font: { size: 11 } } },
          x: { grid: { display: false }, ticks: { font: { size: 10 }, maxRotation: 0 } }
        }
      }
    });
    this.charts.set(id, chart);
  }

  private renderRechazosChart(id: string): void {
    this.destroyChart(id);
    const canvas = document.getElementById(id) as HTMLCanvasElement;
    if (!canvas || !this.informe?.rechazosPorProducto?.length) return;

    const data = this.informe.rechazosPorProducto;
    const chart = new Chart(canvas, {
      type: 'bar',
      data: {
        labels: data.map(r => r.producto.length > 20 ? r.producto.slice(0, 19) + '…' : r.producto),
        datasets: [{
          label: 'Índice de rechazo (%)',
          data: data.map(r => r.indice),
          backgroundColor: '#ef4444',
          borderRadius: 5,
          barThickness: 18,
        }]
      },
      options: {
        indexAxis: 'y',
        responsive: true,
        maintainAspectRatio: false,
        plugins: {
          legend: { display: false },
          tooltip: { callbacks: { label: (ctx: any) => ` Rechazo: ${ctx.parsed.x}%` } }
        },
        scales: {
          x: { beginAtZero: true, max: 100, grid: { color: '#f1f5f9' }, ticks: { callback: (v: any) => v + '%', font: { size: 11 } } },
          y: { grid: { display: false }, ticks: { font: { size: 11 } } }
        }
      }
    });
    this.charts.set(id, chart);
  }

  // ── Helpers ───────────────────────────────────────────────────────────────
  getEstadoClass(estado: string): string {
    const e = (estado ?? '').toLowerCase();
    if (e.includes('completada') || e.includes('devuelta') || e.includes('recibida')) return 'badge-success';
    if (e.includes('rechazada')) return 'badge-danger';
    if (e.includes('tránsito') || e.includes('transito') || e.includes('aprobada')) return 'badge-transit';
    if (e.includes('pendiente')) return 'badge-warning';
    return 'badge-info';
  }

  exportarCSV(): void {
    if (!this.informe) return;
    let csv = 'Fecha,Tipo,Productos,Origen,Destino,Cantidad,Estado,Usuario\n';
    this.informe.detalleMovimientos.forEach(d => {
      const fecha = new Date(d.fechaSolicitud).toLocaleString('es-AR');
      csv += `"${fecha}","${d.tipo}","${d.productos}","${d.sedeOrigen}","${d.sedeDestino}",${d.cantidadTotal},"${d.estado}","${d.usuario}"\n`;
    });
    const blob = new Blob(['\ufeff' + csv], { type: 'text/csv;charset=utf-8;' });
    const url = URL.createObjectURL(blob);
    const a = document.createElement('a');
    a.href = url;
    a.download = `transferencias_prestamos_${new Date().toISOString().slice(0, 10)}.csv`;
    a.click();
    URL.revokeObjectURL(url);
  }
}
