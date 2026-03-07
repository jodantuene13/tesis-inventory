export interface Producto {
    idProducto: number;
    idFamilia: number;
    nombreFamilia: string;
    sku: string;
    nombre: string;
    unidadMedida: string;
    activo: boolean;
    atributos: ProductoAtributoValor[];
}

export interface ProductoAtributoValor {
    idAtributo: number;
    codigoAtributo: string;
    nombreAtributo: string;
    tipoDatoAtributo: string;
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
    atributos: CreateProductoAtributoValor[];
}

export interface UpdateProducto {
    nombre: string;
    unidadMedida: string;
    activo: boolean;
    atributos: CreateProductoAtributoValor[];
}

export interface CreateProductoAtributoValor {
    idAtributo: number;
    valorTexto?: string;
    valorNumero?: number;
    valorDecimal?: number;
    valorBool?: boolean;
    valorLista?: string;
}
