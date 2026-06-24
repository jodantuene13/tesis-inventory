import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { firstValueFrom } from 'rxjs';
import { SedeService } from '../../../services/sede.service';
import { Sede } from '../../../models/sede.model';
import Swal from 'sweetalert2';

interface StockItem {
    nombreProducto: string;
    sku: string;
    cantidad: number;
}

interface SedeDeleteBloqueante {
    tipo: string;
    mensaje: string;
    cantidad?: number;
    items?: StockItem[];
}

@Component({
    selector: 'app-sedes',
    standalone: true,
    imports: [CommonModule, FormsModule],
    templateUrl: './sedes.component.html',
    styleUrls: ['./sedes.component.css']
})
export class SedesComponent implements OnInit {
    sedes: Sede[] = [];
    isModalOpen = false;
    isEditing = false;
    searchTerm = '';

    currentSede: Sede = {
        idSede: 0,
        nombreSede: '',
        direccion: ''
    };

    indicatorTotal = 0;
    indicatorConDireccion = 0;
    indicatorSinDireccion = 0;

    get filteredSedes(): Sede[] {
        if (!this.searchTerm.trim()) return this.sedes;
        const q = this.searchTerm.toLowerCase();
        return this.sedes.filter(s =>
            s.nombreSede.toLowerCase().includes(q) ||
            (s.direccion || '').toLowerCase().includes(q)
        );
    }

    constructor(private sedeService: SedeService) { }

    ngOnInit(): void {
        this.loadSedes();
    }

    loadSedes(): void {
        this.sedeService.getAll().subscribe({
            next: (data) => {
                this.sedes = data;
                this.indicatorTotal = data.length;
                this.indicatorConDireccion = data.filter(s => s.direccion?.trim()).length;
                this.indicatorSinDireccion = data.filter(s => !s.direccion?.trim()).length;
            },
            error: (err) => console.error('Error loading sedes', err)
        });
    }

    openCreateModal(): void {
        this.isEditing = false;
        this.currentSede = { idSede: 0, nombreSede: '', direccion: '' };
        this.isModalOpen = true;
    }

    openEditModal(sede: Sede): void {
        this.isEditing = true;
        this.currentSede = { ...sede };
        this.isModalOpen = true;
    }

    closeModal(): void {
        this.isModalOpen = false;
    }

    saveSede(): void {
        if (!this.currentSede.nombreSede.trim()) {
            Swal.fire('Error', 'El nombre de la sede es obligatorio', 'error');
            return;
        }

        if (this.isEditing) {
            this.sedeService.update(this.currentSede.idSede, this.currentSede).subscribe({
                next: () => {
                    Swal.fire('Éxito', 'Sede actualizada correctamente', 'success');
                    this.loadSedes();
                    this.closeModal();
                },
                error: (err) => {
                    Swal.fire('Error', 'Hubo un error al actualizar la sede', 'error');
                    console.error(err);
                }
            });
        } else {
            this.sedeService.create(this.currentSede).subscribe({
                next: () => {
                    Swal.fire('Éxito', 'Sede creada correctamente', 'success');
                    this.loadSedes();
                    this.closeModal();
                },
                error: (err) => {
                    Swal.fire('Error', 'Hubo un error al crear la sede', 'error');
                    console.error(err);
                }
            });
        }
    }

    async deleteSede(sede: Sede): Promise<void> {
        const confirm = await Swal.fire({
            title: `Desactivar "${sede.nombreSede}"`,
            html: `Esta acción desactivará la sede permanentemente.<br>
                   Para confirmar, escriba <strong>eliminar</strong> en el campo de abajo.`,
            input: 'text',
            inputPlaceholder: 'eliminar',
            inputAttributes: { autocomplete: 'off' },
            showCancelButton: true,
            confirmButtonText: 'Continuar',
            cancelButtonText: 'Cancelar',
            confirmButtonColor: '#d33',
            preConfirm: (value: string) => {
                if (value !== 'eliminar') {
                    Swal.showValidationMessage('Debe escribir exactamente <b>eliminar</b> para confirmar.');
                    return false;
                }
                return true;
            }
        });

        if (!confirm.isConfirmed) return;

        await this.ejecutarDelete(sede);
    }

