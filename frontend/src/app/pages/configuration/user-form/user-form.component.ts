import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { UserService } from '../../../services/user.service';
import { RoleService } from '../../../services/role.service';
import { SedeService } from '../../../services/sede.service';
import { CreateUserDto, UpdateUserDto, User } from '../../../models/user.model';
import { Role } from '../../../models/role.model';
import { Sede } from '../../../models/sede.model';

@Component({
    selector: 'app-user-form',
    standalone: true,
    imports: [CommonModule, FormsModule, RouterModule],
    templateUrl: './user-form.component.html',
    styleUrls: ['./user-form.component.css']
})
export class UserFormComponent implements OnInit {
    isEditMode = false;
    userId: number | null = null;
    user: any = { // Use simplified model for form binding
        nombreUsuario: '',
        email: '',
        password: '',
        idRol: null,
        idSede: null
    };

    roles: Role[] = [];
    sedes: Sede[] = [];

    constructor(
        private userService: UserService,
        private roleService: RoleService,
        private sedeService: SedeService,
        private route: ActivatedRoute,
        private router: Router
    ) { }

    ngOnInit(): void {
        this.loadRoles();
        this.loadSedes();

        const id = this.route.snapshot.paramMap.get('id');
        if (id) {
            this.isEditMode = true;
            this.userId = +id;
            this.loadUser(this.userId);
        }
    }

    loadRoles(): void {
        this.roleService.getAll().subscribe(data => {
            this.roles = data;
        });
    }

    loadSedes(): void {
        this.sedeService.getAll().subscribe(data => {
            this.sedes = data;
        });
    }

    loadUser(id: number): void {
        this.userService.getById(id).subscribe(u => {
            this.user = {
                nombreUsuario: u.nombreUsuario,
                email: u.email,
                idRol: u.idRol,
                idSede: u.idSede
            };
        });
    }

    onSubmit(): void {
        if (this.isEditMode && this.userId) {
            const updateDto: UpdateUserDto = {
                nombreUsuario: this.user.nombreUsuario,
                idRol: this.user.idRol,
                idSede: this.user.idSede
            };
            this.userService.update(this.userId, updateDto).subscribe(() => {
                this.router.navigate(['/configuration/users']);
            });
        } else {
            const createDto: CreateUserDto = {
                nombreUsuario: this.user.nombreUsuario,
                email: this.user.email,
                password: this.user.password,
                idRol: this.user.idRol,
                idSede: this.user.idSede
            };
            this.userService.create(createDto).subscribe(() => {
                this.router.navigate(['/configuration/users']);
            });
        }
    }
}
