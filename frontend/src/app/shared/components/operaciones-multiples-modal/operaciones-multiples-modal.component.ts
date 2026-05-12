import { Component, EventEmitter, Input, Output, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { StockService } from '../../../services/stock.service';
import { SedeContextService } from '../../../services/sede-context.service';
import { OperacionStockMultipleDto, Stock } from '../../../models/stock.model';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-operaciones-multiples-modal',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './operaciones-multiples-modal.html'
})
export class OperacionesMultiplesModalComponent implements OnInit {
  @Input() config: {
    isOpen: boolean;
    isLocked: boolean; // if true, disables adding new products and changing tipo/motivo
    initialRequest: OperacionStockMultipleDto;
    requiereOC: boolean;
  } = {
    isOpen: false,
    isLocked: false,
    requiereOC: false,
    initialRequest: {
      tipoOperacion: 1,
      motivo: 0,
      ordenTrabajo: '',
      ordenCompra: '',
      ticketSolicitud: '',
      observaciones: '',
      detalles: []
    }
  };

  @Output() close = new EventEmitter<void>();
  @Output() onComplete = new EventEmitter<void>();

  isProductModalOpen: boolean = false;
  searchProduct: string = '';
  filteredStock: any[] = [];
  stockList: any[] = [];

  constructor(
    private stockService: StockService,
    private sedeContextService: SedeContextService
  ) { }

  ngOnInit(): void {
  }

  onTipoOperacionChange(): void {
    if (this.config.isLocked) return;

    this.config.initialRequest.tipoOperacion = Number(this.config.initialRequest.tipoOperacion);
    this.config.requiereOC = false;
    if (this.config.initialRequest.tipoOperacion === 0) { // Ingreso
      this.config.initialRequest.motivo = 3; // Por Compra
      this.config.initialRequest.ordenTrabajo = '';
    } else { // Egreso
      this.config.initialRequest.motivo = 0; // Consumo Interno
      this.config.initialRequest.ordenCompra = '';
    }
  }

  onMotivoChange(): void {
    if (this.config.isLocked) return;

    this.config.initialRequest.motivo = Number(this.config.initialRequest.motivo);
    if (this.config.initialRequest.motivo !== 3) {
      this.config.requiereOC = false;
      this.config.initialRequest.ordenCompra = '';
    }
  }

  openProductModal(): void {
    if (this.config.isLocked) return;
    this.isProductModalOpen = true;
    this.searchProduct = '';
    this.stockService.getStockSede('', undefined, undefined, true, undefined, 1, 1000).subscribe({
      next: (res: any) => {
        this.stockList = res.data;
        this.filteredStock = [...this.stockList];
      },
      error: (err: any) => console.error(err)
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
    if (this.config.isLocked) return;
    
    const alreadyExists = this.config.initialRequest.detalles.find((d: any) => d.idProducto === stockItem.idProducto);
    if (alreadyExists) {
      alreadyExists.cantidad += 1;
    } else {
      this.config.initialRequest.detalles.push({
        idProducto: stockItem.idProducto,
        cantidad: 1,
        productoInfo: stockItem
      });
    }
    
    this.searchProduct = '';
    this.isProductModalOpen = false;
  }

  removeDetalle(index: number): void {
    this.config.initialRequest.detalles.splice(index, 1);
  }

  enforceMaxQuantity(index: number): void {
    const d = this.config.initialRequest.detalles[index];
    if (d.maxCantidad !== undefined && d.cantidad > d.maxCantidad) {
      d.cantidad = d.maxCantidad;
      Swal.fire('Atención', `La cantidad máxima permitida para este producto es ${d.maxCantidad}.`, 'warning');
    }
  }

  submitMultipleRequest(): void {
    if (this.config.initialRequest.detalles.length === 0) {
      Swal.fire('Atención', 'Debe agregar al menos un producto con cantidad válida.', 'warning');
      return;
    }

    const detallesValidos = this.config.initialRequest.detalles.filter((d: any) => d.cantidad > 0);
    if (detallesValidos.length === 0) {
      Swal.fire('Atención', 'La cantidad a impactar debe ser mayor a 0.', 'warning');
      return;
    }

    const payload = {
      tipoOperacion: Number(this.config.initialRequest.tipoOperacion),
      motivo: Number(this.config.initialRequest.motivo),
      ordenTrabajo: this.config.initialRequest.ordenTrabajo,
      ordenCompra: this.config.initialRequest.ordenCompra,
      ticketSolicitud: this.config.initialRequest.ticketSolicitud,
      observaciones: this.config.initialRequest.observaciones,
      idSolicitudCompraAsociada: this.config.initialRequest.idSolicitudCompraAsociada,
      detalles: detallesValidos.map((d: any) => ({
        idProducto: d.idProducto,
        cantidad: d.cantidad
      }))
    };

    this.stockService.procesarOperacionMultiple(payload).subscribe({
      next: (res: any) => {
        this.close.emit();
        this.onComplete.emit();
        Swal.fire('Éxito', 'Operación registrada con éxito. Puede ver el remito en Operaciones Múltiples.', 'success');
      },
      error: (err: any) => {
        Swal.fire('Error', err.error?.message || 'Error al procesar la operación múltiple', 'error');
      }
    });
  }

  closeModal(): void {
    this.close.emit();
  }
}