    private async ejecutarDelete(sede: Sede): Promise<void> {
        try {
            await firstValueFrom(this.sedeService.delete(sede.idSede));
            Swal.fire('Desactivada', `La sede "${sede.nombreSede}" fue desactivada.`, 'success');
            this.loadSedes();
        } catch (err: any) {
            const bloqueantes: SedeDeleteBloqueante[] | undefined = err.error?.bloqueantes;

            if (!bloqueantes?.length) {
                Swal.fire('Error', err.error?.message || 'No se pudo desactivar la sede.', 'error');
                return;
            }

            const stockBlocker = bloqueantes.find(b => b.tipo === 'stock');
            const otrosBloqueantes = bloqueantes.filter(b => b.tipo !== 'stock');

            if (otrosBloqueantes.length > 0) {
                const html = otrosBloqueantes.map(b => `<li class="text-left">${b.mensaje}</li>`).join('');
                Swal.fire({
                    title: 'No se puede desactivar',
                    html: `<ul class="list-disc pl-4 space-y-1">${html}</ul>`,
                    icon: 'warning'
                });
                return;
            }

            if (stockBlocker) {
                await this.mostrarDialogoTransferirStock(sede, stockBlocker);
            }
        }
    }

    private async mostrarDialogoTransferirStock(sede: Sede, stockBlocker: SedeDeleteBloqueante): Promise<void> {
        const sedesDestino = this.sedes.filter(s => s.idSede !== sede.idSede);

        const filasStock = stockBlocker.items?.map(i =>
            `<tr>
                <td style="text-align:left;padding:4px 8px">${i.nombreProducto}</td>
                <td style="text-align:left;padding:4px 8px;color:#6b7280">${i.sku}</td>
                <td style="text-align:center;padding:4px 8px;font-weight:600">${i.cantidad}</td>
            </tr>`
        ).join('') ?? '';

        const opcionesSelect = sedesDestino.map(s =>
            `<option value="${s.idSede}">${s.nombreSede}</option>`
        ).join('');

        const result = await Swal.fire({
            title: 'Stock pendiente de transferir',
            width: 600,
            html: `
                <p style="margin-bottom:12px;font-size:14px">
                    Los siguientes productos tienen stock en <strong>${sede.nombreSede}</strong>.
                    Seleccioná una sede de destino para transferirlos automáticamente.
                </p>
                <div style="max-height:200px;overflow-y:auto;margin-bottom:16px;border:1px solid #e5e7eb;border-radius:6px">
                    <table style="width:100%;border-collapse:collapse;font-size:13px">
                        <thead style="background:#f9fafb;position:sticky;top:0">
                            <tr>
                                <th style="text-align:left;padding:6px 8px;border-bottom:1px solid #e5e7eb">Producto</th>
                                <th style="text-align:left;padding:6px 8px;border-bottom:1px solid #e5e7eb">SKU</th>
                                <th style="text-align:center;padding:6px 8px;border-bottom:1px solid #e5e7eb">Cantidad</th>
                            </tr>
                        </thead>
                        <tbody>${filasStock}</tbody>
                    </table>
                </div>
                <label style="display:block;text-align:left;font-size:13px;font-weight:600;margin-bottom:6px">
                    Sede de destino
                </label>
                <select id="swal-sede-destino" class="swal2-input" style="margin:0;width:100%">
                    <option value="">-- Seleccione una sede --</option>
                    ${opcionesSelect}
                </select>
            `,
            showCancelButton: true,
            confirmButtonText: 'Transferir y desactivar sede',
            cancelButtonText: 'Cancelar',
            confirmButtonColor: '#2563eb',
            preConfirm: () => {
                const select = document.getElementById('swal-sede-destino') as HTMLSelectElement;
                if (!select?.value) {
                    Swal.showValidationMessage('Debe seleccionar una sede de destino.');
                    return false;
                }
                return parseInt(select.value, 10);
            }
        });

        if (!result.isConfirmed || !result.value) return;

        try {
            await firstValueFrom(this.sedeService.transferirStock(sede.idSede, result.value as number));
            await this.ejecutarDeleteFinal(sede);
        } catch (err: any) {
            Swal.fire('Error', err.error?.message || 'No se pudo transferir el stock.', 'error');
        }
    }

    private async ejecutarDeleteFinal(sede: Sede): Promise<void> {
        try {
            await firstValueFrom(this.sedeService.delete(sede.idSede));
            Swal.fire('Desactivada', `La sede "${sede.nombreSede}" fue desactivada correctamente.`, 'success');
            this.loadSedes();
        } catch (err: any) {
            const bloqueantes: SedeDeleteBloqueante[] | undefined = err.error?.bloqueantes;
            if (bloqueantes?.length) {
                const html = bloqueantes.map(b => `<li class="text-left">${b.mensaje}</li>`).join('');
                Swal.fire({
                    title: 'No se puede desactivar',
                    html: `<ul class="list-disc pl-4 space-y-1">${html}</ul>`,
                    icon: 'warning'
                });
            } else {
                Swal.fire('Error', err.error?.message || 'No se pudo desactivar la sede.', 'error');
            }
        }
    }
}
