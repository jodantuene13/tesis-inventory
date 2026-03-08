export interface Familia {
    idFamilia: number;
    idRubro: number;
    nombreRubro: string;
    codigoFamilia: string;
    nombre: string;
    activo: boolean;
}

export interface CreateFamilia {
    idRubro: number;
    codigoFamilia: string;
    nombre: string;
    activo: boolean;
}

export interface UpdateFamilia {
    idRubro: number;
    codigoFamilia: string;
    nombre: string;
    activo: boolean;
}

export interface FamiliaAsociaciones {
    productos: string[];
    atributos: string[];
}
