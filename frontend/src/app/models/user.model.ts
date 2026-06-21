export interface User {
    idUsuario: number;
    nombreUsuario: string;
    email: string;
    estado: boolean;
    fechaRegistro: Date;
    idRol: number;
    nombreRol: string;
    idSede: number;
    nombreSede: string;
    todasLasSedes: boolean;
    limitarOperacionSedePrimaria: boolean;
    permisos?: string[];
    sedesPermitidas: number[];
}

export interface CreateUserDto {
    nombreUsuario: string;
    email: string;
    password?: string;
    idRol: number;
    idSede: number;
    todasLasSedes: boolean;
    limitarOperacionSedePrimaria: boolean;
    sedesIds: number[];
}

export interface UpdateUserDto {
    nombreUsuario: string;
    idRol: number;
    idSede: number;
    estado: boolean;
    todasLasSedes: boolean;
    limitarOperacionSedePrimaria: boolean;
    sedesIds: number[];
}
