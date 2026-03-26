import { Routes } from '@angular/router';
import { LoginComponent } from './pages/login/login';
import { HomeComponent } from './pages/home/home';
import { authGuard } from './guards/auth-guard';
import { UsersListComponent } from './pages/configuration/users-list/users-list.component';
import { UserFormComponent } from './pages/configuration/user-form/user-form.component';
// Roles
import { RolesListComponent } from './pages/configuration/roles-list/roles-list.component';
import { RoleFormComponent } from './pages/configuration/role-form/role-form.component';

import { AdminLayoutComponent } from './layout/admin-layout/admin-layout.component';

export const routes: Routes = [
    { path: 'login', component: LoginComponent },

    // Main App with Layout
    {
        path: '',
        component: AdminLayoutComponent,
        canActivate: [authGuard],
        children: [
            { path: 'home', component: HomeComponent },
            // Inventory Module
            {
                path: 'inventory',
                children: [
                    { path: 'rubros-familias', loadComponent: () => import('./pages/inventory/rubros-familias/rubros-familias.component').then(m => m.RubrosFamiliasComponent) },
                    { path: 'atributos', loadComponent: () => import('./pages/inventory/atributos/atributos.component').then(m => m.AtributosComponent) },
                    { path: 'productos', loadComponent: () => import('./pages/inventory/productos/productos.component').then(m => m.ProductosComponent) },
                    { path: 'stock', loadComponent: () => import('./pages/inventory/stock/stock').then(m => m.StockComponent) },
                    { path: '', redirectTo: 'productos', pathMatch: 'full' }
                ]
            },
            // Configuration Module
            {
                path: 'configuration',
                children: [
                    { path: 'users', component: UsersListComponent },
                    { path: 'users/new', component: UserFormComponent },
                    { path: 'users/edit/:id', component: UserFormComponent },
                    // Roles Routes
                    { path: 'roles', component: RolesListComponent },
                    { path: 'roles/new', component: RoleFormComponent },
                    { path: 'roles/edit/:id', component: RoleFormComponent },
                    { path: 'audit-log', loadComponent: () => import('./pages/configuration/audit-log/audit-log.component').then(m => m.AuditLogComponent) },

                    // Sedes Routes
                    { path: 'sedes', loadComponent: () => import('./pages/configuration/sedes/sedes.component').then(m => m.SedesComponent) },

                    { path: '', redirectTo: 'users', pathMatch: 'full' }
                ]
            },
            { path: '', redirectTo: 'home', pathMatch: 'full' }
        ]
    },

    { path: '**', redirectTo: 'home' }
];
