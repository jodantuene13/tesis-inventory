import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RoleService } from '../../../services/role.service';
import { Role } from '../../../models/role.model';
import { RouterModule } from '@angular/router';

@Component({
    selector: 'app-roles-list',
    standalone: true,
    imports: [CommonModule, RouterModule],
    templateUrl: './roles-list.component.html',
    styles: []
})
export class RolesListComponent implements OnInit {
    roles: Role[] = [];
    errorMessage: string = '';

    constructor(private roleService: RoleService) { }

    ngOnInit(): void {
        this.loadRoles();
    }

    loadRoles(): void {
        this.roleService.getAll().subscribe(data => {
            this.roles = data;
        });
    }

    deleteRole(id: number): void {
        if (confirm('¿Está seguro de que desea eliminar este rol?')) {
            this.roleService.delete(id).subscribe({
                next: () => {
                    this.loadRoles();
                    this.errorMessage = '';
                },
                error: (err) => {
                    if (err.status === 409) {
                        this.errorMessage = 'No se puede eliminar el rol porque está asignado a uno o más usuarios.';
                    } else {
                        this.errorMessage = 'Ocurrió un error al intentar eliminar el rol.';
                    }
                    setTimeout(() => this.errorMessage = '', 5000); // Clear message after 5s
                }
            });
        }
    }
}
