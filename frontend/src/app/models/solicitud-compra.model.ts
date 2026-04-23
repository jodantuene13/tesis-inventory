export enum EstadoSolicitudCompra {
    Pendiente = 0,
    Aprobada = 1,
    Rechazada = 2
}

export interface SolicitudCompra {
    idSolicitudCompra: number;
    idProducto: number;
    nombreProducto: string;
    skuProducto: string;
    idSede: number;
    nombreSede: string;
    idUsuarioSolicitante: number;
    nombreSolicitante: string;
    idUsuarioAprobador?: number;
    nombreAprobador?: string;
    cantidad: number;
    estado: EstadoSolicitudCompra;
    fechaSolicitud: string;
    fechaDecision?: string;
    observaciones?: string;
    motivoRechazo?: string;
}

export interface CreateSolicitudCompra {
    idProducto: number;
    cantidad: number;
    observaciones?: string;
}

export interface UpdateSolicitudCompraEstado {
    nuevoEstado: EstadoSolicitudCompra;
    motivoRechazo?: string;
}
