import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { API_BASE_URL } from '../tokens/api-url.token';
import { Observable } from 'rxjs';
import { SolicitudCompra, CreateSolicitudCompra, UpdateSolicitudCompraEstado } from '../models/solicitud-compra.model';

@Injectable({
  providedIn: 'root'
})
export class SolicitudCompraService {
  private readonly apiUrl = `${inject(API_BASE_URL)}/api/SolicitudesCompra`;

  constructor(private http: HttpClient) { }

  getAll(search?: string, estado?: number, page: number = 1, pageSize: number = 10): Observable<any> {
    let params = new HttpParams()
      .set('page', page.toString())
      .set('pageSize', pageSize.toString());

    if (search) params = params.set('search', search);
    if (estado !== undefined && estado !== null) params = params.set('estado', estado.toString());

    return this.http.get<any>(this.apiUrl, { params });
  }

  getById(id: number): Observable<SolicitudCompra> {
    return this.http.get<SolicitudCompra>(`${this.apiUrl}/${id}`);
  }

  create(dto: CreateSolicitudCompra): Observable<SolicitudCompra> {
    return this.http.post<SolicitudCompra>(this.apiUrl, dto);
  }

  updateEstado(id: number, dto: UpdateSolicitudCompraEstado): Observable<SolicitudCompra> {
    return this.http.put<SolicitudCompra>(`${this.apiUrl}/${id}/estado`, dto);
  }
}
