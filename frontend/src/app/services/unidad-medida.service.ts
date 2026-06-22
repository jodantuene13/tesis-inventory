import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { API_BASE_URL } from '../tokens/api-url.token';
import { UnidadMedida, CreateUnidadMedida, UpdateUnidadMedida } from '../models/unidad-medida.model';

@Injectable({ providedIn: 'root' })
export class UnidadMedidaService {
    private readonly apiUrl = `${inject(API_BASE_URL)}/api/UnidadesMedida`;

    constructor(private http: HttpClient) {}

    getAll(includeInactive = false): Observable<UnidadMedida[]> {
        return this.http.get<UnidadMedida[]>(`${this.apiUrl}?includeInactive=${includeInactive}`);
    }

    getById(id: number): Observable<UnidadMedida> {
        return this.http.get<UnidadMedida>(`${this.apiUrl}/${id}`);
    }

    create(dto: CreateUnidadMedida): Observable<UnidadMedida> {
        return this.http.post<UnidadMedida>(this.apiUrl, dto);
    }

    update(id: number, dto: UpdateUnidadMedida): Observable<UnidadMedida> {
        return this.http.put<UnidadMedida>(`${this.apiUrl}/${id}`, dto);
    }

    delete(id: number): Observable<void> {
        return this.http.delete<void>(`${this.apiUrl}/${id}`);
    }
}
