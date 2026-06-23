export interface Producto {
    idProducto: number;
    idFamilia: number;
    idRubro?: number;
    nombreFamilia: string;
    nombreRubro?: string;
    sku: string;
    nombre: string;
    unidadMedida: string;
    activo: boolean;
    puntoReposicion: number;
    atributos: ProductoAtributoValor[];
}

export interface ProductoAtributoValor {
    idAtributo: number;
    codigoAtributo: string;
    nombreAtributo: string;
    tipoDatoAtributo: string;
    idUnidadMedida?: number;
    simboloUnidad?: string;
    unidadesPermitidas: { idUnidadMedida: number; simbolo: string; nombre: string }[];
    valorTexto?: string;
    valorNumero?: number;
    valorDecimal?: number;
    valorBool?: boolean;
    valorLista?: string;
}

export interface CreateProducto {
    idRubro: number;
    idFamilia: number;
    nombre: string;
    unidadMedida: string;
    activo: boolean;
    puntoReposicion: number;
    atributos: CreateProductoAtributoValor[];
}

export interface UpdateProducto {
    nombre: string;
    unidadMedida: string;
    activo: boolean;
    puntoReposicion: number;
    atributos: CreateProductoAtributoValor[];
}

export interface CreateProductoAtributoValor {
    idAtributo: number;
    idUnidadMedida?: number;
    valorTexto?: string;
    valorNumero?: number;
    valorDecimal?: number;
    valorBool?: boolean;
    valorLista?: string;
}
