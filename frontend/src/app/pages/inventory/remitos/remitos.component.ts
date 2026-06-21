import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { PaginationComponent } from '../../../shared/components/pagination/pagination.component';
import { StockService } from '../../../services/stock.service';
import { UserService } from '../../../services/user.service';
import { SedeContextService } from '../../../services/sede-context.service';
import { OperacionStockResponseDto } from '../../../models/stock.model';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-remitos',
  standalone: true,
  imports: [CommonModule, FormsModule, PaginationComponent],
  templateUrl: './remitos.component.html',
  styleUrls: ['./remitos.component.css']
})
export class RemitosComponent implements OnInit {

  private contextSub!: Subscription;
  operaciones: OperacionStockResponseDto[] = [];
  
  // Filters
  searchTerm: string = '';
  selectedTipoOperacion: string = '';
  selectedUsuarioId: number | null = null;
  fechaDesde: string = '';
  fechaHasta: string = '';

  // Pagination
  page: number = 1;
  pageSize: number = 10;
  totalCount: number = 0;
  get totalPages(): number {
    return Math.ceil(this.totalCount / this.pageSize) || 1;
  }

  // Selectors Data
  usuarios: any[] = [];
  
  // Modal Detail
  activeModal: string | null = null;
  selectedOperacion: OperacionStockResponseDto | null = null;

  loading: boolean = false;
  hasSearched: boolean = false;

  constructor(
    private stockService: StockService,
    private userService: UserService,
    private sedeContextService: SedeContextService
  ) {}

  ngOnInit(): void {
    this.loadUsuarios();
    
    this.contextSub = this.sedeContextService.sedeEnContexto$.subscribe(() => {
      this.page = 1;
      this.search();
    });
  }

  ngOnDestroy(): void {
    if (this.contextSub) this.contextSub.unsubscribe();
  }

  loadUsuarios() {
    this.userService.getAll().subscribe({
      next: (data: any) => this.usuarios = data,
      error: (err: any) => console.error('Error al cargar usuarios', err)
    });
  }

  searchFormChange() {
    this.page = 1;
    this.search();
  }

  search() {
    this.loading = true;
    this.hasSearched = true;
    
    const skip = (this.page - 1) * this.pageSize;

    this.stockService.getOperaciones(
      this.searchTerm || undefined,
      this.selectedTipoOperacion || undefined,
      this.selectedUsuarioId || undefined,
      this.fechaDesde || undefined,
      this.fechaHasta || undefined,
      skip,
      this.pageSize
    ).subscribe({
      next: (res) => {
        this.operaciones = res.data;
        this.totalCount = res.totalCount;
        this.loading = false;
      },
      error: (err) => {
        console.error('Error al cargar operaciones', err);
        this.loading = false;
      }
    });
  }

  clearFilters() {
    this.searchTerm = '';
    this.selectedTipoOperacion = '';
    this.selectedUsuarioId = null;
    this.fechaDesde = '';
    this.fechaHasta = '';
    
    this.page = 1;
    this.search();
    this.hasSearched = false;
  }

  onPageChange(p: number): void {
    this.page = p;
    this.search();
  }

  onPageSizeChange(size: number): void {
    this.pageSize = size;
    this.page = 1;
    this.search();
  }

  openDetalleModal(operacion: OperacionStockResponseDto) {
    this.selectedOperacion = operacion;
    this.activeModal = 'detalle';
  }

  closeModal() {
    this.activeModal = null;
    this.selectedOperacion = null;
  }

  printRemito() {
    window.print();
  }
}
