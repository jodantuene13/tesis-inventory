import { Component, AfterViewInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from '../../services/auth';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './login.html',
  styleUrls: ['./login.css']
})
export class LoginComponent implements AfterViewInit {
  testEmail: string = '';
  loginError: string = '';

  constructor(private authService: AuthService, private router: Router) { }

  ngAfterViewInit(): void {
    // Pass the ID of the div where the button will be rendered
    this.authService.initializeGoogleSignIn('google-btn');
  }

  onTestLogin() {
    this.loginError = '';
    if (!this.testEmail) {
      this.loginError = 'Ingrese un correo electrónico';
      return;
    }

    this.authService.loginWithEmail(this.testEmail).subscribe({
      next: () => {
        this.router.navigate(['/home']);
      },
      error: (err) => {
        console.error('Test Login Failed', err);
        this.loginError = err.error?.message || 'Error de inicio de sesión de prueba.';
      }
    });
  }
}
