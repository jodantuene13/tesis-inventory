import { Component, OnInit, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TransferenciaService } from '../../../services/transferencia.service';
import { SedeContextService } from '../../../services/sede-context.service';
import { Transferencia, EstadoTransferencia } from '../../../models/transferencia.model';
import { forkJoin, Subscription } from 'rxjs';

export interface TransferenciaLog extends Transferencia {
  tipo: 'Entrante' | 'Saliente';
}

@Component({
  selector: 'app-historico-transferencias',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './historico-transferencias.component.html',
  styleUrls: []
})
export class HistoricoTransferenciasComponent implements OnInit, OnDestroy {
  transferencias: TransferenciaLog[] = [];
  isLoading = false;
  private contextSub!: Subscription;
  EstadoTransferencia = EstadoTransferencia;

  constructor(
    private transferenciaService: TransferenciaService,
    private sedeContextService: SedeContextService
  ) {}

  ngOnInit() {
    this.contextSub = this.sedeContextService.sedeEnContexto$.subscribe(() => {
      this.cargarHistorico();
    });
  }

  ngOnDestroy() {
    if (this.contextSub) this.contextSub.unsubscribe();
  }

  cargarHistorico() {
    this.isLoading = true;
    forkJoin({
      entrantes: this.transferenciaService.getEntrantes(),
      salientes: this.transferenciaService.getSalientes()
    }).subscribe({
      next: ({ entrantes, salientes }) => {
        // Combinar, marcar tipo y ordenar por fecha descendente
        const unificadas: TransferenciaLog[] = [
          ...entrantes.map(t => ({ ...t, tipo: 'Entrante' as const })),
          ...salientes.map(t => ({ ...t, tipo: 'Saliente' as const }))
        ];
        
        // Ordenar del más reciente al más antiguo
        this.transferencias = unificadas.sort((a, b) => 
          new Date(b.fechaSolicitud).getTime() - new Date(a.fechaSolicitud).getTime()
        );
        this.isLoading = false;
      },
      error: (err) => {
        console.error(err);
        this.isLoading = false;
      }
    });
  }

  getEstadoBadgeConfig(estado: EstadoTransferencia): { bg: string, text: string, textCol: string, icon: string } {
    switch(estado) {
      case EstadoTransferencia.Solicitada: return { bg: 'bg-yellow-100', textCol: 'text-yellow-800', text: 'Solicitada', icon: 'fa-regular fa-clock' };
      case EstadoTransferencia.EnTransito: return { bg: 'bg-blue-100', textCol: 'text-blue-800', text: 'En Tránsito', icon: 'fa-solid fa-truck-fast' };
      case EstadoTransferencia.Recibida: return { bg: 'bg-green-100', textCol: 'text-green-800', text: 'Recibida', icon: 'fa-solid fa-check-double' };
      case EstadoTransferencia.PendienteDevolucion: return { bg: 'bg-indigo-100', textCol: 'text-indigo-800', text: 'Pend. Devolución', icon: 'fa-solid fa-hand-holding-hand' };
      case EstadoTransferencia.Devuelta: return { bg: 'bg-purple-100', textCol: 'text-purple-800', text: 'Devuelta', icon: 'fa-solid fa-rotate-left' };
      case EstadoTransferencia.Rechazada: return { bg: 'bg-red-100', textCol: 'text-red-800', text: 'Rechazada', icon: 'fa-solid fa-xmark' };
      default: return { bg: 'bg-gray-100', textCol: 'text-gray-800', text: 'Desconocido', icon: 'fa-solid fa-question' };
    }
  }
}
