import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, FormArray, Validators } from '@angular/forms';
import { ProductoService } from '../../../services/producto.service';
import { RubroService } from '../../../services/rubro.service';
import { FamiliaService } from '../../../services/familia.service';
import { AtributoService } from '../../../services/atributo.service';

import { Producto, CreateProducto, CreateProductoAtributoValor, ProductoAtributoValor } from '../../../models/producto.model';
import { Rubro } from '../../../models/rubro.model';
import { Familia } from '../../../models/familia.model';
import { FamiliaAtributo, AtributoOpcion } from '../../../models/atributo.model';

import Swal from 'sweetalert2';

@Component({
    selector: 'app-productos',
    standalone: true,
    imports: [CommonModule, ReactiveFormsModule],
    templateUrl: './productos.component.html',
    styleUrls: ['./productos.component.css']
})
export class ProductosComponent implements OnInit {
    productos: Producto[] = [];
    loading = false;
    
    // Indicators
    indicatorTotal: number = 0;
    indicatorActivos: number = 0;
    indicatorInactivos: number = 0;

    showModal = false;
    isEdit = false;
    currentProductoId: number | null = null;
    skuAutogenerado = 'Autogenerado al guardar';

    rubros: Rubro[] = [];
    familias: Familia[] = [];
    atributosDinamicos: FamiliaAtributo[] = [];
    opcionesListas: { [idAtributo: number]: AtributoOpcion[] } = {};
    originalAtributos: ProductoAtributoValor[] = [];

    productForm: FormGroup;

    constructor(
        private fb: FormBuilder,
        private productoService: ProductoService,
        private rubroService: RubroService,
        private familiaService: FamiliaService,
        private atributoService: AtributoService
    ) {
        this.productForm = this.fb.group({
            idRubro: ['', Validators.required],
            idFamilia: [{ value: '', disabled: true }, Validators.required],
            nombre: ['', Validators.required],
            unidadMedida: ['UN', Validators.required],
            activo: [true],
            puntoReposicion: [0, [Validators.required, Validators.min(0)]],
            atributosForm: this.fb.array([])
        });
    }

    ngOnInit(): void {
        this.loadProductos();
        this.loadRubros();

        // Cuando cambie el rubro, cargar familias
        this.productForm.get('idRubro')?.valueChanges.subscribe(idRubro => {
            if (idRubro) {
                this.productForm.get('idFamilia')?.enable();
                this.loadFamilias(Number(idRubro));
            } else {
                this.productForm.get('idFamilia')?.disable();
                if (!this.isEdit) {
                    this.productForm.get('idFamilia')?.setValue('');
                }
                this.familias = [];
            }
        });

        // Cuando cambie la familia, cargar esquema de atributos dinámicos
        this.productForm.get('idFamilia')?.valueChanges.subscribe(idFamilia => {
            if (!this.isEdit) { // Solo reaccionamos si NO es edición (en edición controlamos la carga manualmente)
                if (idFamilia) {
                    this.loadAtributosConfiguracion(Number(idFamilia));
                } else {
                    this.clearAtributosForm();
                }
            }
        });
    }

    get atributosFormArray() {
        return this.productForm.get('atributosForm') as FormArray;
    }

    loadProductos(): void {
        this.loading = true;
        this.productoService.getAll(true).subscribe({
            next: (data) => {
                this.productos = data;
                this.calculateIndicators();
                this.loading = false;
            },
            error: () => {
                this.loading = false;
                Swal.fire('Error', 'No se pudieron cargar los productos', 'error');
            }
        });
    }

    loadRubros(): void {
        this.rubroService.getAll(false).subscribe({
            next: (data) => this.rubros = data
        });
    }

    loadFamilias(idRubro: number): void {
        this.familiaService.getByRubro(idRubro, false).subscribe({
            next: (data) => this.familias = data
        });
    }

