import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { API_BASE_URL } from '../tokens/api-url.token';
import {
    GrupoAtributo,
    CreateGrupoAtributo,
    UpdateGrupoAtributo,
    GrupoAtributoItem,
    AddItemToGrupo,
    FamiliaGrupoAtributo,
    CreateFamiliaGrupoAtributo
} from '../models/grupo-atributo.model';

@Injectable({ providedIn: 'root' })
export class GrupoAtributoService {
    private readonly apiUrl = `${inject(API_BASE_URL)}/api/GruposAtributos`;

    constructor(private http: HttpClient) {}

    // --- Grupos maestros ---
    getAll(includeInactive = false): Observable<GrupoAtributo[]> {
        return this.http.get<GrupoAtributo[]>(`${this.apiUrl}?includeInactive=${includeInactive}`);
    }

    getById(id: number): Observable<GrupoAtributo> {
        return this.http.get<GrupoAtributo>(`${this.apiUrl}/${id}`);
    }

    create(dto: CreateGrupoAtributo): Observable<GrupoAtributo> {
        return this.http.post<GrupoAtributo>(this.apiUrl, dto);
    }

    update(id: number, dto: UpdateGrupoAtributo): Observable<GrupoAtributo> {
        return this.http.put<GrupoAtributo>(`${this.apiUrl}/${id}`, dto);
    }

    delete(id: number): Observable<void> {
        return this.http.delete<void>(`${this.apiUrl}/${id}`);
    }

    // --- Items del grupo ---
    addItem(idGrupo: number, dto: AddItemToGrupo): Observable<GrupoAtributoItem> {
        return this.http.post<GrupoAtributoItem>(`${this.apiUrl}/${idGrupo}/items`, dto);
    }

    deleteItem(idGrupo: number, idAtributo: number): Observable<void> {
        return this.http.delete<void>(`${this.apiUrl}/${idGrupo}/items/${idAtributo}`);
    }

    // --- Asignación a familias ---
    getGruposDeFamilia(idFamilia: number): Observable<FamiliaGrupoAtributo[]> {
        return this.http.get<FamiliaGrupoAtributo[]>(`${this.apiUrl}/familia/${idFamilia}`);
    }

    assignToFamilia(idFamilia: number, dto: CreateFamiliaGrupoAtributo): Observable<FamiliaGrupoAtributo> {
        return this.http.post<FamiliaGrupoAtributo>(`${this.apiUrl}/familia/${idFamilia}`, dto);
    }

    removeFromFamilia(idFamilia: number, idGrupo: number): Observable<void> {
        return this.http.delete<void>(`${this.apiUrl}/familia/${idFamilia}/grupo/${idGrupo}`);
    }
}
