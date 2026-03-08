import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Familia, CreateFamilia, UpdateFamilia, FamiliaAsociaciones } from '../models/familia.model';

@Injectable({
    providedIn: 'root'
})
export class FamiliaService {
    private apiUrl = 'http://localhost:5139/api/Familias';

    constructor(private http: HttpClient) { }

    getAll(includeInactive: boolean = false): Observable<Familia[]> {
        return this.http.get<Familia[]>(`${this.apiUrl}?includeInactive=${includeInactive}`);
    }

    getByRubro(idRubro: number, includeInactive: boolean = false): Observable<Familia[]> {
        return this.http.get<Familia[]>(`${this.apiUrl}/rubro/${idRubro}?includeInactive=${includeInactive}`);
    }

    getById(id: number): Observable<Familia> {
        return this.http.get<Familia>(`${this.apiUrl}/${id}`);
    }

    create(familia: CreateFamilia): Observable<Familia> {
        return this.http.post<Familia>(this.apiUrl, familia);
    }

    update(id: number, familia: UpdateFamilia): Observable<Familia> {
        return this.http.put<Familia>(`${this.apiUrl}/${id}`, familia);
    }

    delete(id: number): Observable<any> {
        return this.http.delete(`${this.apiUrl}/${id}`);
    }

    getAsociaciones(idFamilia: number): Observable<FamiliaAsociaciones> {
        return this.http.get<FamiliaAsociaciones>(`${this.apiUrl}/${idFamilia}/asociaciones`);
    }
}
