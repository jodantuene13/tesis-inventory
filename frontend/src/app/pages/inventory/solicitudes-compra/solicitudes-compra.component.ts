import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { SolicitudCompraService } from '../../../services/solicitud-compra.service';
import { StockService } from '../../../services/stock.service';
import { SolicitudCompra, EstadoSolicitudCompra, CreateSolicitudCompra } from '../../../models/solicitud-compra.model';
import { Stock } from '../../../models/stock.model';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-solicitudes-compra',
  standalone: true,
  imports: [CommonModule, FormsModule, ReactiveFormsModule],
  templateUrl: './solicitudes-compra.component.html',
  styleUrls: ['./solicitudes-compra.component.css']
})
export class SolicitudesCompraComponent implements OnInit {
  solicitudes: SolicitudCompra[] = [];
  loading = false;
  
  // Indicators
  indicatorTotal = 0;
  indicatorPendientes = 0;
  indicatorAprobadas = 0;
  indicatorRechazadas = 0;

  // Filters
  searchTerm = '';
  estadoFiltro: number | null = null;
  page = 1;
  pageSize = 10;
  totalCount = 0;
  totalPages = 0;

  // Create Modal
  showCreateModal = false;
  showDetailModal = false;
  showPrintModal = false;
  selectedSolicitud: SolicitudCompra | null = null;
  solicitudForm: FormGroup;
  productos: Stock[] = []; // Usamos stock para elegir productos de la sede

  // Enum for template
  EstadoEnum = EstadoSolicitudCompra;

  constructor(
    private solicitudService: SolicitudCompraService,
    private stockService: StockService,
    private fb: FormBuilder
  ) {
    this.solicitudForm = this.fb.group({
      idProducto: ['', Validators.required],
      cantidad: [1, [Validators.required, Validators.min(1)]],
      observaciones: ['']
    });
  }

  ngOnInit(): void {
    this.loadSolicitudes();
    this.loadProductos();
  }

  loadSolicitudes(): void {
    this.loading = true;
    this.solicitudService.getAll(this.searchTerm, this.estadoFiltro ?? undefined, this.page, this.pageSize).subscribe({
      next: (res) => {
        this.solicitudes = res.data;
        this.totalCount = res.totalCount;
        this.totalPages = res.totalPages;
        this.calculateIndicators();
        this.loading = false;
      },
      error: () => {
        this.loading = false;
        Swal.fire('Error', 'No se pudieron cargar las solicitudes', 'error');
      }
    });
  }

  loadProductos(): void {
    // Cargamos productos activos de la sede para el selector
    this.stockService.getStockSede(undefined, undefined, undefined, true, undefined, 1, 100).subscribe(res => {
      this.productos = res.data;
    });
  }

  calculateIndicators(): void {
    // Para indicadores reales, idealmente tendríamos un endpoint de estadísticas. 
    // Por ahora filtramos del set actual o traemos totales si page=1.
    this.solicitudService.getAll(undefined, undefined, 1, 1000).subscribe(res => {
      const all = res.data as SolicitudCompra[];
      this.indicatorTotal = all.length;
      this.indicatorPendientes = all.filter(s => s.estado === EstadoSolicitudCompra.Pendiente).length;
      this.indicatorAprobadas = all.filter(s => s.estado === EstadoSolicitudCompra.Aprobada).length;
      this.indicatorRechazadas = all.filter(s => s.estado === EstadoSolicitudCompra.Rechazada).length;
    });
  }

  search(): void {
    this.page = 1;
    this.loadSolicitudes();
  }

  filterByEstado(estado: number | null): void {
    this.estadoFiltro = estado;
    this.search();
  }

  openCreateModal(): void {
    this.solicitudForm.reset({ cantidad: 1 });
    this.showCreateModal = true;
  }

  closeCreateModal(): void {
    this.showCreateModal = false;
  }

  openDetailModal(s: SolicitudCompra): void {
    this.selectedSolicitud = s;
    this.showDetailModal = true;
  }

  closeDetailModal(): void {
    this.showDetailModal = false;
    this.selectedSolicitud = null;
  }

  openPrintModal(s: SolicitudCompra): void {
    this.selectedSolicitud = s;
    this.showPrintModal = true;
  }

  closePrintModal(): void {
    this.showPrintModal = false;
  }

  printVoucher(): void {
    window.print();
  }

  saveSolicitud(): void {
    if (this.solicitudForm.invalid) return;

    const dto: CreateSolicitudCompra = this.solicitudForm.value;
    this.solicitudService.create(dto).subscribe({
      next: () => {
        Swal.fire('Éxito', 'Solicitud enviada correctamente', 'success');
        this.closeCreateModal();
        this.loadSolicitudes();
      },
      error: (err) => Swal.fire('Error', err.error?.message || 'Error al enviar solicitud', 'error')
    });
  }

  procesarSolicitud(s: SolicitudCompra, nuevoEstado: EstadoSolicitudCompra): void {
    const actionText = nuevoEstado === EstadoSolicitudCompra.Aprobada ? 'aprobar' : 'rechazar';
    
    if (nuevoEstado === EstadoSolicitudCompra.Rechazada) {
      Swal.fire({
        title: 'Rechazar Solicitud',
        text: 'Ingrese el motivo del rechazo:',
        input: 'text',
        icon: 'warning',
        showCancelButton: true,
        confirmButtonText: 'Confirmar Rechazo',
        cancelButtonText: 'Cancelar',
        preConfirm: (value) => {
          if (!value) {
            Swal.showValidationMessage('El motivo es obligatorio');
          }
          return value;
        }
      }).then((result) => {
        if (result.isConfirmed) {
          this.confirmarCambioEstado(s.idSolicitudCompra, nuevoEstado, result.value);
        }
      });
    } else {
      Swal.fire({
        title: '¿Confirmar aprobación?',
        text: `Se aprobará la solicitud de ${s.cantidad} unidades de ${s.nombreProducto}`,
        icon: 'question',
        showCancelButton: true,
        confirmButtonText: 'Sí, aprobar',
        cancelButtonText: 'Cancelar'
      }).then((result) => {
        if (result.isConfirmed) {
          this.confirmarCambioEstado(s.idSolicitudCompra, nuevoEstado);
        }
      });
    }
  }

  confirmarCambioEstado(id: number, nuevoEstado: EstadoSolicitudCompra, motivo?: string): void {
    this.solicitudService.updateEstado(id, { nuevoEstado, motivoRechazo: motivo }).subscribe({
      next: () => {
        Swal.fire('Éxito', `Solicitud ${nuevoEstado === EstadoSolicitudCompra.Aprobada ? 'aprobada' : 'rechazada'}`, 'success');
        this.loadSolicitudes();
      },
      error: (err) => Swal.fire('Error', err.error?.message || 'Error al procesar solicitud', 'error')
    });
  }

  nextPage(): void {
    if (this.page < this.totalPages) {
      this.page++;
      this.loadSolicitudes();
    }
  }

  prevPage(): void {
    if (this.page > 1) {
      this.page--;
      this.loadSolicitudes();
    }
  }
}
