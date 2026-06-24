import { Injectable, inject } from '@angular/core';
import { API_BASE_URL } from '../tokens/api-url.token';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Sede } from '../models/sede.model';

@Injectable({
    providedIn: 'root'
})
export class SedeService {
    private readonly apiUrl = `${inject(API_BASE_URL)}/api/sedes`;

    constructor(private http: HttpClient) { }

    getAll(): Observable<Sede[]> {
        return this.http.get<Sede[]>(this.apiUrl);
    }

    getById(id: number): Observable<Sede> {
        return this.http.get<Sede>(`${this.apiUrl}/${id}`);
    }

    create(sede: Partial<Sede>): Observable<Sede> {
        return this.http.post<Sede>(this.apiUrl, sede);
    }

    update(id: number, sede: Partial<Sede>): Observable<Sede> {
        return this.http.put<Sede>(`${this.apiUrl}/${id}`, sede);
    }

    delete(id: number): Observable<void> {
        return this.http.delete<void>(`${this.apiUrl}/${id}`);
    }

    transferirStock(id: number, idSedeDestino: number): Observable<void> {
        return this.http.post<void>(`${this.apiUrl}/${id}/transferir-stock`, { idSedeDestino });
    }
}
