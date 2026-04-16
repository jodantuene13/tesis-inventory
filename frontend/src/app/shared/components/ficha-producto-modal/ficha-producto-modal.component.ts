import { Component, Input, Output, EventEmitter } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-ficha-producto-modal',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './ficha-producto-modal.component.html',
  styleUrls: []
})
export class FichaProductoModalComponent {
  @Input() stock: any; // Se acepta 'any' para soportar los distintos formatos (model Stock vs listado dropdown) que manejan la info central.
  @Output() close = new EventEmitter<void>();

  closeModal(): void {
    this.close.emit();
  }
}
