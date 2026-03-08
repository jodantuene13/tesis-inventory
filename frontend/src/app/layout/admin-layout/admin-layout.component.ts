import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Router } from '@angular/router';
import { AuthService } from '../../services/auth';

@Component({
    selector: 'app-admin-layout',
    standalone: true,
    imports: [CommonModule, RouterModule],
    templateUrl: './admin-layout.component.html',
    styleUrls: ['./admin-layout.component.css']
})
export class AdminLayoutComponent {
    isConfigMenuOpen = false;
    isInventoryMenuOpen = false;

    constructor(private authService: AuthService, private router: Router) { }

    toggleConfigMenu() {
        this.isConfigMenuOpen = !this.isConfigMenuOpen;
    }

    toggleInventoryMenu() {
        this.isInventoryMenuOpen = !this.isInventoryMenuOpen;
    }

    logout() {
        this.authService.logout();
    }
}
