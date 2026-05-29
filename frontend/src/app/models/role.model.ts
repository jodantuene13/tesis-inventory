export interface Role {
    idRol: number;
    nombreRol: string;
    descripcion: string;
    todasLasSedes: boolean;
    limitarOperacionSedePrimaria: boolean;
    permisosIds: number[];
    sedesIds: number[];
}

export interface Permiso {
    idPermiso: number;
    nombre: string;
    modulo: string;
    descripcion: string;
}
