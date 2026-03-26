import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Atributo, CreateAtributo, UpdateAtributo, AtributoOpcion, CreateAtributoOpcion, FamiliaAtributo, CreateFamiliaAtributo } from '../models/atributo.model';

@Injectable({
    providedIn: 'root'
})
export class AtributoService {
    private apiUrl = 'http://localhost:5139/api/Atributos';

    constructor(private http: HttpClient) { }

    // --- Mantenimiento Atributos ---
    getAll(includeInactive: boolean = false): Observable<Atributo[]> {
        return this.http.get<Atributo[]>(`${this.apiUrl}?includeInactive = ${includeInactive} `);
    }

    getById(id: number): Observable<Atributo> {
        return this.http.get<Atributo>(`${this.apiUrl}/${id}`);
    }

    create(atributo: CreateAtributo): Observable<Atributo> {
        return this.http.post<Atributo>(this.apiUrl, atributo);
    }

    update(id: number, atributo: UpdateAtributo): Observable<Atributo> {
        return this.http.put<Atributo>(`${this.apiUrl}/${id}`, atributo);
    }

    delete(id: number): Observable<any> {
        return this.http.delete(`${this.apiUrl}/${id}`);
    }

    // --- Opciones de LIST ---
    getOpciones(idAtributo: number): Observable<AtributoOpcion[]> {
        return this.http.get<AtributoOpcion[]>(`${this.apiUrl}/${idAtributo}/opciones`);
    }

    addOpcion(idAtributo: number, opcion: CreateAtributoOpcion): Observable<AtributoOpcion> {
        return this.http.post<AtributoOpcion>(`${this.apiUrl}/${idAtributo}/opciones`, opcion);
    }

    deleteOpcion(idOpcion: number): Observable<any> {
        return this.http.delete(`${this.apiUrl}/opciones/${idOpcion}`);
    }

    // --- Asignaciones a Familias ---
    getAtributosDeFamilia(idFamilia: number): Observable<FamiliaAtributo[]> {
        return this.http.get<FamiliaAtributo[]>(`${this.apiUrl}/familia/${idFamilia}`);
    }

    assignToFamilia(idFamilia: number, assign: CreateFamiliaAtributo): Observable<FamiliaAtributo> {
        return this.http.post<FamiliaAtributo>(`${this.apiUrl}/familia/${idFamilia}`, assign);
    }

    removeFromFamilia(idFamilia: number, idAtributo: number): Observable<any> {
        return this.http.delete(`${this.apiUrl}/familia/${idFamilia}/atributo/${idAtributo}`);
    }

    getFamiliasDeAtributo(idAtributo: number): Observable<FamiliaAtributo[]> {
        return this.http.get<FamiliaAtributo[]>(`${this.apiUrl}/${idAtributo}/familias`);
    }
}
