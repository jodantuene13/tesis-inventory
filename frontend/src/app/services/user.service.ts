import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { User, CreateUserDto, UpdateUserDto } from '../models/user.model';

@Injectable({
    providedIn: 'root'
})
export class UserService {
    private apiUrl = 'http://localhost:5139/api/users'; // Adjust port if needed

    constructor(private http: HttpClient) { }

    getAll(search?: string, roleId?: number, sedeId?: number, status?: boolean): Observable<User[]> {
        let params = new HttpParams();
        if (search) params = params.set('search', search);
        if (roleId) params = params.set('roleId', roleId.toString());
        if (sedeId) params = params.set('sedeId', sedeId.toString());
        if (status !== undefined && status !== null) params = params.set('status', status.toString());

        return this.http.get<User[]>(this.apiUrl, { params });
    }

    getById(id: number): Observable<User> {
        return this.http.get<User>(`${this.apiUrl}/${id}`);
    }

    create(user: CreateUserDto): Observable<User> {
        return this.http.post<User>(this.apiUrl, user);
    }

    update(id: number, user: UpdateUserDto): Observable<User> {
        return this.http.put<User>(`${this.apiUrl}/${id}`, user);
    }

    delete(id: number): Observable<void> {
        return this.http.delete<void>(`${this.apiUrl}/${id}`);
    }

    changeStatus(id: number, estado: boolean): Observable<User> {
        return this.http.patch<User>(`${this.apiUrl}/${id}/status`, { estado });
    }

    filterByRole(roleId: number): Observable<User[]> {
        return this.http.get<User[]>(`${this.apiUrl}/filter/role/${roleId}`);
    }

    filterBySede(sedeId: number): Observable<User[]> {
        return this.http.get<User[]>(`${this.apiUrl}/filter/sede/${sedeId}`);
    }
}