    loadAtributosConfiguracion(idFamilia: number, existingValues?: any[]): void {
        this.clearAtributosForm();
        this.atributoService.getAtributosDeFamilia(idFamilia).subscribe({
            next: (data) => {
                this.atributosDinamicos = data;
                data.forEach(fa => {
                    if (fa.tipoDatoAtributo === 'LIST') {
                        this.loadOpcionesLista(fa.idAtributo);
                    }

                    let val = null;
                    if (existingValues) {
                        const ev = existingValues.find(e => e.idAtributo === fa.idAtributo);
                        console.log(`Buscando atributo ID ${fa.idAtributo}`, ev, existingValues);

                        if (ev) {
                            if (fa.tipoDatoAtributo === 'STRING') val = ev.valorTexto;
                            else if (fa.tipoDatoAtributo === 'NUMBER') val = Number(ev.valorNumero);
                            else if (fa.tipoDatoAtributo === 'DECIMAL') val = parseFloat(ev.valorDecimal);
                            else if (fa.tipoDatoAtributo === 'BOOLEAN') val = (ev.valorBool === true || ev.valorBool === 'true');
                            else if (fa.tipoDatoAtributo === 'LIST') val = ev.valorLista ? ev.valorLista.toString() : null;
                        }
                    }

                    if (fa.tipoDatoAtributo === 'BOOLEAN' && val === null) val = false; // Default booleano

                    const validators = fa.obligatorio ? [Validators.required] : [];
                    const formGroupControl = this.fb.group({
                        idAtributo: [fa.idAtributo],
                        tipoDato: [fa.tipoDatoAtributo],
                        nombreAtributo: [fa.nombreAtributo],
                        obligatorio: [fa.obligatorio],
                        valor: [val, validators]
                    });

                    this.atributosFormArray.push(formGroupControl);
                });
            }
        });
    }

    loadOpcionesLista(idAtributo: number): void {
        if (!this.opcionesListas[idAtributo]) {
            this.atributoService.getOpciones(idAtributo).subscribe({
                next: (ops) => this.opcionesListas[idAtributo] = ops
            });
        }
    }

    clearAtributosForm(): void {
        while (this.atributosFormArray.length !== 0) {
            this.atributosFormArray.removeAt(0);
        }
        this.atributosDinamicos = [];
    }

    openCreateModal(): void {
        this.isEdit = false;
        this.currentProductoId = null;
        this.skuAutogenerado = 'Autogenerado al guardar';
        this.productForm.reset({ unidadMedida: 'UN', activo: true, puntoReposicion: 0 });
        this.clearAtributosForm();

        // Enable rubro/familia to allow selection
        this.productForm.get('idRubro')?.enable();

        this.showModal = true;
    }

    openEditModal(p: Producto): void {
        this.isEdit = true;
        this.currentProductoId = p.idProducto;
        this.skuAutogenerado = p.sku;
        this.originalAtributos = p.atributos || [];

        // Necesitamos cargar los atributos de la familia y bindear los valores existentes
        this.loadAtributosConfiguracion(p.idFamilia, p.atributos);

        // Encontrar a qué rubro pertenece esta familia
        let idRubroEncontrado = '';
        const familiaDelProducto = this.familias.find(f => f.idFamilia === p.idFamilia);
        if (familiaDelProducto) {
            idRubroEncontrado = familiaDelProducto.idRubro.toString();
        } else {
            // Si no teníamos las familias cargadas (porque no eligió rubro en una sesión previa), cargamos la familia
            this.familiaService.getAll(false).subscribe(fams => {
                this.familias = fams;
                const fam = fams.find(f => f.idFamilia === p.idFamilia);
                if (fam) {
                    this.productForm.patchValue({ idRubro: fam.idRubro, idFamilia: p.idFamilia });
                }
            });
        }

        this.productForm.patchValue({
            idRubro: idRubroEncontrado,
            idFamilia: p.idFamilia,
            nombre: p.nombre,
            unidadMedida: p.unidadMedida,
            activo: p.activo,
            puntoReposicion: p.puntoReposicion || 0
        });

        // Al editar, no se permite cambiar familia/rubro (generalmente la identidad del producto)
        this.productForm.get('idRubro')?.disable();
        this.productForm.get('idFamilia')?.disable();

        this.showModal = true;
    }

    closeModal(): void {
        this.showModal = false;
    }

