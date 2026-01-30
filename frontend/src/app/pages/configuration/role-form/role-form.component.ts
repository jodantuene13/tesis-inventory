import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { RoleService } from '../../../services/role.service';
import { Role } from '../../../models/role.model';

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
    role: any = {
        nombreRol: '',
        descripcion: ''
    };

    constructor(
        private roleService: RoleService,
        private route: ActivatedRoute,
        private router: Router
    ) { }

    ngOnInit(): void {
        const id = this.route.snapshot.paramMap.get('id');
        if (id) {
            this.isEditMode = true;
            this.roleId = +id;
            this.loadRole(this.roleId);
        }
    }

    loadRole(id: number): void {
        this.roleService.getById(id).subscribe(r => {
            this.role = {
                idRol: r.idRol,
                nombreRol: r.nombreRol,
                descripcion: r.descripcion
            };
        });
    }

    onSubmit(): void {
        if (this.isEditMode && this.roleId) {
            const roleToUpdate: Role = {
                idRol: this.roleId,
                nombreRol: this.role.nombreRol,
                descripcion: this.role.descripcion
            };
            this.roleService.update(this.roleId, roleToUpdate).subscribe(() => {
                this.router.navigate(['/configuration/roles']);
            });
        } else {
            const roleToCreate: Role = {
                idRol: 0, // Ignored by backend
                nombreRol: this.role.nombreRol,
                descripcion: this.role.descripcion
            };
            this.roleService.create(roleToCreate).subscribe(() => {
                this.router.navigate(['/configuration/roles']);
            });
        }
    }
}
