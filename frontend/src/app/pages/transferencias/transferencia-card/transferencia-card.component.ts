import { Component, EventEmitter, Input, Output } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Transferencia, EstadoTransferencia, MotivoTransferencia } from '../../../models/transferencia.model';

@Component({
  selector: 'app-transferencia-card',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './transferencia-card.component.html',
  styleUrls: []
})
export class TransferenciaCardComponent {
  @Input() transferencia!: Transferencia;
  @Input() tipoVisita: 'Entrante' | 'Saliente' = 'Entrante';
  @Input() isCreatorSede: boolean = false; // helps to know if they can Accept or Confirm

  @Output() onAceptar = new EventEmitter<Transferencia>();
  @Output() onRechazar = new EventEmitter<Transferencia>();
  @Output() onConfirmarRecepcion = new EventEmitter<Transferencia>();
  @Output() onDevolver = new EventEmitter<Transferencia>();
  @Output() onVerDetalles = new EventEmitter<Transferencia>();

  get estadoLabel(): string {
    switch (this.transferencia.estado) {
      case EstadoTransferencia.Solicitada: return 'Solicitada';
      case EstadoTransferencia.Aprobada: return 'Aprobada';
      case EstadoTransferencia.Rechazada: return 'Rechazada';
      case EstadoTransferencia.Completada: return 'Completada';
      case EstadoTransferencia.EnTransito: return 'En Tránsito';
      case EstadoTransferencia.Recibida: return 'Recibida';
      case EstadoTransferencia.PendienteDevolucion: return 'Pendiente Devolución';
      case EstadoTransferencia.Devuelta: return 'Devuelta';
      default: return 'Desconocido';
    }
  }

  get estadoColor(): string {
    switch (this.transferencia.estado) {
      case EstadoTransferencia.Solicitada: return 'bg-yellow-100 text-yellow-800 border-yellow-200';
      case EstadoTransferencia.Rechazada: return 'bg-red-100 text-red-800 border-red-200';
      case EstadoTransferencia.EnTransito: return 'bg-blue-100 text-blue-800 border-blue-200';
      case EstadoTransferencia.Recibida: 
      case EstadoTransferencia.Completada: 
      case EstadoTransferencia.Devuelta: return 'bg-green-100 text-green-800 border-green-200';
      case EstadoTransferencia.PendienteDevolucion: return 'bg-purple-100 text-purple-800 border-purple-200';
      default: return 'bg-gray-100 text-gray-800 border-gray-200';
    }
  }

  get puedeAceptarORechazar(): boolean {
    return this.tipoVisita === 'Entrante' && this.transferencia.estado === EstadoTransferencia.Solicitada;
  }

  get puedeConfirmarRecepcion(): boolean {
    return this.tipoVisita === 'Saliente' && this.transferencia.estado === EstadoTransferencia.EnTransito;
  }

  get puedeDevolver(): boolean {
    // Only the one who received it can return it. Which means Saliente view (because they requested it), state PendienteDevolucion
    return this.tipoVisita === 'Saliente' && this.transferencia.estado === EstadoTransferencia.PendienteDevolucion;
  }
}
