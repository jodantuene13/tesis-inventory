import { Component, OnInit, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import {
  InformesService,
  InformeSolicitudesCompraDto,
  SolicitudesPorEntidadDto,
  SolicitudPendienteDto
} from '../../../services/informes.service';
import { Sede } from '../../../models/sede.model';
import { SedeService } from '../../../services/sede.service';
import { AuthService } from '../../../services/auth';
import {
  Chart,
  BarController, BarElement,
  CategoryScale, LinearScale, Tooltip, Legend
} from 'chart.js';

Chart.register(BarController, BarElement, CategoryScale, LinearScale, Tooltip, Legend);

type ActiveTab = 'breakdown' | 'pendientes' | 'productos';

@Component({
  selector: 'app-solicitudes-compra-informe',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './solicitudes-compra.component.html',
  styleUrls: ['./solicitudes-compra.component.css']
})
export class SolicitudesCompraInformeComponent implements OnInit, OnDestroy {

  // ── Filtros ───────────────────────────────────────────────────────────────
  periodoPreset: '30' | '90' | 'custom' = '30';
  fechaDesde: string = '';
  fechaHasta: string = '';
  sedes: Sede[] = [];
  sedeId: number | null = null;
  isAdmin: boolean = false;

  // ── Tabs ──────────────────────────────────────────────────────────────────
  activeTab: ActiveTab = 'breakdown';

  // ── Toggle usuario/sede (Tab 1) ───────────────────────────────────────────
  vistaAgrupacion: 'usuario' | 'sede' = 'usuario';

  // ── Filtro pendientes (Tab 2) ─────────────────────────────────────────────
  filtroPendientes: 'todos' | '5' | '10' | '30' | 'mas30' = 'todos';

  // ── Estado ────────────────────────────────────────────────────────────────
  informe: InformeSolicitudesCompraDto | null = null;
  loading: boolean = false;
  error: string = '';

  private charts: Map<string, Chart> = new Map();
  private destroy$ = new Subject<void>();

  constructor(
    private informesService: InformesService,
    private sedeService: SedeService,
    private authService: AuthService
  ) {}

  ngOnInit(): void {
    this.initFechas();
    this.authService.user$.pipe(takeUntil(this.destroy$)).subscribe(user => {
      this.isAdmin = user?.nombreRol === 'Admin';
    });
    this.sedeService.getAll().pipe(takeUntil(this.destroy$))
      .subscribe((res: Sede[]) => { this.sedes = res; });
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

  aplicarFiltros(): void {
    this.periodoPreset = 'custom';
    this.loadInforme();
  }

  limpiarFiltros(): void {
    this.sedeId = null;
    this.vistaAgrupacion = 'usuario';
    this.filtroPendientes = 'todos';
    this.periodoPreset = '30';
    this.initFechas();
    this.loadInforme();
  }

  // ── Carga ─────────────────────────────────────────────────────────────────
  private loadInforme(): void {
    this.loading = true;
    this.error = '';
    this.informesService.getInformeSolicitudesCompra(
      this.sedeId ?? undefined,
      this.fechaDesde,
      this.fechaHasta,
      15
    ).pipe(takeUntil(this.destroy$)).subscribe({
      next: (res) => {
        this.informe = res;
        this.loading = false;
        setTimeout(() => this.renderChartsForTab(this.activeTab), 60);
      },
      error: () => {
        this.error = 'No se pudo cargar el informe de solicitudes de compra.';
        this.loading = false;
      }
    });
  }

  // ── Toggle agrupación ─────────────────────────────────────────────────────
  get toggleDisabled(): boolean {
    return this.sedeId !== null;
  }

  setAgrupacion(v: 'usuario' | 'sede'): void {
    if (this.toggleDisabled) return;
    this.vistaAgrupacion = v;
    setTimeout(() => this.renderChartsForTab(this.activeTab), 60);
  }

  get datosBreakdown(): SolicitudesPorEntidadDto[] {
    if (!this.informe) return [];
    return this.vistaAgrupacion === 'usuario' || this.toggleDisabled
      ? this.informe.porUsuario
      : this.informe.porSede;
  }

  // ── Pendientes filtradas ──────────────────────────────────────────────────
  get pendientesFiltradas(): SolicitudPendienteDto[] {
    if (!this.informe) return [];
    const all = this.informe.pendientes;
    if (this.filtroPendientes === '5') return all.filter(p => p.diasEsperando <= 5);
    if (this.filtroPendientes === '10') return all.filter(p => p.diasEsperando > 5 && p.diasEsperando <= 10);
    if (this.filtroPendientes === '30') return all.filter(p => p.diasEsperando > 10 && p.diasEsperando <= 30);
    if (this.filtroPendientes === 'mas30') return all.filter(p => p.diasEsperando > 30);
    return all;
  }

  // ── Tabs ──────────────────────────────────────────────────────────────────
  setTab(tab: ActiveTab): void {
    this.activeTab = tab;
    setTimeout(() => this.renderChartsForTab(tab), 60);
  }

  // ── Charts ────────────────────────────────────────────────────────────────
  private destroyAllCharts(): void {
    this.charts.forEach(c => c.destroy());
    this.charts.clear();
  }

  private destroyChart(id: string): void {
    const c = this.charts.get(id);
    if (c) { c.destroy(); this.charts.delete(id); }
  }

  private renderChartsForTab(tab: ActiveTab): void {
    if (!this.informe) return;
    if (tab === 'breakdown') this.renderBreakdownChart();
    if (tab === 'productos') this.renderProductosChart();
  }

  private renderBreakdownChart(): void {
    this.destroyChart('chart-breakdown');
    const canvas = document.getElementById('chart-breakdown') as HTMLCanvasElement;
    const datos = this.datosBreakdown;
    if (!canvas || !datos.length) return;

    const top = datos.slice(0, 10);
    const chart = new Chart(canvas, {
      type: 'bar',
      data: {
        labels: top.map(d => d.nombre.length > 20 ? d.nombre.slice(0, 19) + '…' : d.nombre),
        datasets: [
          {
            label: 'Aprobadas',
            data: top.map(d => d.aprobadas),
            backgroundColor: '#10b981',
            borderRadius: 4,
            barThickness: 14
          },
          {
            label: 'Rechazadas',
            data: top.map(d => d.rechazadas),
            backgroundColor: '#ef4444',
            borderRadius: 4,
            barThickness: 14
          },
          {
            label: 'Pendientes',
            data: top.map(d => d.pendientes),
            backgroundColor: '#f59e0b',
            borderRadius: 4,
            barThickness: 14
          }
        ]
      },
      options: {
        indexAxis: 'y',
        responsive: true,
        maintainAspectRatio: false,
        plugins: {
          legend: { position: 'bottom', labels: { font: { size: 11 }, boxWidth: 12 } },
          tooltip: { mode: 'index', intersect: false }
        },
        scales: {
          x: { beginAtZero: true, stacked: false, grid: { color: '#f1f5f9' }, ticks: { font: { size: 11 } } },
          y: { grid: { display: false }, ticks: { font: { size: 11 } } }
        }
      }
    });
    this.charts.set('chart-breakdown', chart);
  }

  private renderProductosChart(): void {
    this.destroyChart('chart-productos');
    const canvas = document.getElementById('chart-productos') as HTMLCanvasElement;
    const datos = this.informe?.productosMasSolicitados;
    if (!canvas || !datos?.length) return;

    const top = datos.slice(0, 10);
    const chart = new Chart(canvas, {
      type: 'bar',
      data: {
        labels: top.map(p => p.producto.length > 18 ? p.producto.slice(0, 17) + '…' : p.producto),
        datasets: [
          {
            label: 'Total unidades',
            data: top.map(p => p.totalUnidades),
            backgroundColor: '#6366f1',
            borderRadius: 4,
            barThickness: 14
          },
          {
            label: 'Veces solicitado',
            data: top.map(p => p.vecesSolicitado),
            backgroundColor: '#0d9488',
            borderRadius: 4,
            barThickness: 14
          }
        ]
      },
      options: {
        indexAxis: 'x',
        responsive: true,
        maintainAspectRatio: false,
        plugins: {
          legend: { position: 'bottom', labels: { font: { size: 11 }, boxWidth: 12 } },
          tooltip: { mode: 'index', intersect: false }
        },
        scales: {
          y: { beginAtZero: true, grid: { color: '#f1f5f9' }, ticks: { font: { size: 11 } } },
          x: { grid: { display: false }, ticks: { font: { size: 10 }, maxRotation: 30 } }
        }
      }
    });
    this.charts.set('chart-productos', chart);
  }

  // ── Helpers ───────────────────────────────────────────────────────────────
  diasBadgeClass(dias: number): string {
    if (dias > 30) return 'badge-danger';
    if (dias > 10) return 'badge-warning';
    return 'badge-ok';
  }

  formatFecha(iso: string): string {
    if (!iso) return '—';
    return new Date(iso).toLocaleDateString('es-AR', { day: '2-digit', month: '2-digit', year: 'numeric' });
  }

  // ── Exportar CSV ──────────────────────────────────────────────────────────
  exportarCSV(): void {
    if (!this.informe) return;
    let csv = '';
    let filename = '';

    if (this.activeTab === 'breakdown') {
      const label = (this.vistaAgrupacion === 'usuario' || this.toggleDisabled) ? 'Usuario' : 'Sede';
      csv = `${label},Total,Aprobadas,Rechazadas,Pendientes,Cumpl. Total,Cumpl. Parcial,No Concretadas,T.Prom Decisión (días),T.Prom Stock (días)\n`;
      this.datosBreakdown.forEach(d => {
        csv += `"${d.nombre}",${d.total},${d.aprobadas},${d.rechazadas},${d.pendientes},${d.cumplimientoTotal},${d.cumplimientoParcial},${d.noConcretadas},${d.tiempoPromedioDecisionDias},${d.tiempoPromedioStockDias}\n`;
      });
      filename = `solicitudes_por_${this.vistaAgrupacion}_${new Date().toISOString().slice(0, 10)}.csv`;
    } else if (this.activeTab === 'pendientes') {
      csv = 'ID,Usuario,Sede,Fecha solicitud,Días esperando,Productos,Motivo\n';
      this.pendientesFiltradas.forEach(p => {
        csv += `${p.idSolicitudCompra},"${p.usuario}","${p.sede}","${this.formatFecha(p.fechaSolicitud)}",${p.diasEsperando},"${p.productos}","${p.motivoSolicitud}"\n`;
      });
      filename = `solicitudes_pendientes_${new Date().toISOString().slice(0, 10)}.csv`;
    } else {
      csv = 'Producto,SKU,Familia,Unidad,Total unidades,Veces solicitado,Veces en aprobadas\n';
      this.informe.productosMasSolicitados.forEach(p => {
        csv += `"${p.producto}","${p.sku}","${p.familia}","${p.unidadMedida}",${p.totalUnidades},${p.vecesSolicitado},${p.vecesEnAprobadas}\n`;
      });
      filename = `productos_solicitados_${new Date().toISOString().slice(0, 10)}.csv`;
    }

    const blob = new Blob(['﻿' + csv], { type: 'text/csv;charset=utf-8;' });
    const url = URL.createObjectURL(blob);
    const a = document.createElement('a');
    a.href = url;
    a.download = filename;
    a.click();
    URL.revokeObjectURL(url);
  }
}
