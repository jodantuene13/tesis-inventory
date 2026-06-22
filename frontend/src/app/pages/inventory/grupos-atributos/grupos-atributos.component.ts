import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { GrupoAtributoService } from '../../../services/grupo-atributo.service';
import { AtributoService } from '../../../services/atributo.service';
import { RubroService } from '../../../services/rubro.service';
import { FamiliaService } from '../../../services/familia.service';
import { UnidadMedidaService } from '../../../services/unidad-medida.service';
import {
    GrupoAtributo,
    CreateGrupoAtributo,
    UpdateGrupoAtributo,
    FamiliaGrupoAtributo
} from '../../../models/grupo-atributo.model';
import { Atributo } from '../../../models/atributo.model';
import { UnidadMedida } from '../../../models/unidad-medida.model';
import { Rubro } from '../../../models/rubro.model';
import { Familia } from '../../../models/familia.model';
import Swal from 'sweetalert2';

@Component({
    selector: 'app-grupos-atributos',
    standalone: true,
    imports: [CommonModule, FormsModule],
    templateUrl: './grupos-atributos.component.html',
    styleUrls: ['./grupos-atributos.component.css']
})
export class GruposAtributosComponent implements OnInit {
    grupos: GrupoAtributo[] = [];
    loadingGrupos = false;
    selectedGrupo: GrupoAtributo | null = null;

    // KPIs
    indicatorTotal = 0;
    indicatorActivos = 0;

    // Modal grupo
    showGrupoModal = false;
    isEditGrupo = false;
    currentGrupoId: number | null = null;
    grupoForm: CreateGrupoAtributo | UpdateGrupoAtributo = this.emptyGrupoForm();
    separadoresComunes = ['*', 'x', '/', '-', '+'];

    // Atributos disponibles para agregar como items (solo NUMBER/DECIMAL)
    atributosNumericos: Atributo[] = [];
    selectedAtributoIdParaItem: number | null = null;
    idUnidadMedidaParaItem: number | null = null;

    // Unidades de medida (todas activas)
    unidades: UnidadMedida[] = [];

    get unidadesParaItemSeleccionado(): UnidadMedida[] {
        if (!this.selectedAtributoIdParaItem) return [];
        const attr = this.atributosNumericos.find(a => a.idAtributo === this.selectedAtributoIdParaItem);
        return attr?.unidadesMedida ?? [];
    }

    onAtributoParaItemChange(): void {
        this.idUnidadMedidaParaItem = null;
    }

    // Sección asignación a familia
    rubros: Rubro[] = [];
    familiasSelect: Familia[] = [];
    selectedRubroId: number | null = null;
    selectedFamiliaId: number | null = null;
    gruposDeFamilia: FamiliaGrupoAtributo[] = [];
    configObligatorio = false;

    get selectedFamiliaNombre(): string {
        const f = this.familiasSelect.find(fam => fam.idFamilia === this.selectedFamiliaId);
        return f ? f.nombre : '';
    }

    constructor(
        private grupoService: GrupoAtributoService,
        private atributoService: AtributoService,
        private rubroService: RubroService,
        private familiaService: FamiliaService,
        private unidadMedidaService: UnidadMedidaService
    ) {}

    ngOnInit(): void {
        this.loadGrupos();
        this.loadAtributosNumericos();
        this.loadRubros();
        this.loadUnidades();
    }

    loadUnidades(): void {
        this.unidadMedidaService.getAll(false).subscribe({
            next: (data) => this.unidades = data.filter(u => u.activo)
        });
    }

    // === Grupos ===

    loadGrupos(): void {
        this.loadingGrupos = true;
        this.grupoService.getAll(true).subscribe({
            next: (data) => {
                this.grupos = data;
                this.indicatorTotal = data.length;
                this.indicatorActivos = data.filter(g => g.activo).length;
                this.loadingGrupos = false;
            },
            error: () => {
                this.loadingGrupos = false;
                Swal.fire('Error', 'No se pudieron cargar los grupos de atributos', 'error');
            }
        });
    }

    selectGrupo(g: GrupoAtributo): void {
        this.selectedGrupo = g;
    }

    openCreateModal(): void {
        this.isEditGrupo = false;
        this.grupoForm = this.emptyGrupoForm();
        this.showGrupoModal = true;
    }

