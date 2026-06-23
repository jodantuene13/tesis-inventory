export interface GrupoAtributoItem {
    idGrupoAtributoItem: number;
    idGrupoAtributo: number;
    idAtributo: number;
    nombreAtributo: string;
    tipoDatoAtributo: string;
    idUnidadMedida?: number;
    simboloUnidad?: string;
    orden: number;
    activo: boolean;
}

export interface GrupoAtributo {
    idGrupoAtributo: number;
    codigoGrupo: string;
    nombre: string;
    separador: string;
    unidadSufijo?: string;
    activo: boolean;
    fechaCreacion: string;
    fechaActualizacion: string;
    items: GrupoAtributoItem[];
}

export interface CreateGrupoAtributo {
    codigoGrupo: string;
    nombre: string;
    separador: string;
    unidadSufijo?: string;
    activo: boolean;
}

export interface UpdateGrupoAtributo {
    codigoGrupo: string;
    nombre: string;
    separador: string;
    unidadSufijo?: string;
    activo: boolean;
}

export interface AddItemToGrupo {
    idAtributo: number;
    orden: number;
    idUnidadMedida?: number;
}

export interface FamiliaGrupoAtributo {
    idFamiliaGrupoAtributo: number;
    idFamilia: number;
    nombreFamilia: string;
    idGrupoAtributo: number;
    nombreGrupo: string;
    separador: string;
    unidadSufijo?: string;
    obligatorio: boolean;
    activo: boolean;
    items: GrupoAtributoItem[];
}

export interface CreateFamiliaGrupoAtributo {
    idGrupoAtributo: number;
    obligatorio: boolean;
    activo: boolean;
}
