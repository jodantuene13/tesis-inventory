export interface User {
    idUsuario?: number;
    nombreUsuario: string;
    email: string;
    idRol: number;
    idSede?: number;
    nombreRol?: string;
    nombreSede?: string;
    token?: string;
    todasLasSedes?: boolean;
    limitarOperacionSedePrimaria?: boolean;
    permisos?: string[];
    sedesPermitidas?: number[];
}
