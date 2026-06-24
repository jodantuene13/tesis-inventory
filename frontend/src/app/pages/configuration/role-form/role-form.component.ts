import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { RoleService } from '../../../services/role.service';
import { Role, Permiso } from '../../../models/role.model';

@Component({
    selector: 'app-role-form',
    standalone: true,
    imports: [CommonModule, FormsModule, RouterModule],
    templateUrl: './role-form.component.html',
    styles: []
})
export class RoleFormComponent implements OnInit {
    isEditMode = false;
    roleId: number | null = null;
    role: Role = {
        idRol: 0,
        nombreRol: '',
        descripcion: '',
        permisosIds: []
    };

    permisosList: Permiso[] = [];
    permisosPorModulo: { [modulo: string]: Permiso[] } = {};

    constructor(
        private roleService: RoleService,
        private route: ActivatedRoute,
        private router: Router
    ) { }

    ngOnInit(): void {
        this.loadPermisos();
        const id = this.route.snapshot.paramMap.get('id');
        if (id) {
            this.isEditMode = true;
            this.roleId = +id;
            this.loadRole(this.roleId);
        }
    }

    loadPermisos(): void {
        this.roleService.getPermisos().subscribe(data => {
            this.permisosList = data;
            this.agruparPermisos();
        });
    }

    agruparPermisos(): void {
        this.permisosPorModulo = this.permisosList.reduce((acc, curr) => {
            if (!acc[curr.modulo]) acc[curr.modulo] = [];
            acc[curr.modulo].push(curr);
            return acc;
        }, {} as { [modulo: string]: Permiso[] });
    }

    loadRole(id: number): void {
        this.roleService.getById(id).subscribe(r => {
            this.role = {
                idRol: r.idRol,
                nombreRol: r.nombreRol,
                descripcion: r.descripcion,
                permisosIds: r.permisosIds || []
            };
        });
    }

    hasPermiso(id: number): boolean {
        return this.role.permisosIds.includes(id);
    }

    togglePermiso(id: number, event: any): void {
        if (event.target.checked) {
            this.role.permisosIds.push(id);
        } else {
            this.role.permisosIds = this.role.permisosIds.filter(pid => pid !== id);
        }
    }

    onSubmit(): void {
        if (this.isEditMode && this.roleId) {
            this.roleService.update(this.roleId, this.role).subscribe(() => {
                this.router.navigate(['/configuration/roles']);
            });
        } else {
            this.roleService.create(this.role).subscribe(() => {
                this.router.navigate(['/configuration/roles']);
            });
        }
    }

    objectKeys(obj: any): string[] {
        return Object.keys(obj);
    }

    getPermissionLabel(permiso: Permiso): string {
        let label = permiso.descripcion;

        // Quitar prefijo "Ver Informe de " (módulo Informes)
        label = label.replace(/^Ver Informe de /i, '');

        // Quitar sufijos que repiten el nombre del módulo
        const sufijos: Record<string, string[]> = {
            'Solicitudes':  ['Solicitudes de Compra', 'Solicitudes'],
            'Transferencias': ['de Transferencias', 'de Transferencia', 'en Transferencias'],
            'Inventario':   ['de Inventario', 'de Stock Local'],
            'Parametricas': ['de Paramétricas del Gestor'],
            'Informes':     ['de Informes'],
            'Inicio':       ['de Inicio'],
        };

        for (const sufijo of sufijos[permiso.modulo] ?? []) {
            const re = new RegExp('[,\\s]*' + sufijo.replace(/[.*+?^${}()|[\]\\]/g, '\\$&') + '$', 'i');
            label = label.replace(re, '');
        }

        return label.trim() || permiso.descripcion;
    }

    getModuloLabel(modulo: string): string {
        const labels: Record<string, string> = {
            'Parametricas': 'Paramétricas',
            'Configuracion': 'Configuración',
        };
        return labels[modulo] ?? modulo;
    }
}