    openEditModal(g: GrupoAtributo, event: Event): void {
        event.stopPropagation();
        this.isEditGrupo = true;
        this.currentGrupoId = g.idGrupoAtributo;
        this.grupoForm = {
            codigoGrupo: g.codigoGrupo,
            nombre: g.nombre,
            separador: g.separador,
            unidadSufijo: g.unidadSufijo,
            activo: g.activo
        };
        this.showGrupoModal = true;
    }

    closeGrupoModal(): void {
        this.showGrupoModal = false;
    }

    saveGrupo(): void {
        if (!this.grupoForm.codigoGrupo || !this.grupoForm.nombre) {
            Swal.fire('Atención', 'Código y Nombre son obligatorios', 'warning');
            return;
        }

        if (this.isEditGrupo && this.currentGrupoId) {
            this.grupoService.update(this.currentGrupoId, this.grupoForm as UpdateGrupoAtributo).subscribe({
                next: (res) => {
                    this.loadGrupos();
                    if (this.selectedGrupo?.idGrupoAtributo === this.currentGrupoId) {
                        this.selectedGrupo = res;
                    }
                    this.closeGrupoModal();
                    Swal.fire('Éxito', 'Grupo actualizado', 'success');
                },
                error: (err) => Swal.fire('Error', err.error?.message || 'Error al actualizar', 'error')
            });
        } else {
            this.grupoService.create(this.grupoForm as CreateGrupoAtributo).subscribe({
                next: (res) => {
                    this.loadGrupos();
                    this.selectedGrupo = res;
                    this.closeGrupoModal();
                    Swal.fire('Éxito', 'Grupo creado', 'success');
                },
                error: (err) => Swal.fire('Error', err.error?.message || 'Error al crear', 'error')
            });
        }
    }

    deleteGrupo(id: number, event: Event): void {
        event.stopPropagation();
        Swal.fire({
            title: '¿Desactivar grupo?',
            text: 'Se desactivarán también todas sus asignaciones a familias.',
            icon: 'warning',
            showCancelButton: true,
            confirmButtonText: 'Sí',
            cancelButtonText: 'No'
        }).then((result) => {
            if (result.isConfirmed) {
                this.grupoService.delete(id).subscribe({
                    next: () => {
                        if (this.selectedGrupo?.idGrupoAtributo === id) this.selectedGrupo = null;
                        this.loadGrupos();
                        Swal.fire('Baja exitosa', '', 'success');
                    },
                    error: (err) => Swal.fire('Error', err.error?.message, 'error')
                });
            }
        });
    }

    // === Items del grupo ===

    loadAtributosNumericos(): void {
        this.atributoService.getAll(false).subscribe({
            next: (data) => {
                this.atributosNumericos = data.filter(a => a.tipoDato === 'NUMBER' || a.tipoDato === 'DECIMAL');
                console.log('[Grupos] atributosNumericos:', this.atributosNumericos.map(a => ({ nombre: a.nombre, unidades: a.unidadesMedida })));
            }
        });
    }

    atributoYaEnGrupo(idAtributo: number): boolean {
        return this.selectedGrupo?.items.some(i => i.idAtributo === idAtributo && i.activo) ?? false;
    }

    addItemToGrupo(): void {
        if (!this.selectedGrupo || !this.selectedAtributoIdParaItem) {
            Swal.fire('Atención', 'Seleccioná un atributo para agregar', 'warning');
            return;
        }

        const ordenAuto = this.selectedGrupo.items.filter(i => i.activo).length;
        this.grupoService.addItem(this.selectedGrupo.idGrupoAtributo, {
            idAtributo: this.selectedAtributoIdParaItem,
            orden: ordenAuto,
            idUnidadMedida: this.idUnidadMedidaParaItem ?? undefined
        }).subscribe({
            next: () => {
                this.selectedAtributoIdParaItem = null;
                this.idUnidadMedidaParaItem = null;
                this.refreshSelectedGrupo();
            },
            error: (err) => Swal.fire('Error', err.error?.message || 'Error al agregar item', 'error')
        });
    }

