import { Component, Input, Output, EventEmitter, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ProductoService } from '../../../services/producto.service';

@Component({
  selector: 'app-ficha-producto-modal',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './ficha-producto-modal.component.html',
  styleUrls: []
})
export class FichaProductoModalComponent implements OnInit {
  @Input() stock: any; // Se acepta 'any' para soportar los distintos formatos (model Stock vs listado dropdown) que manejan la info central.
  @Output() close = new EventEmitter<void>();

  isLoadingAtributos: boolean = true;

  constructor(private productoService: ProductoService) {}

  ngOnInit(): void {
    if (this.stock && this.stock.idProducto) {
      this.productoService.getById(this.stock.idProducto).subscribe({
        next: (prod) => {
          this.stock.atributos = prod.atributos || [];
          this.stock.estadoProducto = prod.activo;
          this.stock.familiaProducto = prod.nombreFamilia || this.stock.familiaProducto;
          this.isLoadingAtributos = false;
        },
        error: () => {
          this.isLoadingAtributos = false;
        }
      });
    } else {
      this.isLoadingAtributos = false;
    }
  }

  closeModal(): void {
    this.close.emit();
  }
}
