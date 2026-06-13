import { Injectable, inject } from '@angular/core';
import { API_BASE_URL } from '../tokens/api-url.token';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Rubro, CreateRubro, UpdateRubro } from '../models/rubro.model';

@Injectable({
    providedIn: 'root'
})
export class RubroService {
    private readonly apiUrl = `${inject(API_BASE_URL)}/api/Rubros`;

    constructor(private http: HttpClient) { }

    getAll(includeInactive: boolean = false): Observable<Rubro[]> {
        return this.http.get<Rubro[]>(`${this.apiUrl}?includeInactive=${includeInactive}`);
    }

    getById(id: number): Observable<Rubro> {
        return this.http.get<Rubro>(`${this.apiUrl}/${id}`);
    }

    create(rubro: CreateRubro): Observable<Rubro> {
        return this.http.post<Rubro>(this.apiUrl, rubro);
    }

    update(id: number, rubro: UpdateRubro): Observable<Rubro> {
        return this.http.put<Rubro>(`${this.apiUrl}/${id}`, rubro);
    }

    delete(id: number): Observable<any> {
        return this.http.delete(`${this.apiUrl}/${id}`);
    }
}
