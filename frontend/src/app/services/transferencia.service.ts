import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Transferencia, CreateTransferenciaDto, ResolverTransferenciaDto } from '../models/transferencia.model';

@Injectable({
  providedIn: 'root'
})
export class TransferenciaService {
  private apiUrl = 'http://localhost:5139/api/transferencias'; // Verify port from environments

  constructor(private http: HttpClient) { }

  getEntrantes(): Observable<Transferencia[]> {
    return this.http.get<Transferencia[]>(`${this.apiUrl}/entrantes`);
  }

  getSalientes(): Observable<Transferencia[]> {
    return this.http.get<Transferencia[]>(`${this.apiUrl}/salientes`);
  }

  create(dto: CreateTransferenciaDto): Observable<Transferencia> {
    return this.http.post<Transferencia>(this.apiUrl, dto);
  }

  aceptar(id: number, observaciones?: string): Observable<any> {
    const dto: ResolverTransferenciaDto = { observaciones };
    return this.http.put(`${this.apiUrl}/${id}/aceptar`, dto);
  }

  rechazar(id: number, observaciones?: string): Observable<any> {
    const dto: ResolverTransferenciaDto = { observaciones };
    return this.http.put(`${this.apiUrl}/${id}/rechazar`, dto);
  }

  confirmarRecepcion(id: number, observaciones?: string): Observable<any> {
    const dto: ResolverTransferenciaDto = { observaciones };
    return this.http.put(`${this.apiUrl}/${id}/confirmar-recepcion`, dto);
  }

  devolver(id: number, observaciones?: string): Observable<any> {
    const dto: ResolverTransferenciaDto = { observaciones };
    return this.http.put(`${this.apiUrl}/${id}/devolver`, dto);
  }
}
