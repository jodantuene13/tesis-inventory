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
}

export interface CreateUserDto {
    nombreUsuario: string;
    email: string;
    password?: string;
    idRol: number;
    idSede: number;
}

export interface UpdateUserDto {
    nombreUsuario: string;
    idRol: number;
    idSede: number;
    estado: boolean;
}
