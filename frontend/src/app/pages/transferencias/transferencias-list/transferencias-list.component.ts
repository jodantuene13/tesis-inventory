import { Component, OnInit, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { TransferenciaService } from '../../../services/transferencia.service';
import { SedeService } from '../../../services/sede.service';
import { StockService } from '../../../services/stock.service';
import { ProductoService } from '../../../services/producto.service';
import { AuthService } from '../../../services/auth';
import { Transferencia, MotivoTransferencia, CreateTransferenciaDetalleDto } from '../../../models/transferencia.model';
import { TransferenciaCardComponent } from '../transferencia-card/transferencia-card.component';
import { SedeContextService } from '../../../services/sede-context.service';
import { Subscription, lastValueFrom } from 'rxjs';
import { FichaProductoModalComponent } from '../../../shared/components/ficha-producto-modal/ficha-producto-modal.component';
import Swal from 'sweetalert2';

interface CartItem extends CreateTransferenciaDetalleDto {
  productoInfo: any;
}

@Component({
  selector: 'app-transferencias-list',
  standalone: true,
  imports: [CommonModule, FormsModule, TransferenciaCardComponent, FichaProductoModalComponent],
  templateUrl: './transferencias-list.component.html',
  styleUrls: []
})
export class TransferenciasListComponent implements OnInit, OnDestroy {
  activeTab: 'entrantes' | 'salientes' = 'entrantes';
  entrantes: Transferencia[] = [];
  salientes: Transferencia[] = [];
  isLoading = false;
  private contextSub!: Subscription;

  // Modal Properties
  isModalOpen = false;
  isProductModalOpen = false;
  sedes: any[] = [];
  stockList: any[] = [];
  filteredStock: any[] = [];
  searchProduct = '';
  MotivoEnum = MotivoTransferencia;

  newRequest = {
    idSedeOrigen: null as number | null,
    motivo: MotivoTransferencia.Definitiva,
    observaciones: '',
    detalles: [] as CartItem[]
  };

  // View Details (Cards)
  isViewModalOpen = false;
  selectedViewTransferencia: Transferencia | null = null;
  selectedViewStockResults: any[] = [];
  isLoadingViewStock = false;

  // Shared Modal: Ficha de Producto
  isFichaProductoOpen = false;
  selectedFichaStock: any = null;

  constructor(
    private transferenciaService: TransferenciaService,
    private sedeService: SedeService,
    private stockService: StockService,
    private productoService: ProductoService,
    private authService: AuthService,
    private sedeContextService: SedeContextService
  ) {}

  ngOnInit(): void {
    this.contextSub = this.sedeContextService.sedeEnContexto$.subscribe(() => {
      this.cargarDatos();
    });
  }

  ngOnDestroy(): void {
    if (this.contextSub) this.contextSub.unsubscribe();
  }

  cambiarPestana(tab: 'entrantes' | 'salientes') {
    this.activeTab = tab;
  }

  cargarDatos() {
    this.isLoading = true;
    
    this.transferenciaService.getEntrantes().subscribe({
      next: (res) => { this.entrantes = res; },
      error: (err) => { console.error(err); }
    });

    this.transferenciaService.getSalientes().subscribe({
      next: (res) => { this.salientes = res; this.isLoading = false; },
      error: (err) => { console.error(err); this.isLoading = false; }
    });
  }

  // --- Modal Logic ---
  openModal() {
    this.isModalOpen = true;
    this.resetForm();
    this.loadSedes();
    this.stockList = [];
    this.filteredStock = [];
  }

  closeModal() {
    this.isModalOpen = false;
    this.isProductModalOpen = false;
  }

  openProductModal() {
    this.isProductModalOpen = true;
    this.searchProduct = '';
    this.filteredStock = [...this.stockList];
  }

  closeProductModal() {
    this.isProductModalOpen = false;
  }

  openFichaProducto(item: any) {
    this.selectedFichaStock = item;
    this.isFichaProductoOpen = true;
  }

  closeFichaProducto() {
    this.isFichaProductoOpen = false;
    this.selectedFichaStock = null;
  }

  resetForm() {
    this.newRequest = {
      idSedeOrigen: null,
      motivo: MotivoTransferencia.Definitiva,
      observaciones: '',
      detalles: []
    };
    this.searchProduct = '';
  }

  loadSedes() {
    let userSedeId = 0;
    this.authService.user$.subscribe(u => { if(u?.idSede) userSedeId = u.idSede; }).unsubscribe();

    this.sedeService.getAll().subscribe(res => {
      this.sedes = res.filter(s => s.idSede !== userSedeId);
    });
  }

  onSedeChange() {
    this.searchProduct = '';
    this.newRequest.detalles = []; // Limpiar carrito si cambia la sede
    
    if (!this.newRequest.idSedeOrigen) {
        this.stockList = [];
        this.filteredStock = [];
        return;
    }

    Promise.all([
      lastValueFrom(this.productoService.getAll(false)),
      lastValueFrom(this.stockService.getStockSede('', undefined, undefined, true, undefined, 1, 1000, this.newRequest.idSedeOrigen))
    ]).then(([productosRes, stockRes]: [any, any]) => {
      const allProductos = productosRes || [];
      const stockArray = stockRes?.data || stockRes || [];

      this.stockList = allProductos.map((p: any) => {
        const matchingStock = stockArray.find((s: any) => s.idProducto === p.idProducto);
        return {
          idProducto: p.idProducto,
          sku: p.sku,
          nombreProducto: p.nombre,
          rubroProducto: matchingStock?.rubroProducto || 'N/A',
          familiaProducto: p.nombreFamilia || matchingStock?.familiaProducto || 'N/A',
          unidadMedida: p.unidadMedida || 'Unidades',
          cantidadActual: matchingStock ? matchingStock.cantidadActual : 0,
          puntoReposicion: matchingStock ? matchingStock.puntoReposicion : 0,
          estadoProducto: typeof p.activo === 'boolean' ? p.activo : (matchingStock?.estadoProducto ?? false),
          conBajoStock: matchingStock ? (matchingStock.cantidadActual <= matchingStock.puntoReposicion) : true,
          atributos: p.atributos || matchingStock?.atributos || []
        };
      });

      this.filteredStock = [...this.stockList];
    });
  }

  onSearchProduct() {
    const term = this.searchProduct.toLowerCase();
    this.filteredStock = this.stockList.filter(s => 
      s.nombreProducto?.toLowerCase().includes(term) || 
      s.sku?.toLowerCase().includes(term)
    );
  }

  selectProduct(stockItem: any) {
    const alreadyExists = this.newRequest.detalles.find(d => d.idProducto === stockItem.idProducto);
    if (alreadyExists) {
      alreadyExists.cantidad += 1;
    } else {
      this.newRequest.detalles.push({
        idProducto: stockItem.idProducto,
        cantidad: 1,
        productoInfo: stockItem // Guardamos info para renderizar la tabla
      });
    }
    
    this.searchProduct = '';
    this.isProductModalOpen = false; // Como acordado, devolvemos al carrito tras seleccionar
  }

  removeDetalle(index: number) {
    this.newRequest.detalles.splice(index, 1);
  }

  submitRequest() {
    if (!this.newRequest.idSedeOrigen || this.newRequest.detalles.length === 0) {
      Swal.fire('Error', 'Debe seleccionar una Sede Origen y al menos un producto.', 'error');
      return;
    }

    const hasInvalidQuantities = this.newRequest.detalles.some(d => d.cantidad <= 0);
    if (hasInvalidQuantities) {
      Swal.fire('Error', 'Todos los productos deben tener una cantidad mayor a 0.', 'error');
      return;
    }

    const sedeOrigenStr = this.sedes.find(s => s.idSede == this.newRequest.idSedeOrigen)?.nombreSede || 'Desconocida';
    const motivoStr = this.newRequest.motivo == MotivoTransferencia.Prestamo ? 'Préstamo' : 'Definitiva';

    // Generar tabla HTML para el sweet alert
    let tableRows = '';
    let hasStockWarnings = false;

    this.newRequest.detalles.forEach(d => {
      const remaining = d.productoInfo.cantidadActual - d.cantidad;
      if (remaining < 0) hasStockWarnings = true;
      
      tableRows += `
        <tr>
          <td style="text-align: left; border-bottom: 1px solid #eee; padding: 4px;">${d.productoInfo.sku} - ${d.productoInfo.nombreProducto}</td>
          <td style="text-align: center; border-bottom: 1px solid #eee; padding: 4px;">${d.productoInfo.cantidadActual}</td>
          <td style="text-align: center; border-bottom: 1px solid #eee; padding: 4px; font-weight: bold; color: #ef4444;">+${d.cantidad}</td>
        </tr>
      `;
    });

    const warningInfo = hasStockWarnings ? '<p style="color: #d97706; font-size: 12px; margin-top: 10px; font-weight: bold;">⚠️ Advertencia: Algunos productos solicitados superan el stock actual en la Sede origen.</p>' : '';

    Swal.fire({
      title: 'Confirmar Solicitud',
      html: `
        <div style="font-size: 14px; text-align: left;">
          <p><strong>Solicitar a Sede:</strong> ${sedeOrigenStr}</p>
          <p><strong>Motivo:</strong> ${motivoStr}</p>
          <table style="width: 100%; margin-top: 15px; border-collapse: collapse; font-size: 13px;">
            <thead>
              <tr style="background-color: #f3f4f6;">
                <th style="padding: 5px;">Producto</th>
                <th style="padding: 5px;">Stock Sede</th>
                <th style="padding: 5px;">Pedir</th>
              </tr>
            </thead>
            <tbody>
              ${tableRows}
            </tbody>
          </table>
          ${warningInfo}
        </div>
      `,
      icon: 'question',
      showCancelButton: true,
      confirmButtonText: 'Enviar Solicitud',
      cancelButtonText: 'Cancelar',
      confirmButtonColor: '#4f46e5',
      cancelButtonColor: '#9ca3af',
      width: '600px'
    }).then((result) => {
      if (result.isConfirmed) {
        let userSedeId = 1; 
        this.authService.user$.subscribe(u => { if(u?.idSede) userSedeId = u.idSede; }).unsubscribe();

        // Mapeo simple de detalles
        const payloadDetalles = this.newRequest.detalles.map(d => ({
          idProducto: d.idProducto,
          cantidad: d.cantidad
        }));

        this.transferenciaService.create({
          idSedeDestino: userSedeId,
          idSedeOrigen: this.newRequest.idSedeOrigen!,
          detalles: payloadDetalles,
          motivo: Number(this.newRequest.motivo),
          observaciones: this.newRequest.observaciones
        }).subscribe({
          next: () => {
            Swal.fire('Completado', 'Solicitud creada con éxito', 'success');
            this.closeModal();
            this.cargarDatos(); 
            this.activeTab = 'salientes';
          },
          error: (err) => {
            console.error(err);
            const backendMsg = err.error?.message || err.message;
            const backendDetails = err.error?.details || '';
            Swal.fire('Error', `<p>No se pudo crear la solicitud.</p><br><p class="text-sm text-red-600">${backendMsg}</p>`, 'error');
          }
        });
      }
    });
  }

  // --- Actions Logic ---
  aceptar(t: Transferencia) {
    if(confirm(`¿Desea aceptar la transferencia de ${t.detalles.length} productos hacia ${t.nombreSedeDestino}?`)) {
      this.transferenciaService.aceptar(t.idTransferencia, 'Aceptado desde listado').subscribe({
        next: () => {
          Swal.fire('Aceptada', 'La solicitud pasó a En Tránsito', 'success');
          this.cargarDatos();
        },
        error: (e) => Swal.fire('Error', 'No se pudo aceptar', 'error')
      });
    }
  }

  rechazar(t: Transferencia) {
    const motivo = prompt('Motivo de rechazo:');
    if(motivo !== null) {
      this.transferenciaService.rechazar(t.idTransferencia, motivo).subscribe({
        next: () => this.cargarDatos(),
        error: (e) => Swal.fire('Error', 'No se pudo rechazar', 'error')
      });
    }
  }

  recibir(t: Transferencia) {
    if(confirm(`¿Confirma la recepción de ${t.detalles.length} productos? Esto impactará el stock.`)) {
      this.transferenciaService.confirmarRecepcion(t.idTransferencia, 'Recibido').subscribe({
        next: () => {
          Swal.fire('Recibido', 'Stock actualizado en destino', 'success');
          this.cargarDatos();
        },
        error: (e) => Swal.fire('Error', 'No se pudo confirmar', 'error')
      });
    }
  }

  devolver(t: Transferencia) {
    if(confirm(`¿Desea devolver el préstamo asignado a la transferencia ${t.codigoTracking}?`)) {
      this.transferenciaService.devolver(t.idTransferencia, 'Devolución').subscribe({
        next: () => {
          Swal.fire('Devuelto', 'Se ha devuelto el stock a la sede origen', 'success');
          this.cargarDatos();
        },
        error: (e) => Swal.fire('Error', 'Error al devolver', 'error')
      });
    }
  }

  verDetalles(t: Transferencia) {
    this.selectedViewTransferencia = t;
    this.isViewModalOpen = true;
    this.selectedViewStockResults = [];
    this.isLoadingViewStock = true;

    // Buscar en el stock en vivo de la Sede Origen para MÚLTIPLES productos
    this.stockService.getStockSede('', undefined, undefined, true, undefined, 1, 1000, t.idSedeOrigen).subscribe({
      next: (res) => {
        const list = res.data || res;
        this.selectedViewStockResults = t.detalles.map(detalle => {
           const infoVivo = list.find((s: any) => s.idProducto === detalle.idProducto);
           return {
             idProducto: detalle.idProducto,
             sku: detalle.sku,
             nombreProducto: detalle.nombreProducto,
             cantidadActual: infoVivo ? infoVivo.cantidadActual : 0,
             cantidadPedido: detalle.cantidad,
             stockSnapshot: detalle.stockOrigenSnapshot
           };
        });
        this.isLoadingViewStock = false;
      },
      error: () => {
        this.selectedViewStockResults = t.detalles.map(detalle => ({
           idProducto: detalle.idProducto,
           sku: detalle.sku,
           nombreProducto: detalle.nombreProducto,
           cantidadActual: '?',
           cantidadPedido: detalle.cantidad,
           stockSnapshot: detalle.stockOrigenSnapshot
        }));
        this.isLoadingViewStock = false;
      }
    });

  }

  closeViewModal() {
    this.isViewModalOpen = false;
    this.selectedViewTransferencia = null;
  }
}
