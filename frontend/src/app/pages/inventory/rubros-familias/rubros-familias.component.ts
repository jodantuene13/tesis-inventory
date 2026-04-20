import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RubroService } from '../../../services/rubro.service';
import { FamiliaService } from '../../../services/familia.service';
import { Rubro, CreateRubro, UpdateRubro } from '../../../models/rubro.model';
import { Familia, CreateFamilia, UpdateFamilia, FamiliaAsociaciones } from '../../../models/familia.model';
import Swal from 'sweetalert2';

@Component({
    selector: 'app-rubros-familias',
    standalone: true,
    imports: [CommonModule, FormsModule],
    templateUrl: './rubros-familias.component.html',
    styleUrls: ['./rubros-familias.component.css']
})
export class RubrosFamiliasComponent implements OnInit {
    // Rubros state
    rubros: Rubro[] = [];
    selectedRubroId: number | null = null;
    loadingRubros = false;
    showRubroModal = false;
    isEditRubro = false;
    currentRubroForm: CreateRubro | UpdateRubro = { codigoRubro: '', nombre: '', activo: true };
    currentRubroId: number | null = null;
    errorRubro: string | null = null;
    
    // Indicators
    indicatorTotalRubros: number = 0;
    indicatorTotalFamilias: number = 0;
    indicatorRubrosActivos: number = 0;

    // Familias state
    familias: Familia[] = [];
    loadingFamilias = false;
    showFamiliaModal = false;
    isEditFamilia = false;
    currentFamiliaForm: CreateFamilia | UpdateFamilia = { idRubro: 0, codigoFamilia: '', nombre: '', activo: true };
    currentFamiliaId: number | null = null;
    errorFamilia: string | null = null;

    showAsociacionesModal = false;
    loadingAsociaciones = false;
    currentAsociaciones: FamiliaAsociaciones | null = null;
    currentFamiliaName = '';

    constructor(
        private rubroService: RubroService,
        private familiaService: FamiliaService
    ) { }

    ngOnInit(): void {
        this.loadRubros();
    }

    // --- Rubro Logic ---
    loadRubros(): void {
        this.loadingRubros = true;
        this.rubroService.getAll(true).subscribe({
            next: (data) => {
                this.rubros = data;
                this.calculateIndicators();
                this.loadingRubros = false;
            },
            error: () => {
                this.loadingRubros = false;
                Swal.fire('Error', 'No se pudieron cargar los rubros', 'error');
            }
        });
    }

    selectRubro(idRubro: number): void {
        this.selectedRubroId = idRubro;
        this.loadFamilias(idRubro);
    }

    openCreateRubroModal(): void {
        this.isEditRubro = false;
        this.currentRubroForm = { codigoRubro: '', nombre: '', activo: true };
        this.errorRubro = null;
        this.showRubroModal = true;
    }

    openEditRubroModal(r: Rubro, event: Event): void {
        event.stopPropagation();
        this.isEditRubro = true;
        this.currentRubroId = r.idRubro;
        this.currentRubroForm = { codigoRubro: r.codigoRubro, nombre: r.nombre, activo: r.activo };
        this.errorRubro = null;
        this.showRubroModal = true;
    }

    closeRubroModal(): void {
        this.showRubroModal = false;
    }

    saveRubro(): void {
        if (!this.currentRubroForm.codigoRubro || !this.currentRubroForm.nombre) {
            this.errorRubro = "Código y Nombre son obligatorios";
            return;
        }

        if (this.isEditRubro && this.currentRubroId) {
            this.rubroService.update(this.currentRubroId, this.currentRubroForm as UpdateRubro).subscribe({
                next: () => {
                    this.loadRubros();
                    this.closeRubroModal();
                    Swal.fire('Éxito', 'Rubro actualizado', 'success');
                },
                error: (err) => {
                    this.errorRubro = err.error?.message || 'Error al actualizar rubro';
                }
            });
        } else {
            this.rubroService.create(this.currentRubroForm as CreateRubro).subscribe({
                next: () => {
                    this.loadRubros();
                    this.closeRubroModal();
                    Swal.fire('Éxito', 'Rubro creado', 'success');
                },
                error: (err) => {
                    this.errorRubro = err.error?.message || 'Error al crear rubro';
                }
            });
        }
    }

    deleteRubro(id: number, event: Event): void {
        event.stopPropagation();
        Swal.fire({
            title: '¿Dar de baja este rubro?',
            text: "Se marcará como inactivo (Baja lógica).",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonText: 'Sí, dar de baja',
            cancelButtonText: 'Cancelar'
        }).then((result) => {
            if (result.isConfirmed) {
                this.rubroService.delete(id).subscribe({
                    next: () => {
                        this.loadRubros();
                        if (this.selectedRubroId === id) {
                            this.selectedRubroId = null;
                            this.familias = [];
                        }
                        Swal.fire('Baja Exitosa', 'El rubro ha sido desactivado.', 'success');
                    },
                    error: (err) => {
                        Swal.fire('Error', err.error?.message || 'No se pudo desactivar el rubro.', 'error');
                    }
                });
            }
        });
    }


