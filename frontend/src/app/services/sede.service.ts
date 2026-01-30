import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Sede } from '../models/sede.model';

@Injectable({
    providedIn: 'root'
})
export class SedeService {
    private apiUrl = 'http://localhost:5139/api/sedes';

    constructor(private http: HttpClient) { }

    getAll(): Observable<Sede[]> {
        return this.http.get<Sede[]>(this.apiUrl);
    }
}
