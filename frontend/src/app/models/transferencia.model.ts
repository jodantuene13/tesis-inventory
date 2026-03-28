export enum EstadoTransferencia {
  Solicitada = 0,
  Aprobada = 1,
  Rechazada = 2,
  Completada = 3,
  EnTransito = 4,
  Recibida = 5,
  PendienteDevolucion = 6,
  Devuelta = 7
}

export enum MotivoTransferencia {
  Definitiva = 0,
  Prestamo = 1
}

export interface Transferencia {
  idTransferencia: number;
  idProducto: number;
  nombreProducto: string;
  idSedeOrigen: number;
  nombreSedeOrigen: string;
  idSedeDestino: number;
  nombreSedeDestino: string;
  cantidad: number;
  stockOrigenSnapshot?: number;
  fechaSolicitud: string;
  estado: EstadoTransferencia;
  motivo: MotivoTransferencia;
  idUsuarioSolicita: number;
  nombreUsuarioSolicita: string;
  observaciones?: string;
}

export interface CreateTransferenciaDto {
  idProducto: number;
  cantidad: number;
  idSedeOrigen: number;
  idSedeDestino: number;
  motivo: MotivoTransferencia;
  observaciones?: string;
}

export interface ResolverTransferenciaDto {
  observaciones?: string;
}
