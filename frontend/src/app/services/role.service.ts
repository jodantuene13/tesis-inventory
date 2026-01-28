import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Role } from '../models/role.model';

@Injectable({
    providedIn: 'root'
})
export class RoleService {
    private apiUrl = 'http://localhost:5139/api/roles';

    constructor(private http: HttpClient) { }

    getAll(): Observable<Role[]> {
        return this.http.get<Role[]>(this.apiUrl);
    }
}
