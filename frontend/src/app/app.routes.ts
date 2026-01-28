import { Routes } from '@angular/router';
import { LoginComponent } from './pages/login/login';
import { HomeComponent } from './pages/home/home';
import { authGuard } from './guards/auth-guard';
import { UsersListComponent } from './pages/configuration/users-list/users-list.component';
import { UserFormComponent } from './pages/configuration/user-form/user-form.component';

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
            // Configuration Module
            {
                path: 'configuration',
                children: [
                    { path: 'users', component: UsersListComponent },
                    { path: 'users/new', component: UserFormComponent },
                    { path: 'users/edit/:id', component: UserFormComponent },
                    { path: '', redirectTo: 'users', pathMatch: 'full' }
                ]
            },
            { path: '', redirectTo: 'home', pathMatch: 'full' }
        ]
    },

    { path: '**', redirectTo: 'home' }
];
