import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { UserService } from '../../../services/user.service';
import { User } from '../../../models/user.model';
import { RouterModule } from '@angular/router';

@Component({
    selector: 'app-users-list',
    standalone: true,
    imports: [CommonModule, RouterModule],
    templateUrl: './users-list.component.html',
    styleUrls: ['./users-list.component.css']
})
export class UsersListComponent implements OnInit {
    users: User[] = [];

    constructor(private userService: UserService) { }

    ngOnInit(): void {
        this.loadUsers();
    }

    loadUsers(): void {
        this.userService.getAll().subscribe(data => {
            this.users = data;
        });
    }

    toggleStatus(user: User): void {
        this.userService.changeStatus(user.idUsuario, !user.estado).subscribe(updatedUser => {
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
