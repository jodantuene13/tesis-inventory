export interface User {
    idUsuario?: number;
    nombreUsuario: string;
    email: string;
    idRol: number;
    idSede?: number;
    nombreRol?: string;
    token?: string;
}