    saveProducto(): void {
        if (this.productForm.invalid) {
            Object.keys(this.productForm.controls).forEach(k => this.productForm.controls[k].markAsTouched());
            this.atributosFormArray.controls.forEach(c => c.markAsTouched());
            Swal.fire('Atención', 'Complete los campos obligatorios.', 'warning');
            return;
        }

        const formVal = this.productForm.getRawValue(); // gets disabled values too if needed

        // Mapear arreglo reactivo al modelo esperado por backend
        const attrsPayload: CreateProductoAtributoValor[] = formVal.atributosForm.map((af: any) => {
            let attrVal: CreateProductoAtributoValor = { idAtributo: af.idAtributo };

            if (af.tipoDato === 'STRING') attrVal.valorTexto = af.valor;
            else if (af.tipoDato === 'NUMBER') attrVal.valorNumero = Number(af.valor);
            else if (af.tipoDato === 'DECIMAL') attrVal.valorDecimal = parseFloat(af.valor);
            else if (af.tipoDato === 'BOOLEAN') attrVal.valorBool = af.valor === true || af.valor === 'true';
            else if (af.tipoDato === 'LIST') attrVal.valorLista = af.valor;

            return attrVal;
        });

        if (this.isEdit && this.currentProductoId) {
            const updatePayload = {
                nombre: formVal.nombre,
                unidadMedida: formVal.unidadMedida,
                activo: formVal.activo,
                puntoReposicion: formVal.puntoReposicion,
                atributos: attrsPayload
            };

            const changedMandatory: string[] = [];
            formVal.atributosForm.forEach((af: any) => {
                if (af.obligatorio) {
                    const originalAttr = this.originalAtributos.find(o => o.idAtributo === af.idAtributo);
                    const newAttrDto = attrsPayload.find(a => a.idAtributo === af.idAtributo);

                    let isChanged = false;
                    if (!originalAttr) {
                        isChanged = true;
                    } else {
                        if (af.tipoDato === 'STRING' && (newAttrDto?.valorTexto || '') !== (originalAttr.valorTexto || '')) isChanged = true;
                        if (af.tipoDato === 'NUMBER' && newAttrDto?.valorNumero != originalAttr.valorNumero) isChanged = true;
                        if (af.tipoDato === 'DECIMAL' && newAttrDto?.valorDecimal != originalAttr.valorDecimal) isChanged = true;
                        if (af.tipoDato === 'BOOLEAN') {
                            const originalBool = originalAttr.valorBool === true || originalAttr.valorBool === 'true' as any;
                            if (newAttrDto?.valorBool !== originalBool) isChanged = true;
                        }
                        if (af.tipoDato === 'LIST') {
                            const originalList = originalAttr.valorLista ? originalAttr.valorLista.toString() : '';
                            const newList = newAttrDto?.valorLista ? newAttrDto.valorLista.toString() : '';
                            if (newList !== originalList) isChanged = true;
                        }
                    }

                    if (isChanged) {
                        changedMandatory.push(af.nombreAtributo);
                    }
                }
            });

            const performUpdate = () => {
                this.productoService.update(this.currentProductoId!, updatePayload).subscribe({
                    next: () => {
                        this.loadProductos();
                        this.closeModal();
                        Swal.fire('Éxito', 'Producto actualizado', 'success');
                    },
                    error: (err) => Swal.fire('Error', err.error?.message || 'Hubo un error', 'error')
                });
            };

            if (changedMandatory.length > 0) {
                Swal.fire({
                    title: 'Se recalculará el SKU',
                    text: `El SKU se recalculará para incluir atributo nuevo/modificado: ${changedMandatory.join(', ')}`,
                    icon: 'warning',
                    showCancelButton: true,
                    confirmButtonText: 'Confirmar',
                    cancelButtonText: 'Cancelar'
                }).then((result) => {
                    if (result.isConfirmed) {
                        performUpdate();
                    }
                });
            } else {
                performUpdate();
            }

        } else {
            const createPayload: CreateProducto = {
                idRubro: formVal.idRubro,
                idFamilia: formVal.idFamilia,
                nombre: formVal.nombre,
                unidadMedida: formVal.unidadMedida,
                activo: formVal.activo,
                puntoReposicion: formVal.puntoReposicion,
                atributos: attrsPayload
            };

            this.productoService.create(createPayload).subscribe({
                next: () => {
                    this.loadProductos();
                    this.closeModal();
                    Swal.fire('Éxito', 'Producto creado exitosamente', 'success');
                },
                error: (err) => Swal.fire('Error', err.error?.message || 'Hubo un error', 'error')
            });
        }
    }

    deleteProducto(p: Producto): void {
        Swal.fire({
            title: 'Validación de Seguridad',
            html: `Para dar de baja este producto, escriba textualmente su nombre:<br/><br/><b>${p.nombre}</b>`,
            input: 'text',
            icon: 'warning',
            showCancelButton: true,
            confirmButtonText: 'Dar de baja',
            cancelButtonText: 'Cancelar',
            preConfirm: (inputValue) => {
                if (inputValue !== p.nombre) {
                    Swal.showValidationMessage('El nombre no coincide.');
                    return false;
                }
                return true;
            }
        }).then((result) => {
            if (result.isConfirmed) {
                this.productoService.delete(p.idProducto, p.nombre).subscribe({
                    next: () => {
                        this.loadProductos();
                        Swal.fire('Baja Exitosa', 'El producto ha sido dado de baja (Lógica).', 'success');
                    },
                    error: (err) => Swal.fire('Error', err.error?.message, 'error')
                });
            }
        });
    }

    calculateIndicators(): void {
        this.indicatorTotal = this.productos.length;
        this.indicatorActivos = this.productos.filter(p => p.activo).length;
        this.indicatorInactivos = this.productos.filter(p => !p.activo).length;
    }
}
