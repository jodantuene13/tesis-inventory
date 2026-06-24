import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { UnidadMedidaService } from '../../../services/unidad-medida.service';
import { UnidadMedida, CreateUnidadMedida, UpdateUnidadMedida } from '../../../models/unidad-medida.model';
import Swal from 'sweetalert2';

@Component({
    selector: 'app-unidades-medida',
    standalone: true,
    imports: [CommonModule, FormsModule],
    templateUrl: './unidades-medida.component.html',
    styleUrls: ['./unidades-medida.component.css']
})
export class UnidadesMedidaComponent implements OnInit {
    unidades: UnidadMedida[] = [];
    loading = false;
    indicatorTotal = 0;
    indicatorActivos = 0;

    // Search/filter state
    searchUnidad = '';
    selectedEstadoUnidad = '';

    get hasUnidadFilter(): boolean {
        return !!this.searchUnidad || !!this.selectedEstadoUnidad;
    }

    get unidadesFiltered(): UnidadMedida[] {
        let list = this.unidades;
        if (this.searchUnidad) {
            const t = this.searchUnidad.toLowerCase();
            list = list.filter(u => u.nombre.toLowerCase().includes(t) || u.simbolo.toLowerCase().includes(t));
        }
        if (this.selectedEstadoUnidad !== '') {
            const activo = this.selectedEstadoUnidad === 'true';
            list = list.filter(u => u.activo === activo);
        }
        return list;
    }

    clearUnidadFilter(): void {
        this.searchUnidad = '';
        this.selectedEstadoUnidad = '';
    }

    showModal = false;
    isEdit = false;
    currentId: number | null = null;
    form: CreateUnidadMedida | UpdateUnidadMedida = this.emptyForm();

    constructor(private service: UnidadMedidaService) {}

    ngOnInit(): void {
        this.load();
    }

    load(): void {
        this.loading = true;
        this.service.getAll(true).subscribe({
            next: (data) => {
                this.unidades = data;
                this.indicatorTotal = data.length;
                this.indicatorActivos = data.filter(u => u.activo).length;
                this.loading = false;
            },
            error: () => {
                this.loading = false;
                Swal.fire('Error', 'No se pudieron cargar las unidades', 'error');
            }
        });
    }

    openCreate(): void {
        this.isEdit = false;
        this.form = this.emptyForm();
        this.showModal = true;
    }

    openEdit(u: UnidadMedida, event: Event): void {
        event.stopPropagation();
        this.isEdit = true;
        this.currentId = u.idUnidadMedida;
        this.form = { simbolo: u.simbolo, nombre: u.nombre, activo: u.activo };
        this.showModal = true;
    }

    closeModal(): void {
        this.showModal = false;
    }

    save(): void {
        if (!this.form.simbolo || !this.form.nombre) {
            Swal.fire('Atención', 'Símbolo y Nombre son obligatorios', 'warning');
            return;
        }

        if (this.isEdit && this.currentId) {
            this.service.update(this.currentId, this.form as UpdateUnidadMedida).subscribe({
                next: () => { this.load(); this.closeModal(); Swal.fire('Éxito', 'Unidad actualizada', 'success'); },
                error: (err) => Swal.fire('Error', err.error?.message || 'Error al actualizar', 'error')
            });
        } else {
            this.service.create(this.form as CreateUnidadMedida).subscribe({
                next: () => { this.load(); this.closeModal(); Swal.fire('Éxito', 'Unidad creada', 'success'); },
                error: (err) => Swal.fire('Error', err.error?.message || 'Error al crear', 'error')
            });
        }
    }

    delete(id: number, event: Event): void {
        event.stopPropagation();
        Swal.fire({ title: '¿Desactivar unidad?', icon: 'warning', showCancelButton: true, confirmButtonText: 'Sí' })
            .then((result) => {
                if (result.isConfirmed) {
                    this.service.delete(id).subscribe({
                        next: () => { this.load(); Swal.fire('Baja exitosa', '', 'success'); },
                        error: (err) => Swal.fire('Error', err.error?.message, 'error')
                    });
                }
            });
    }

    private emptyForm(): CreateUnidadMedida {
        return { simbolo: '', nombre: '', activo: true };
    }
}
