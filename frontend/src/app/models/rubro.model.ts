export interface Rubro {
    idRubro: number;
    codigoRubro: string;
    nombre: string;
    activo: boolean;
}

export interface CreateRubro {
    codigoRubro: string;
    nombre: string;
    activo: boolean;
}

export interface UpdateRubro {
    codigoRubro: string;
    nombre: string;
    activo: boolean;
    activarFamilias?: boolean;
}
