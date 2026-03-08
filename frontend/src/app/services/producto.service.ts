import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Producto, CreateProducto, UpdateProducto } from '../models/producto.model';

@Injectable({
    providedIn: 'root'
})
export class ProductoService {
    private apiUrl = 'http://localhost:5139/api/Productos';

    constructor(private http: HttpClient) { }

    getAll(includeInactive: boolean = false): Observable<Producto[]> {
        return this.http.get<Producto[]>(`${this.apiUrl}?includeInactive=${includeInactive}`);
    }

    getById(id: number): Observable<Producto> {
        return this.http.get<Producto>(`${this.apiUrl}/${id}`);
    }

    create(producto: CreateProducto): Observable<Producto> {
        return this.http.post<Producto>(this.apiUrl, producto);
    }

    update(id: number, producto: UpdateProducto): Observable<Producto> {
        return this.http.put<Producto>(`${this.apiUrl}/${id}`, producto);
    }

    delete(id: number, confirmacionNombre: string): Observable<any> {
        return this.http.delete(`${this.apiUrl}/${id}?confirmacionNombre=${encodeURIComponent(confirmacionNombre)}`);
    }
}
