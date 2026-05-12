export enum EstadoSolicitudCompra {
    Pendiente = 0,
    Aprobada = 1,
    Rechazada = 2
}

export enum EtiquetaSolicitudCompra {
    Pendiente = 0,
    ParcialmenteIngresada = 1,
    IngresadaAlStock = 2,
    NoConcretada = 3
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
    etiqueta: EtiquetaSolicitudCompra;
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
    cantidadRecibida?: number;
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
