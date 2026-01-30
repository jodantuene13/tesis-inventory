import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AuthService } from '../../services/auth';
import { Observable } from 'rxjs';
import { User } from '../../models/user';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [CommonModule],
  template: `
    <div class="home-container">
      <header>
        <h1>Bienvenido al Inventario</h1>
        <button (click)="logout()" class="logout-btn">Cerrar Sesión</button>
      </header>
      <main>
        <div *ngIf="user$ | async as user" class="user-info">
          <h3>Información del Usuario</h3>
          <p><strong>Usuario:</strong> {{ user.nombreUsuario }}</p>
          <p><strong>Email:</strong> {{ user.email }}</p>
          <p><strong>Rol:</strong> {{ user.nombreRol || user.idRol }}</p>
        </div>
      </main>
    </div>
  `,
  styles: [`
    .home-container { max-width: 800px; margin: 0 auto; padding: 2rem; }
    header { display: flex; justify-content: space-between; align-items: center; margin-bottom: 2rem; border-bottom: 1px solid #eee; padding-bottom: 1rem; }
    .logout-btn { background: #dc3545; color: white; border: none; padding: 0.5rem 1rem; border-radius: 4px; cursor: pointer; }
    .user-info { background: #f8f9fa; padding: 1.5rem; border-radius: 8px; }
  `]
})
export class HomeComponent implements OnInit {
  user$!: Observable<User | null>;

  constructor(private authService: AuthService) { }

  ngOnInit() {
    this.user$ = this.authService.user$;
  }

  logout() {
    this.authService.logout();
  }
}
