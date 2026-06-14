import { Component, OnInit, OnDestroy, HostListener, ElementRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { TransferenciaService } from '../../../services/transferencia.service';
import { SedeContextService } from '../../../services/sede-context.service';
import { SedeService } from '../../../services/sede.service';
import { StockService } from '../../../services/stock.service';
import { Transferencia, EstadoTransferencia } from '../../../models/transferencia.model';
import { Sede } from '../../../models/sede.model';
import { forkJoin, Subscription } from 'rxjs';

export interface TransferenciaLog extends Transferencia {
  tipo: 'Entrante' | 'Saliente';
}

import { PaginationComponent } from '../../../shared/components/pagination/pagination.component';

@Component({
  selector: 'app-historico-transferencias',
  standalone: true,
  imports: [CommonModule, FormsModule, PaginationComponent],
  templateUrl: './historico-transferencias.component.html',
  styleUrls: []
})
export class HistoricoTransferenciasComponent implements OnInit, OnDestroy {
  allTransferencias: TransferenciaLog[] = [];
  filteredTransferencias: TransferenciaLog[] = [];
  paginatedTransferencias: TransferenciaLog[] = [];
  sedes: Sede[] = [];

  // Paginación
  totalCount: number = 0;
  page: number = 1;
  pageSize: number = 10;
  totalPages: number = 1;

  // Filtros
  searchTerm: string = '';
  selectedEstado: string = '';
  selectedSedeDesdeId: string = '';
  selectedSedeHaciaId: string = '';
  
  // Custom Product Dropdown
  uniqueProducts: { idProducto: number, sku: string, nombreProducto: string }[] = [];
  selectedProductIds: number[] = [];
  isProductDropdownOpen = false;
  productSearchTerm = '';

  hasSearched: boolean = false;

  // View Details (Modal)
  isViewModalOpen = false;
  selectedViewTransferencia: TransferenciaLog | null = null;
  selectedViewStockResults: any[] = [];
  isLoadingViewStock = false;

  // Print (Modal)
  isPrintModalOpen = false;

  isLoading = false;
  private contextSub!: Subscription;
  EstadoTransferencia = EstadoTransferencia;

  constructor(
    private transferenciaService: TransferenciaService,
    private sedeContextService: SedeContextService,
    private sedeService: SedeService,
    private stockService: StockService,
    private eRef: ElementRef
  ) {}

  @HostListener('document:click', ['$event'])
  clickout(event: Event) {
    if (this.isProductDropdownOpen && !this.eRef.nativeElement.contains(event.target)) {
      this.isProductDropdownOpen = false;
    }
  }

  ngOnInit() {
    this.sedeService.getAll().subscribe(res => this.sedes = res);
    this.cargarHistorico();
  }

  ngOnDestroy() {
    // No subs to clean
  }

  cargarHistorico() {
    this.isLoading = true;
    this.transferenciaService.getAll().subscribe({
      next: (transferencias) => {
        // As it's global, we don't strict-type 'Entrante' or 'Saliente' because each is both depending on perspective.
        // We just cast it to TransferenciaLog for UI compatibility
        const unificadas: TransferenciaLog[] = transferencias.map(t => ({ ...t, tipo: 'Entrante' as const }));
        
        this.allTransferencias = unificadas.sort((a, b) => 
          new Date(b.fechaSolicitud).getTime() - new Date(a.fechaSolicitud).getTime()
        );
        
        // Extract unique products
        const productMap = new Map<number, { idProducto: number, sku: string, nombreProducto: string }>();
        this.allTransferencias.forEach(t => {
          t.detalles.forEach(d => {
            if (!productMap.has(d.idProducto)) {
              productMap.set(d.idProducto, { idProducto: d.idProducto, sku: d.sku, nombreProducto: d.nombreProducto });
            }
          });
        });
        this.uniqueProducts = Array.from(productMap.values()).sort((a, b) => a.nombreProducto.localeCompare(b.nombreProducto));

        this.isLoading = false;
        this.applyFilters();
      },
      error: (err) => {
        console.error(err);
        this.isLoading = false;
      }
    });
  }

  // --- Multi-Select Helpers ---
  toggleProductDropdown(event: Event) {
    event.stopPropagation();
    this.isProductDropdownOpen = !this.isProductDropdownOpen;
  }

  toggleProductSelection(idProducto: number) {
    const idx = this.selectedProductIds.indexOf(idProducto);
    if (idx > -1) {
      this.selectedProductIds.splice(idx, 1);
    } else {
      this.selectedProductIds.push(idProducto);
    }
  }

  get filteredDropdownProducts() {
    if (!this.productSearchTerm) return this.uniqueProducts;
    const term = this.productSearchTerm.toLowerCase();
    return this.uniqueProducts.filter(p => 
      p.nombreProducto.toLowerCase().includes(term) || p.sku.toLowerCase().includes(term)
    );
  }
  // ---------------------------

  applyFilters() {
    this.hasSearched = true;
    
    // Filtrar por término de búsqueda (Tracking, SedeOrigen, SedeDestino)
    let temp = this.allTransferencias;
    
    if (this.searchTerm && this.searchTerm.trim() !== '') {
      const term = this.searchTerm.toLowerCase();
      temp = temp.filter(t => 
        (t.codigoTracking && t.codigoTracking.toLowerCase().includes(term)) ||
        (t.nombreSedeOrigen && t.nombreSedeOrigen.toLowerCase().includes(term)) ||
        (t.nombreSedeDestino && t.nombreSedeDestino.toLowerCase().includes(term))
      );
    }

    if (this.selectedEstado && this.selectedEstado !== '') {
      const estadoNum = parseInt(this.selectedEstado, 10);
      temp = temp.filter(t => t.estado === estadoNum);
    }

    if (this.selectedSedeDesdeId && this.selectedSedeDesdeId !== '') {
      const sedeOrigenId = parseInt(this.selectedSedeDesdeId, 10);
      temp = temp.filter(t => t.idSedeOrigen === sedeOrigenId);
    }

    if (this.selectedSedeHaciaId && this.selectedSedeHaciaId !== '') {
      const sedeDestinoId = parseInt(this.selectedSedeHaciaId, 10);
      temp = temp.filter(t => t.idSedeDestino === sedeDestinoId);
    }

    if (this.selectedProductIds.length > 0) {
      temp = temp.filter(t => 
        this.selectedProductIds.every(selectedId => 
          t.detalles.some(d => d.idProducto === selectedId)
        )
      );
    }

    this.filteredTransferencias = temp;
    this.totalCount = this.filteredTransferencias.length;
    this.totalPages = Math.max(1, Math.ceil(this.totalCount / this.pageSize));
    
    // Reajustar página si se excedió por nuevo filtro
    if (this.page > this.totalPages) {
      this.page = 1;
    }

    this.updatePagination();
  }

  updatePagination() {
    const startIndex = (this.page - 1) * this.pageSize;
    const endIndex = startIndex + this.pageSize;
    this.paginatedTransferencias = this.filteredTransferencias.slice(startIndex, endIndex);
  }

  clearFilters() {
    this.searchTerm = '';
    this.selectedEstado = '';
    this.selectedSedeDesdeId = '';
    this.selectedSedeHaciaId = '';
    this.selectedProductIds = [];
    this.productSearchTerm = '';
    this.hasSearched = false;
    this.page = 1;
    this.cargarHistorico();
  }

  onPageChange(p: number): void {
    this.page = p;
    this.updatePagination();
  }

  onPageSizeChange(size: number): void {
    this.pageSize = size;
    this.page = 1;
    this.applyFilters();
  }

  getEstadoBadgeConfig(estado: EstadoTransferencia): { bg: string, text: string, textCol: string, icon: string } {
    switch(estado) {
      case EstadoTransferencia.Solicitada: return { bg: 'bg-yellow-100', textCol: 'text-yellow-800', text: 'Solicitada', icon: 'fa-regular fa-clock' };
      case EstadoTransferencia.Aprobada: return { bg: 'bg-blue-100', textCol: 'text-blue-800', text: 'Aprobada', icon: 'fa-solid fa-check' };
      case EstadoTransferencia.Completada: return { bg: 'bg-purple-100', textCol: 'text-purple-800', text: 'Completada', icon: 'fa-solid fa-box-open' };
      case EstadoTransferencia.EnTransito: return { bg: 'bg-indigo-100', textCol: 'text-indigo-800', text: 'En Tránsito', icon: 'fa-solid fa-truck-fast' };
      case EstadoTransferencia.Recibida: return { bg: 'bg-emerald-100', textCol: 'text-emerald-800', text: 'Recibida', icon: 'fa-solid fa-check-double' };
      case EstadoTransferencia.Rechazada: return { bg: 'bg-red-100', textCol: 'text-red-800', text: 'Rechazada', icon: 'fa-solid fa-ban' };
      case EstadoTransferencia.Devuelta: return { bg: 'bg-orange-100', textCol: 'text-orange-800', text: 'Devuelta', icon: 'fa-solid fa-rotate-left' };
      case EstadoTransferencia.PendienteDevolucion: return { bg: 'bg-amber-100', textCol: 'text-amber-800', text: 'Pend. Devolución', icon: 'fa-solid fa-clock-rotate-left' };
      default: return { bg: 'bg-gray-100', textCol: 'text-gray-800', text: 'Desconocido', icon: 'fa-solid fa-question' };
    }
  }

  verDetalles(t: TransferenciaLog) {
    this.selectedViewTransferencia = t;
    this.isViewModalOpen = true;
    this.selectedViewStockResults = [];
    this.isLoadingViewStock = true;

    this.stockService.getStockSede('', undefined, undefined, true, undefined, 1, 1000, t.idSedeOrigen).subscribe({
      next: (res) => {
        const list = res.data || res;
        this.selectedViewStockResults = t.detalles.map(detalle => {
           const infoVivo = list.find((s: any) => s.idProducto === detalle.idProducto);
           return {
             idProducto: detalle.idProducto,
             sku: detalle.sku,
             nombreProducto: detalle.nombreProducto,
             cantidadActual: infoVivo ? infoVivo.cantidadActual : 0,
             cantidadPedido: detalle.cantidad,
             stockSnapshot: detalle.stockOrigenSnapshot
           };
        });
        this.isLoadingViewStock = false;
      },
      error: () => {
        this.selectedViewStockResults = t.detalles.map(detalle => ({
           idProducto: detalle.idProducto,
           sku: detalle.sku,
           nombreProducto: detalle.nombreProducto,
           cantidadActual: '?',
           cantidadPedido: detalle.cantidad,
           stockSnapshot: detalle.stockOrigenSnapshot
        }));
        this.isLoadingViewStock = false;
      }
    });
  }

  closeViewModal() {
    this.isViewModalOpen = false;
    this.selectedViewTransferencia = null;
  }

  openPrintModal(t: TransferenciaLog) {
    this.selectedViewTransferencia = t;
    this.isPrintModalOpen = true;
  }

  closePrintModal() {
    this.isPrintModalOpen = false;
    if (!this.isViewModalOpen) {
      this.selectedViewTransferencia = null;
    }
  }

  printVoucher() {
    window.print();
  }
}
