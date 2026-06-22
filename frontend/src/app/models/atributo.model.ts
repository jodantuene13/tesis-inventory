import { UnidadMedida } from './unidad-medida.model';

export interface Atributo {
    idAtributo: number;
    codigoAtributo: string;
    nombre: string;
    tipoDato: string;
    unidadesMedida: UnidadMedida[];
    descripcion?: string;
    activo: boolean;
}

export interface CreateAtributo {
    codigoAtributo: string;
    nombre: string;
    tipoDato: string;
    idsUnidadesMedida: number[];
    descripcion?: string;
    activo: boolean;
}

export interface UpdateAtributo {
    codigoAtributo: string;
    nombre: string;
    tipoDato: string;
    idsUnidadesMedida: number[];
    descripcion?: string;
    activo: boolean;
}

export interface AtributoOpcion {
    idAtributoOpcion: number;
    idAtributo: number;
    codigoOpcion: string;
    valor: string;
    activo: boolean;
}

export interface CreateAtributoOpcion {
    codigoOpcion: string;
    valor: string;
    activo: boolean;
}

export interface FamiliaAtributo {
    idFamiliaAtributo: number;
    idFamilia: number;
    nombreFamilia: string;
    idAtributo: number;
    nombreAtributo: string;
    tipoDatoAtributo: string;
    obligatorio: boolean;
    activo: boolean;
    unidadesMedida: UnidadMedida[];
}

export interface CreateFamiliaAtributo {
    idAtributo: number;
    obligatorio: boolean;
    activo: boolean;
}
