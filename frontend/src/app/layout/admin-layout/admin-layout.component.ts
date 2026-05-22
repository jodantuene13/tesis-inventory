import { Component, OnInit, OnDestroy, HostListener, ElementRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Router, NavigationEnd } from '@angular/router';
import { AuthService } from '../../services/auth';
import { SedeService } from '../../services/sede.service';
import { SedeContextService } from '../../services/sede-context.service';
import { Sede } from '../../models/sede.model';
import { Subscription, filter } from 'rxjs';

import { FormsModule } from '@angular/forms';

@Component({
    selector: 'app-admin-layout',
    standalone: true,
    imports: [CommonModule, RouterModule, FormsModule],
    templateUrl: './admin-layout.component.html',
    styleUrls: ['./admin-layout.component.css']
})
export class AdminLayoutComponent implements OnInit, OnDestroy {
    isConfigMenuOpen = false;
    isInventoryMenuOpen = false;
    isTransferenciasMenuOpen = false;
    isParametricasMenuOpen = false;

    /** Controla si el sidebar está abierto en mobile */
    isSidebarOpen = false;

    isAdmin = false;
    sedes: Sede[] = [];
    selectedSedeId!: number;
    isSedeDropdownOpen = false;
    private userSub!: Subscription;
    private contextSub!: Subscription;
    private routerSub!: Subscription;

    constructor(
        private authService: AuthService,
        private router: Router,
        private sedeService: SedeService,
        private sedeContextService: SedeContextService,
        private eRef: ElementRef
    ) { }

    @HostListener('document:click', ['$event'])
    clickout(event: Event) {
        if (this.isSedeDropdownOpen && !this.eRef.nativeElement.contains(event.target)) {
            this.isSedeDropdownOpen = false;
        }
    }

    ngOnInit() {
        this.userSub = this.authService.user$.subscribe(user => {
            if (user) {
                // Determine if user is Admin
                const roleValue = user.nombreRol;
                this.isAdmin = roleValue === 'Admin' || roleValue === 'Administrador';

                if (this.isAdmin) {
                    this.loadSedes();
                }
            }
        });

        this.contextSub = this.sedeContextService.sedeEnContexto$.subscribe(sedeId => {
            this.selectedSedeId = sedeId;
        });

        // Cerrar sidebar automáticamente al navegar en mobile
        this.routerSub = this.router.events
            .pipe(filter(e => e instanceof NavigationEnd))
            .subscribe(() => {
                this.isSidebarOpen = false;
            });
    }

    ngOnDestroy() {
        if (this.userSub) this.userSub.unsubscribe();
        if (this.contextSub) this.contextSub.unsubscribe();
        if (this.routerSub) this.routerSub.unsubscribe();
    }

    loadSedes() {
        this.sedeService.getAll().subscribe(data => {
            this.sedes = data;
        });
    }

    toggleSedeDropdown(event: Event) {
        event.stopPropagation();
        this.isSedeDropdownOpen = !this.isSedeDropdownOpen;
    }

    selectSede(idSede: number) {
        this.sedeContextService.setSedeEnContexto(idSede);
        this.isSedeDropdownOpen = false;
    }

    get selectedSedeName(): string {
        const sede = this.sedes.find(s => s.idSede === this.selectedSedeId);
        return sede ? sede.nombreSede : 'Seleccionar Sede';
    }

    /** Abre/cierra el sidebar en mobile */
    toggleSidebar() {
        this.isSidebarOpen = !this.isSidebarOpen;
    }

    /** Cierra el sidebar (usado por el overlay y links en mobile) */
    closeSidebar() {
        this.isSidebarOpen = false;
    }

    toggleConfigMenu() {
        this.isConfigMenuOpen = !this.isConfigMenuOpen;
    }

    toggleInventoryMenu() {
        this.isInventoryMenuOpen = !this.isInventoryMenuOpen;
    }

    toggleTransferenciasMenu() {
        this.isTransferenciasMenuOpen = !this.isTransferenciasMenuOpen;
    }

    toggleParametricasMenu() {
        this.isParametricasMenuOpen = !this.isParametricasMenuOpen;
    }

    logout() {
        this.authService.logout();
    }
}
