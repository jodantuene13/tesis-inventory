import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { StockService } from '../../../services/stock.service';
import { RubroService } from '../../../services/rubro.service';
import { FamiliaService } from '../../../services/familia.service';
import { ProductoService } from '../../../services/producto.service';
import { SedeService } from '../../../services/sede.service';
import { SedeContextService } from '../../../services/sede-context.service';
import { Subscription } from 'rxjs';
import { IncrementarStockDto, RegistrarConsumoDto, RegistrarTransferenciaDto, Stock, OperacionStockMultipleDto } from '../../../models/stock.model';
import { Rubro } from '../../../models/rubro.model';
import { Familia } from '../../../models/familia.model';
import Swal from 'sweetalert2';
import { FichaProductoModalComponent } from '../../../shared/components/ficha-producto-modal/ficha-producto-modal.component';

@Component({
  selector: 'app-stock',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule, FichaProductoModalComponent],
  templateUrl: './stock.html',
  styleUrls: ['./stock.css']
})
export class StockComponent implements OnInit {
  stocks: Stock[] = [];
  rubros: Rubro[] = [];
  familias: Familia[] = [];
  
  private contextSub!: Subscription;

  // Paginación
  totalCount: number = 0;
  page: number = 1;
  pageSize: number = 50;
  totalPages: number = 1;

  // Filtros
  searchTerm: string = '';
  selectedRubroId: number | null = null;
  selectedFamiliaId: number | null = null;
  selectedEstado: string = '';
  selectedBajoStock: string = '';
  hasSearched: boolean = false;

  // Indicators
  indicatorTotal: number = 0;
  indicatorBajoStock: number = 0;

  // Modals state
  activeModal: 'consumo' | 'transferencia' | 'incremento' | 'historial' | 'detalle' | 'multiple' | null = null;
  selectedStock: Stock | null = null;

  // Consumo state
  consumoCantidad: number = 1;
  consumoMotivo: number = 0; // 0=Consumo, 1=Vencimiento, 2=Daño
  consumoConfirm: boolean = false;

  // Incremento state
  incrementoCantidad: number = 1;
  incrementoMotivo: number = 3; // 3=Compra, 4=Ajustes
  incrementoObs: string = '';

  // Historial state
  historial: any[] = [];
  histFiltroTipo: string = '';
  histFiltroDesde: string = '';
  histFiltroHasta: string = '';

  // Operacion Multiple State
  multipleRequest: OperacionStockMultipleDto = {
    tipoOperacion: 1, // Default Egreso (Consumo)
    motivo: 0, // Consumo Interno
    ordenTrabajo: '',
    ordenCompra: '',
    ticketSolicitud: '',
    observaciones: '',
    detalles: []
  };
  isProductModalOpen: boolean = false;
  searchProduct: string = '';
  filteredStock: any[] = [];
  stockList: any[] = [];

  constructor(
    private stockService: StockService,
    private rubroService: RubroService,
    private familiaService: FamiliaService,
    private productoService: ProductoService,
    private sedeContextService: SedeContextService
  ) { }

  ngOnInit(): void {
    this.loadRubros();
    // Suscribirse a cambios de sede para recargar
    this.contextSub = this.sedeContextService.sedeEnContexto$.subscribe(() => {
      this.hasSearched = false;
      this.page = 1;
      this.loadStocks();
      this.loadIndicators();
    });
  }

  ngOnDestroy(): void {
    if (this.contextSub) this.contextSub.unsubscribe();
  }

  loadRubros(): void {
    this.rubroService.getAll(true).subscribe(data => this.rubros = data);
  }

  onRubroChange(): void {
    this.selectedFamiliaId = null;
    this.familias = [];
    if (this.selectedRubroId) {
      this.familiaService.getByRubro(this.selectedRubroId, true).subscribe(data => this.familias = data);
    }
    this.search();
  }

  loadStocks(): void {
    const estadoBool = this.selectedEstado === '' ? undefined : (this.selectedEstado === 'true');
    const bajoStockBool = this.selectedBajoStock === '' ? undefined : (this.selectedBajoStock === 'true');

    this.stockService.getStockSede(
      this.searchTerm || undefined,
      this.selectedRubroId || undefined,
      this.selectedFamiliaId || undefined,
      estadoBool,
      bajoStockBool,
      this.page,
      this.pageSize
    ).subscribe(res => {
      this.stocks = res.data;
      this.totalCount = res.totalCount;
      this.page = res.page;
      this.totalPages = res.totalPages;
    });
  }

