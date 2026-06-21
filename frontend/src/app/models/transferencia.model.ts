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

export interface TransferenciaDetalle {
  idTransferenciaDetalle: number;
  idProducto: number;
  nombreProducto: string;
  sku: string;
  cantidad: number;
  stockOrigenSnapshot?: number;
}

export interface Transferencia {
  idTransferencia: number;
  codigoTracking: string;
  idSedeOrigen: number;
  nombreSedeOrigen: string;
  idSedeDestino: number;
  nombreSedeDestino: string;
  fechaSolicitud: string;
  estado: EstadoTransferencia;
  motivo: MotivoTransferencia;
  idUsuarioSolicita: number;
  nombreUsuarioSolicita: string;
  observaciones?: string;
  detalles: TransferenciaDetalle[];
}

export interface CreateTransferenciaDetalleDto {
  idProducto: number;
  cantidad: number;
}

export interface CreateTransferenciaDto {
  idSedeOrigen: number;
  idSedeDestino: number;
  motivo: MotivoTransferencia;
  observaciones?: string;
  detalles: CreateTransferenciaDetalleDto[];
}

export interface ResolverTransferenciaDto {
  observaciones?: string;
}