    removeItemFromGrupo(idAtributo: number): void {
        if (!this.selectedGrupo) return;
        Swal.fire({
            title: '¿Quitar atributo del grupo?',
            icon: 'warning',
            showCancelButton: true,
            confirmButtonText: 'Sí',
            cancelButtonText: 'No'
        }).then((result) => {
            if (result.isConfirmed) {
                this.grupoService.deleteItem(this.selectedGrupo!.idGrupoAtributo, idAtributo).subscribe({
                    next: () => this.refreshSelectedGrupo(),
                    error: (err) => Swal.fire('Error', err.error?.message, 'error')
                });
            }
        });
    }

    refreshSelectedGrupo(): void {
        if (!this.selectedGrupo) return;
        this.grupoService.getById(this.selectedGrupo.idGrupoAtributo).subscribe({
            next: (g) => {
                this.selectedGrupo = g;
                const idx = this.grupos.findIndex(x => x.idGrupoAtributo === g.idGrupoAtributo);
                if (idx >= 0) this.grupos[idx] = g;
            }
        });
    }

    formatPreview(g: GrupoAtributo): string {
        if (!g.items || g.items.length === 0) return '—';
        const valores = g.items
            .filter(i => i.activo)
            .sort((a, b) => a.orden - b.orden)
            .map(() => '?');
        return valores.join(g.separador) + (g.unidadSufijo ?? '');
    }

    // === Asignación a familia ===

    loadRubros(): void {
        this.rubroService.getAll(false).subscribe({ next: (data) => this.rubros = data });
    }

    onRubroChange(): void {
        this.selectedFamiliaId = null;
        this.gruposDeFamilia = [];
        if (this.selectedRubroId) {
            this.familiaService.getByRubro(this.selectedRubroId, false).subscribe({
                next: (data) => this.familiasSelect = data
            });
        } else {
            this.familiasSelect = [];
        }
    }

    onFamiliaChange(): void {
        if (this.selectedFamiliaId) {
            this.loadGruposDeFamilia(this.selectedFamiliaId);
        } else {
            this.gruposDeFamilia = [];
        }
    }

    loadGruposDeFamilia(idFamilia: number): void {
        this.grupoService.getGruposDeFamilia(idFamilia).subscribe({
            next: (data) => this.gruposDeFamilia = data
        });
    }

    asignarGrupoAFamilia(): void {
        if (!this.selectedFamiliaId || !this.selectedGrupo) {
            Swal.fire('Info', 'Seleccioná una familia y un grupo de la lista principal para asignar.', 'info');
            return;
        }

        this.grupoService.assignToFamilia(this.selectedFamiliaId, {
            idGrupoAtributo: this.selectedGrupo.idGrupoAtributo,
            obligatorio: this.configObligatorio,
            activo: true
        }).subscribe({
            next: () => {
                this.loadGruposDeFamilia(this.selectedFamiliaId!);
                Swal.fire('Éxito', 'Grupo asignado a la familia', 'success');
            },
            error: (err) => Swal.fire('Error', err.error?.message || 'Error al asignar', 'error')
        });
    }

    removerGrupoDeFamilia(idGrupo: number): void {
        if (!this.selectedFamiliaId) return;
        Swal.fire({
            title: '¿Quitar grupo de la familia?',
            icon: 'warning',
            showCancelButton: true,
            confirmButtonText: 'Sí, quitar'
        }).then((result) => {
            if (result.isConfirmed) {
                this.grupoService.removeFromFamilia(this.selectedFamiliaId!, idGrupo).subscribe({
                    next: () => this.loadGruposDeFamilia(this.selectedFamiliaId!),
                    error: (err) => Swal.fire('Error', err.error?.message, 'error')
                });
            }
        });
    }

    formatGrupoDisplay(fg: FamiliaGrupoAtributo): string {
        if (!fg.items || fg.items.length === 0) return '(sin atributos)';
        const partes = fg.items
            .filter(i => i.activo)
            .sort((a, b) => a.orden - b.orden)
            .map(i => i.nombreAtributo);
        return partes.join(` ${fg.separador} `) + (fg.unidadSufijo ? ` ${fg.unidadSufijo}` : '');
    }

    private emptyGrupoForm(): CreateGrupoAtributo {
        return { codigoGrupo: '', nombre: '', separador: '*', unidadSufijo: '', activo: true };
    }
}
