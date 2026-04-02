import { Component, AfterViewInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AuthService } from '../../services/auth';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './login.html',
  styleUrls: ['./login.css']
})
export class LoginComponent implements AfterViewInit {

  constructor(private authService: AuthService) { }

  ngAfterViewInit(): void {
    // Pass the ID of the div where the button will be rendered
    this.authService.initializeGoogleSignIn('google-btn');
  }
}
