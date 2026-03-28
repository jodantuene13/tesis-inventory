export interface Stock {
    idStock: number;
    idProducto: number;
    sku: string;
    nombreProducto: string;
    unidadMedida: string;
    rubroProducto: string;
    familiaProducto: string;
    estadoProducto: boolean;
    idSede: number;
    cantidadActual: number;
    puntoReposicion: number;
    conBajoStock: boolean;
    fechaActualizacion: string;
}

export interface Movimiento {
    idMovimiento: number;
    idProducto: number;
    sku: string;
    nombreProducto: string;
    unidadMedida: string;
    rubroProducto: string;
    familiaProducto: string;
    idSede: number;
    tipoMovimiento: number;
    tipoMovimientoDescripcion: string;
    cantidad: number;
    cantidadRestante: number;
    fecha: string;
    motivo: number;
    motivoDescripcion: string;
    idUsuario: number;
    nombreUsuario: string;
    observaciones?: string;
}

export interface IncrementarStockDto {
    idProducto: number;
    cantidad: number;
    motivo: number;
    observaciones?: string;
}

export interface RegistrarConsumoDto {
    idProducto: number;
    cantidad: number;
    motivo: number;
}

export interface RegistrarTransferenciaDto {
    idProducto: number;
    idSedeDestino: number;
    cantidad: number;
    observaciones?: string;
}