  loadIndicators(): void {
    // Traer total de productos activos en la sede
    this.stockService.getStockSede(undefined, undefined, undefined, true, undefined, 1, 1).subscribe(res => {
      this.indicatorTotal = res.totalCount;
    });

    // Traer total de productos con bajo stock (activos)
    this.stockService.getStockSede(undefined, undefined, undefined, true, true, 1, 1).subscribe(res => {
      this.indicatorBajoStock = res.totalCount;
    });
  }

  search(): void {
    this.hasSearched = true;
    this.page = 1;
    this.loadStocks();
  }

  clearFilters(): void {
    this.searchTerm = '';
    this.selectedRubroId = null;
    this.selectedFamiliaId = null;
    this.selectedEstado = '';
    this.selectedBajoStock = '';
    this.familias = [];
    this.hasSearched = false;
    this.page = 1;
    this.loadStocks();
  }

  filterByBajoStock(): void {
    this.clearFilters();
    this.selectedBajoStock = 'true';
    this.search();
  }

  filterAll(): void {
    this.clearFilters();
  }

  nextPage(): void {
    if (this.page < this.totalPages) {
      this.page++;
      this.loadStocks();
    }
  }

  prevPage(): void {
    if (this.page > 1) {
      this.page--;
      this.loadStocks();
    }
  }

  // Modals hooks
  closeModal(): void {
    this.activeModal = null;
    this.selectedStock = null;
    this.consumoConfirm = false;
    this.isProductModalOpen = false;
  }

  openConsumoModal(stock: Stock): void {
    this.selectedStock = stock;
    this.consumoCantidad = 1;
    this.consumoMotivo = 0;
    this.consumoConfirm = false;
    this.activeModal = 'consumo';
  }

  confirmConsumo(): void {
    if (!this.selectedStock) return;

    if (!this.consumoConfirm) {
      this.consumoConfirm = true;
      return;
    }

    const dto: RegistrarConsumoDto = {
      idProducto: this.selectedStock.idProducto,
      cantidad: this.consumoCantidad,
      motivo: this.consumoMotivo
    };

    this.stockService.registrarConsumo(dto).subscribe({
      next: () => {
        this.closeModal();
        this.loadStocks();
        this.loadIndicators();
      },
      error: (err) => alert(err.error?.message || 'Error al registrar consumo')
    });
  }

  openIncrementoModal(stock: Stock): void {
    this.selectedStock = stock;
    this.incrementoCantidad = 1;
    this.incrementoMotivo = 3;
    this.incrementoObs = '';
    this.activeModal = 'incremento';
  }

  saveIncremento(): void {
    if (!this.selectedStock) return;

    const dto: IncrementarStockDto = {
      idProducto: this.selectedStock.idProducto,
      cantidad: this.incrementoCantidad,
      motivo: this.incrementoMotivo,
      observaciones: this.incrementoObs
    };

    this.stockService.incrementarStock(dto).subscribe({
      next: () => {
        this.closeModal();
        this.loadStocks();
        this.loadIndicators();
      },
      error: (err) => alert(err.error?.message || 'Error al incrementar stock')
    });
  }

  openHistorialModal(stock: Stock): void {
    this.selectedStock = stock;
    this.activeModal = 'historial';
    this.histFiltroTipo = '';
    this.histFiltroDesde = '';
    this.histFiltroHasta = '';
    this.loadHistorial();
  }

  loadHistorial(): void {
    if (!this.selectedStock) return;
    this.stockService.getMovimientos(this.selectedStock.idProducto, this.histFiltroTipo || undefined, this.histFiltroDesde || undefined, this.histFiltroHasta || undefined)
      .subscribe({
        next: (data) => this.historial = data.slice(0, 6),
        error: (err) => alert('Error al cargar historial')
      });
  }

  openDetalleModal(stock: Stock): void {
    this.selectedStock = stock;
    this.activeModal = 'detalle';
    // Lógica de atributos puede ir aquí
  }

