import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AuthService } from '../../services/auth';
import { ProductoService } from '../../services/producto.service';
import { StockService } from '../../services/stock.service';
import { TransferenciaService } from '../../services/transferencia.service';
import { Observable, forkJoin } from 'rxjs';
import { User } from '../../models/user';
import { RouterLink } from '@angular/router';

import { SedeContextService } from '../../services/sede-context.service';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [CommonModule, RouterLink],
  templateUrl: './home.html',
  styleUrls: ['./home.css']
})
export class HomeComponent implements OnInit {
  user$!: Observable<User | null>;
  
  // Stats
  totalProductos: number = 0;
  bajoStockCount: number = 0;
  transferenciasPendientes: number = 0;
  movimientosHoy: number = 0;
  
  loading: boolean = true;

  constructor(
    private authService: AuthService,
    private productoService: ProductoService,
    private stockService: StockService,
    private transferenciaService: TransferenciaService,
    private sedeContextService: SedeContextService
  ) { }

  ngOnInit() {
    this.user$ = this.authService.user$;
    
    // Escuchar cambios de sede para actualizar dashboard
    this.sedeContextService.sedeEnContexto$.subscribe(() => {
        this.loadDashboardData();
    });
  }

  loadDashboardData() {
    this.loading = true;
    
    const today = new Date().toISOString().split('T')[0];

    forkJoin({
      productos: this.productoService.getAll(false),
      stockBajo: this.stockService.getStockSede(undefined, undefined, undefined, true, true, 1, 10),
      transferencias: this.transferenciaService.getEntrantes(),
      movimientos: this.stockService.getHistorialGlobal(undefined, undefined, undefined, undefined, undefined, today, today)
    }).subscribe({
      next: (data) => {
        this.totalProductos = data.productos.length;
        this.bajoStockCount = data.stockBajo.totalRecords || data.stockBajo.length || 0;
        this.transferenciasPendientes = data.transferencias.filter(t => t.estado === 0).length;
        this.movimientosHoy = data.movimientos.totalRecords || data.movimientos.length || 0;
        this.loading = false;
      },
      error: (err) => {
        console.error('Error loading dashboard data', err);
        this.loading = false;
      }
    });
  }

  logout() {
    this.authService.logout();
  }
}
