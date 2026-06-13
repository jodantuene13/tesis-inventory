import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { API_BASE_URL } from '../tokens/api-url.token';
import { Observable } from 'rxjs';

// ── Stock bajo y alertas ──────────────────────────────────────────────────────

export interface ProductoAlertaStockDto {
  idAlertaStock: number;
  idProducto: number;
  producto: string;
  familia: string;
  sede: string;
  stockActual: number;
  stockMinimo: number;
  diferencia: number;
  diasEnAlerta: number;
  ultimaAlerta: string;
  criticidad: 'Alta' | 'Media' | 'Baja';
}

export interface ProductoRecurrenciaDto {
  idProducto: number;
  producto: string;
  familia: string;
  sede: string;
  cantidadAlertas: number;
  diasAcumulados: number;
  stockActual: number;
  stockMinimo: number;
  ultimaAlerta: string;
  estadoActual: string;
  criticidad: 'Alta' | 'Media' | 'Baja';
}

export interface EvolucionSemanalDto {
  semana: string;
  alertas: number;
}

export interface InformeAlertasStockDto {
  bajoStock: ProductoAlertaStockDto[];
  recurrencia: ProductoRecurrenciaDto[];
  evolucionSemanal: EvolucionSemanalDto[];
}

// ── Interfaces: Rotación de Productos ─────────────────────────────────────────

export interface ProductoRotacionDto {
  idProducto: number;
  producto: string;
  sku: string;
  familia: string;
  sede: string;
  totalIngresos: number;
  totalEgresos: number;
  stockPromedioPonderado: number;
  indiceRotacion: number;
  tendencia: string;
  ultimoIngreso?: string;
  ultimoEgreso?: string;
}

export interface ProductoMovimientoDto {
  idProducto: number;
  producto: string;
  sku: string;
  familia: string;
  sede: string;
  totalUnidades: number;
  cantidadOperaciones: number;
  ultimaFecha: string;
}

export interface InformeRotacionDto {
  rotacion: ProductoRotacionDto[];
  mayorIngreso: ProductoMovimientoDto[];
  mayorEgreso: ProductoMovimientoDto[];
  fechaDesde: string;
  fechaHasta: string;
  totalDiasPeriodo: number;
  rotacionPromedio: number;
  saldoNetoTotal: number;
}

// ── Interfaces: Transferencias y Préstamos ────────────────────────────────────

export interface KpiTransferenciaDto {
  totalTransferencias: number;
  totalPrestamos: number;
  porcentajePrestamos: number;
  tasaRechazo: number;
  tiempoPromedioPrestamoDias: number;
  sedeMasActiva: string;
  sedeMasActivaCantidad: number;
}

export interface TransferenciaHabitualDto {
  nombre: string;
  sku?: string;
  familia?: string;
  cantidad: number;
  porcentaje: number;
}

export interface PrestamoPorDiaDto {
  fecha: string;
  cantidadActivos: number;
}

export interface PrestamoActivoDto {
  idTransferencia: number;
  productos: string;
  sedeOrigen: string;
  sedeDestino: string;
  fechaSolicitud: string;
  fechaDevolucionEsperada?: string;
  diasTranscurridos: number;
  estado: string;
}

export interface ProductoRechazoDto {
  producto: string;
  sku: string;
  familia: string;
  rechazadas: number;
  total: number;
  indice: number;
  motivoPrincipal: string;
}

export interface TransferenciaDetalleDto {
  idTransferencia: number;
  fechaSolicitud: string;
  tipo: string;
  productos: string;
  sedeOrigen: string;
  sedeDestino: string;
  cantidadTotal: number;
  estado: string;
  usuario: string;
}

export interface InformeTransferenciaDto {
  kpis: KpiTransferenciaDto;
  habitualesPorSede: TransferenciaHabitualDto[];
  habitualesPorProducto: TransferenciaHabitualDto[];
  prestamosPorDia: PrestamoPorDiaDto[];
  prestamosActivos: PrestamoActivoDto[];
  rechazosPorProducto: ProductoRechazoDto[];
  detalleMovimientos: TransferenciaDetalleDto[];
}

// ── Service ───────────────────────────────────────────────────────────────────

@Injectable({
  providedIn: 'root'
})
export class InformesService {
  private readonly apiUrl = `${inject(API_BASE_URL)}/api/Informes`;

  constructor(private http: HttpClient) {}

  getAlertasStock(
    idSede?: number,
    idFamilia?: number,
    semanas: number = 5
  ): Observable<InformeAlertasStockDto> {
    let params = new HttpParams().set('semanas', semanas.toString());

    if (idSede !== undefined && idSede !== null)
      params = params.set('idSede', idSede.toString());

    if (idFamilia !== undefined && idFamilia !== null)
      params = params.set('idFamilia', idFamilia.toString());

    return this.http.get<InformeAlertasStockDto>(`${this.apiUrl}/alertas-stock`, { params });
  }

  getRotacionProductos(
    idSede?: number,
    idFamilia?: number,
    fechaDesde?: string,
    fechaHasta?: string
  ): Observable<InformeRotacionDto> {
    let params = new HttpParams();
    if (idSede) params = params.set('idSede', idSede.toString());
    if (idFamilia) params = params.set('idFamilia', idFamilia.toString());
    if (fechaDesde) params = params.set('fechaDesde', fechaDesde);
    if (fechaHasta) params = params.set('fechaHasta', fechaHasta);

    return this.http.get<InformeRotacionDto>(`${this.apiUrl}/rotacion-productos`, { params });
  }

  getInformeTransferencias(
    idSedeOrigen?: number,
    idSedeDestino?: number,
    idFamilia?: number,
    motivo?: number,
    estado?: number,
    fechaDesde?: string,
    fechaHasta?: string,
    topN: number = 10
  ): Observable<InformeTransferenciaDto> {
    let params = new HttpParams();
    if (idSedeOrigen) params = params.set('idSedeOrigen', idSedeOrigen.toString());
    if (idSedeDestino) params = params.set('idSedeDestino', idSedeDestino.toString());
    if (idFamilia) params = params.set('idFamilia', idFamilia.toString());
    if (motivo !== undefined && motivo !== null) params = params.set('motivo', motivo.toString());
    if (estado !== undefined && estado !== null) params = params.set('estado', estado.toString());
    if (fechaDesde) params = params.set('fechaDesde', fechaDesde);
    if (fechaHasta) params = params.set('fechaHasta', fechaHasta);
    params = params.set('topN', topN.toString());

    return this.http.get<InformeTransferenciaDto>(`${this.apiUrl}/transferencias`, { params });
  }
}