  deleteStock(idStock: number, event: Event): void {
    event.stopPropagation();
    const stockItem = this.stocks.find(s => s.idStock === idStock);
    if (!stockItem) return;

    Swal.fire({
      title: 'Validación de Seguridad',
      html: `Para dar de baja este producto, escriba textualmente su nombre:<br/><br/><b>${stockItem.nombreProducto}</b>`,
      input: 'text',
      icon: 'warning',
      showCancelButton: true,
      confirmButtonText: 'Dar de baja',
      cancelButtonText: 'Cancelar',
      preConfirm: (inputValue) => {
        if (inputValue !== stockItem.nombreProducto) {
          Swal.showValidationMessage('El nombre no coincide.');
          return false;
        }
        return true;
      }
    }).then((result) => {
      if (result.isConfirmed) {
        this.productoService.delete(stockItem.idProducto, stockItem.nombreProducto).subscribe({
          next: () => {
            this.loadStocks();
            this.loadIndicators();
            Swal.fire('Baja Exitosa', 'El producto ha sido dado de baja (Lógica).', 'success');
          },
          error: (err) => Swal.fire('Error', err.error?.message || 'Error al procesar la baja', 'error')
        });
      }
    });
  }

  // Multi-operation Logic
  openMultipleModal(): void {
    this.resetMultipleForm();
    this.stockList = this.stocks.map(s => ({ ...s })); // Copia rápida de los filtrados
    this.filteredStock = [...this.stockList];
    this.activeModal = 'multiple';
  }

  resetMultipleForm(): void {
    this.multipleRequest = {
      tipoOperacion: 1, // DEFAULT: Egreso
      motivo: 0,
      ordenTrabajo: '',
      ordenCompra: '',
      ticketSolicitud: '',
      observaciones: '',
      detalles: []
    };
    this.searchProduct = '';
  }

  onTipoOperacionChange(): void {
    this.multipleRequest.tipoOperacion = Number(this.multipleRequest.tipoOperacion);
    if (this.multipleRequest.tipoOperacion === 0) { // Ingreso
      this.multipleRequest.motivo = 3; // Por Compra
      this.multipleRequest.ordenTrabajo = '';
    } else { // Egreso
      this.multipleRequest.motivo = 0; // Consumo Interno
      this.multipleRequest.ordenCompra = '';
    }
  }

  openProductModal(): void {
    this.isProductModalOpen = true;
    this.searchProduct = '';
    // Recargar stockList con todo o lo actual
    this.stockService.getStockSede('', undefined, undefined, true, undefined, 1, 1000).subscribe(res => {
        this.stockList = res.data;
        this.filteredStock = [...this.stockList];
    });
  }

  closeProductModal(): void {
    this.isProductModalOpen = false;
  }

  onSearchProduct(): void {
    const term = this.searchProduct.toLowerCase();
    this.filteredStock = this.stockList.filter(s => 
      s.nombreProducto?.toLowerCase().includes(term) || 
      s.sku?.toLowerCase().includes(term)
    );
  }

  selectProduct(stockItem: any): void {
    const alreadyExists = this.multipleRequest.detalles.find(d => d.idProducto === stockItem.idProducto);
    if (alreadyExists) {
      alreadyExists.cantidad += 1;
    } else {
      this.multipleRequest.detalles.push({
        idProducto: stockItem.idProducto,
        cantidad: 1,
        productoInfo: stockItem
      });
    }
    
    this.searchProduct = '';
    this.isProductModalOpen = false;
  }

  removeDetalle(index: number): void {
    this.multipleRequest.detalles.splice(index, 1);
  }

  submitMultipleRequest(): void {
    if (this.multipleRequest.detalles.length === 0) {
      alert('Debe agregar al menos un producto.');
      return;
    }

    const payload = {
      tipoOperacion: Number(this.multipleRequest.tipoOperacion),
      motivo: Number(this.multipleRequest.motivo),
      ordenTrabajo: this.multipleRequest.ordenTrabajo,
      ordenCompra: this.multipleRequest.ordenCompra,
      ticketSolicitud: this.multipleRequest.ticketSolicitud,
      observaciones: this.multipleRequest.observaciones,
      detalles: this.multipleRequest.detalles.map(d => ({
        idProducto: d.idProducto,
        cantidad: d.cantidad
      }))
    };

    this.stockService.procesarOperacionMultiple(payload).subscribe({
      next: (res) => {
        alert(res.message || 'Operación procesada con éxito, ID Operación vinculada.');
        this.closeModal();
        this.loadStocks();
        this.loadIndicators();
      },
      error: (err) => {
        alert(err.error?.message || 'Error al procesar la operación múltiple');
      }
    });
  }
}

