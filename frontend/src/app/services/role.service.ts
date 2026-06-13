import { Injectable, inject } from '@angular/core';
import { API_BASE_URL } from '../tokens/api-url.token';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Role, Permiso } from '../models/role.model';

@Injectable({
    providedIn: 'root'
})
export class RoleService {
    private readonly apiUrl = `${inject(API_BASE_URL)}/api/roles`;

    constructor(private http: HttpClient) { }

    getAll(): Observable<Role[]> {
        return this.http.get<Role[]>(this.apiUrl);
    }

    getById(id: number): Observable<Role> {
        return this.http.get<Role>(`${this.apiUrl}/${id}`);
    }

    create(role: Role): Observable<Role> {
        return this.http.post<Role>(this.apiUrl, role);
    }

    update(id: number, role: Role): Observable<void> {
        return this.http.put<void>(`${this.apiUrl}/${id}`, role);
    }

    delete(id: number): Observable<void> {
        return this.http.delete<void>(`${this.apiUrl}/${id}`);
    }

    getPermisos(): Observable<Permiso[]> {
        return this.http.get<Permiso[]>(`${this.apiUrl}/permisos`);
    }
}
