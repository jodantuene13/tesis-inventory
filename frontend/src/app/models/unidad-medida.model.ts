export interface UnidadMedida {
    idUnidadMedida: number;
    simbolo: string;
    nombre: string;
    activo: boolean;
}

export interface CreateUnidadMedida {
    simbolo: string;
    nombre: string;
    activo: boolean;
}

export interface UpdateUnidadMedida {
    simbolo: string;
    nombre: string;
    activo: boolean;
}
