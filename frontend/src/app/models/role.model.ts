export interface Role {
    idRol: number;
    nombreRol: string;
    descripcion: string;
    permisosIds: number[];
}

export interface Permiso {
    idPermiso: number;
    nombre: string;
    modulo: string;
    descripcion: string;
}
