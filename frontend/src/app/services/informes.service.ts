import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

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

@Injectable({
  providedIn: 'root'
})
export class InformesService {
  private apiUrl = 'http://localhost:5139/api/Informes';

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
}
