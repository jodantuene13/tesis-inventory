import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { UserService } from '../../../services/user.service';
import { RoleService } from '../../../services/role.service';
import { SedeService } from '../../../services/sede.service';
import { User } from '../../../models/user.model';
import { RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';

@Component({
    selector: 'app-users-list',
    standalone: true,
    imports: [CommonModule, RouterModule, FormsModule],
    templateUrl: './users-list.component.html',
    styleUrls: ['./users-list.component.css']
})
export class UsersListComponent implements OnInit {
    users: User[] = [];
    roles: any[] = [];
    sedes: any[] = [];

    // Filters
    hasSearched: boolean = false;
    searchTerm: string = '';
    selectedRoleId: number | null = null;
    selectedSedeId: number | null = null;
    selectedStatus: string = ''; // 'true', 'false', or ''

    constructor(
        private userService: UserService,
        private roleService: RoleService,
        private sedeService: SedeService
    ) { }

    ngOnInit(): void {
        this.loadRoles();
        this.loadSedes();
        this.loadUsers();
    }

    loadRoles(): void {
        this.roleService.getAll().subscribe(data => this.roles = data);
    }

    loadSedes(): void {
        this.sedeService.getAll().subscribe(data => this.sedes = data);
    }

    loadUsers(): void {
        const statusBool = this.selectedStatus === '' ? undefined : (this.selectedStatus === 'true');

        this.userService.getAll(
            this.searchTerm || undefined,
            this.selectedRoleId || undefined,
            this.selectedSedeId || undefined,
            statusBool
        ).subscribe(data => {
            this.users = data;
        });
    }

    search(): void {
        this.hasSearched = true;
        this.loadUsers();
    }

    clearFilters(): void {
        this.searchTerm = '';
        this.selectedRoleId = null;
        this.selectedSedeId = null;
        this.selectedStatus = '';
        this.hasSearched = false;
        this.loadUsers();
    }

    toggleStatus(user: User): void {
        this.userService.changeStatus(user.idUsuario, !user.estado).subscribe(updatedUser => {
            // Update local state directly or reload
            user.estado = updatedUser.estado;
        });
    }

    deleteUser(id: number): void {
        if (confirm('¿Está seguro de que desea eliminar este usuario?')) {
            this.userService.delete(id).subscribe(() => {
                this.loadUsers();
            });
        }
    }
}
