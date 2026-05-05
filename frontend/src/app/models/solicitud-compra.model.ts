export enum EstadoSolicitudCompra {
    Pendiente = 0,
    Aprobada = 1,
    Rechazada = 2
}

export interface SolicitudCompra {
    idSolicitudCompra: number;
    idSede: number;
    nombreSede: string;
    idUsuarioSolicitante: number;
    nombreSolicitante: string;
    idUsuarioAprobador?: number;
    nombreAprobador?: string;
    
    motivoSolicitud?: string;
    ordenTrabajo?: string;
    ticketSolicitud?: string;
    tareaARealizar?: string;

    estado: EstadoSolicitudCompra;
    fechaSolicitud: string;
    fechaDecision?: string;
    observaciones?: string;
    motivoRechazo?: string;

    detalles: SolicitudCompraDetalle[];
}

export interface SolicitudCompraDetalle {
    idProducto: number;
    nombreProducto: string;
    skuProducto: string;
    cantidad: number;
}

export interface CreateSolicitudCompra {
    motivoSolicitud?: string;
    ordenTrabajo?: string;
    ticketSolicitud?: string;
    tareaARealizar?: string;
    observaciones?: string;
    detalles: CreateSolicitudCompraDetalle[];
}

export interface CreateSolicitudCompraDetalle {
    idProducto: number;
    cantidad: number;
    nombreProducto?: string;
    skuProducto?: string;
    sku?: string;
}

export interface UpdateSolicitudCompraEstado {
    nuevoEstado: EstadoSolicitudCompra;
    motivoRechazo?: string;
}