    // --- Familia Logic ---
    loadFamilias(idRubro: number): void {
        this.loadingFamilias = true;
        this.familiaService.getByRubro(idRubro, true).subscribe({
            next: (data) => {
                this.familias = data;
                this.loadingFamilias = false;
            },
            error: () => {
                this.loadingFamilias = false;
                Swal.fire('Error', 'No se pudieron cargar las familias', 'error');
            }
        });
    }

    calculateIndicators(): void {
        this.indicatorTotalRubros = this.rubros.length;
        this.indicatorRubrosActivos = this.rubros.filter(r => r.activo).length;
        
        // Cargar total de familias global
        this.familiaService.getAll(true).subscribe(fams => {
            this.indicatorTotalFamilias = fams.length;
        });
    }

    openCreateFamiliaModal(): void {
        if (!this.selectedRubroId) {
            Swal.fire('Atención', 'Seleccione un Rubro primero', 'info');
            return;
        }
        this.isEditFamilia = false;
        this.currentFamiliaForm = { idRubro: this.selectedRubroId, codigoFamilia: '', nombre: '', activo: true };
        this.errorFamilia = null;
        this.showFamiliaModal = true;
    }

    openEditFamiliaModal(f: Familia): void {
        this.isEditFamilia = true;
        this.currentFamiliaId = f.idFamilia;
        this.currentFamiliaForm = { idRubro: f.idRubro, codigoFamilia: f.codigoFamilia, nombre: f.nombre, activo: f.activo };
        this.errorFamilia = null;
        this.showFamiliaModal = true;
    }

    closeFamiliaModal(): void {
        this.showFamiliaModal = false;
    }

    saveFamilia(): void {
        if (!this.currentFamiliaForm.codigoFamilia || !this.currentFamiliaForm.nombre) {
            this.errorFamilia = "Código y Nombre son obligatorios";
            return;
        }

        if (this.isEditFamilia && this.currentFamiliaId) {
            this.familiaService.update(this.currentFamiliaId, this.currentFamiliaForm as UpdateFamilia).subscribe({
                next: () => {
                    if (this.selectedRubroId) this.loadFamilias(this.selectedRubroId);
                    this.closeFamiliaModal();
                    Swal.fire('Éxito', 'Familia actualizada', 'success');
                },
                error: (err) => {
                    this.errorFamilia = err.error?.message || 'Error al actualizar familia';
                }
            });
        } else {
            this.familiaService.create(this.currentFamiliaForm as CreateFamilia).subscribe({
                next: () => {
                    if (this.selectedRubroId) this.loadFamilias(this.selectedRubroId);
                    this.closeFamiliaModal();
                    Swal.fire('Éxito', 'Familia creada', 'success');
                },
                error: (err) => {
                    this.errorFamilia = err.error?.message || 'Error al crear familia';
                }
            });
        }
    }

    deleteFamilia(id: number): void {
        Swal.fire({
            title: '¿Eliminar físicamente esta familia?',
            text: "Se borrará de forma permanente. Esta operación fallará si hay productos o atributos asociados.",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonText: 'Sí, eliminar',
            cancelButtonText: 'Cancelar'
        }).then((result) => {
            if (result.isConfirmed) {
                this.familiaService.delete(id).subscribe({
                    next: () => {
                        if (this.selectedRubroId) this.loadFamilias(this.selectedRubroId);
                        Swal.fire('Eliminación Exitosa', 'La familia ha sido eliminada permanentemente.', 'success');
                    },
                    error: (err) => {
                        Swal.fire('Error', err.error?.message || 'No se pudo eliminar la familia.', 'error');
                    }
                });
            }
        });
    }

    // --- Asociaciones Modal Logic ---
    openAsociacionesModal(f: Familia): void {
        this.currentFamiliaName = f.nombre;
        this.showAsociacionesModal = true;
        this.loadingAsociaciones = true;
        this.currentAsociaciones = null;

        this.familiaService.getAsociaciones(f.idFamilia).subscribe({
            next: (data) => {
                this.currentAsociaciones = data;
                this.loadingAsociaciones = false;
            },
            error: (err) => {
                this.loadingAsociaciones = false;
                Swal.fire('Error', err.error?.message || 'Error al cargar asociaciones', 'error');
                this.showAsociacionesModal = false;
            }
        });
    }

    closeAsociacionesModal(): void {
        this.showAsociacionesModal = false;
        this.currentAsociaciones = null;
    }
}
