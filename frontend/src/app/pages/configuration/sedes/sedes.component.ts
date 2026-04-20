import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { SedeService } from '../../../services/sede.service';
import { Sede } from '../../../models/sede.model';
import Swal from 'sweetalert2';

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

    currentSede: Sede = {
        idSede: 0,
        nombreSede: '',
        direccion: ''
    };
    
    // Indicators
    indicatorTotal: number = 0;

    constructor(private sedeService: SedeService) { }

    ngOnInit(): void {
        this.loadSedes();
    }

    loadSedes(): void {
        this.sedeService.getAll().subscribe({
            next: (data) => {
                this.sedes = data;
                this.indicatorTotal = this.sedes.length;
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
            Swal.fire('Error', 'El nombre de la sede es obigatorio', 'error');
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

    deleteSede(sede: Sede): void {
        Swal.fire({
            title: `¿Eliminar "${sede.nombreSede}"?`,
            text: 'Para confirmar, escriba el nombre de la sede exactamente como aparece arriba:',
            input: 'text',
            inputPlaceholder: sede.nombreSede,
            showCancelButton: true,
            confirmButtonText: 'Eliminar',
            cancelButtonText: 'Cancelar',
            confirmButtonColor: '#d33',
            preConfirm: (inputValue: string) => {
                if (inputValue !== sede.nombreSede) {
                    Swal.showValidationMessage('El nombre no coincide. Operación cancelada.');
                    return false;
                }
                return true;
            }
        }).then((result: any) => {
            if (result.isConfirmed) {
                this.sedeService.delete(sede.idSede).subscribe({
                    next: () => {
                        Swal.fire('Eliminado', 'La sede ha sido eliminada.', 'success');
                        this.loadSedes();
                    },
                    error: (err) => {
                        // DIC Validation error response
                        const errorMsg = err.error?.message || 'No se pudo eliminar la sede.';
                        Swal.fire('Error', errorMsg, 'error');
                        console.error(err);
                    }
                });
            }
        });
    }
}
