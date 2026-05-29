import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { RoleService } from '../../../services/role.service';
import { Role, Permiso } from '../../../models/role.model';
import { SedeService } from '../../../services/sede.service';
import { Sede } from '../../../models/sede.model';

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
        todasLasSedes: false,
        limitarOperacionSedePrimaria: false,
        permisosIds: [],
        sedesIds: []
    };

    sedes: Sede[] = [];
    permisosList: Permiso[] = [];
    permisosPorModulo: { [modulo: string]: Permiso[] } = {};

    constructor(
        private roleService: RoleService,
        private sedeService: SedeService,
        private route: ActivatedRoute,
        private router: Router
    ) { }

    ngOnInit(): void {
        this.loadSedes();
        this.loadPermisos();
        const id = this.route.snapshot.paramMap.get('id');
        if (id) {
            this.isEditMode = true;
            this.roleId = +id;
            this.loadRole(this.roleId);
        }
    }

    loadSedes(): void {
        this.sedeService.getAll().subscribe(data => {
            this.sedes = data;
        });
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
                todasLasSedes: r.todasLasSedes,
                limitarOperacionSedePrimaria: r.limitarOperacionSedePrimaria,
                permisosIds: r.permisosIds || [],
                sedesIds: r.sedesIds || []
            };
        });
    }
    
    // Funciones para checkboxes
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

    hasSede(id: number): boolean {
        return this.role.sedesIds.includes(id);
    }

    toggleSede(id: number, event: any): void {
        if (event.target.checked) {
            this.role.sedesIds.push(id);
        } else {
            this.role.sedesIds = this.role.sedesIds.filter(sid => sid !== id);
        }
    }

    // Handlers para lógica condicional
    onTodasLasSedesChange(): void {
        if (this.role.todasLasSedes) {
            this.role.sedesIds = [];
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
}
