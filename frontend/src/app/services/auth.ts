import { Injectable, NgZone } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { BehaviorSubject, Observable } from 'rxjs';
import { tap } from 'rxjs/operators';
import { User } from '../models/user';

declare const google: any;

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private apiUrl = 'http://localhost:5139/api/auth'; // Adjust port if needed
  private tokenKey = 'inventory_token';
  private userKey = 'inventory_user';

  private userSubject = new BehaviorSubject<User | null>(this.getUserFromStorage());
  public user$ = this.userSubject.asObservable();

  constructor(private http: HttpClient, private router: Router, private ngZone: NgZone) { }

  initializeGoogleSignIn(buttonElementId: string) {
    if (typeof google === 'undefined' || !google?.accounts?.id) {
      setTimeout(() => this.initializeGoogleSignIn(buttonElementId), 100);
      return;
    }

    google.accounts.id.initialize({
      client_id: '70922431623-9uvscucfm7r8e33c2c5e3dugt1f5isi0.apps.googleusercontent.com', // Replace with real ID
      callback: (response: any) => this.handleGoogleCredential(response)
    });

    const btnElement = document.getElementById(buttonElementId);
    if (btnElement) {
      google.accounts.id.renderButton(
        btnElement,
        { theme: 'outline', size: 'large', type: 'standard', text: 'signin_with' }
      );
    } else {
      console.error('Sign-in button element not found.');
    }
  }

  handleGoogleCredential(response: any) {
    console.log('Google Token:', response.credential);
    // Send to backend
    this.loginWithGoogle(response.credential).subscribe({
      next: () => {
        this.ngZone.run(() => {
          this.router.navigate(['/home']);
        });
      },
      error: (err) => {
        console.error('Login Failed', err);
        const errorMessage = err.error?.message || err.message || 'Unknown error';
        alert(`Login Failed. Status: ${err.status} (${err.statusText})\nDetails: ${errorMessage}\nRaw: ${JSON.stringify(err.error)}`);
      }
    });
  }

  loginWithGoogle(token: string): Observable<any> {
    return this.http.post(`${this.apiUrl}/google-login`, { token }).pipe(
      tap((res: any) => {
        localStorage.setItem(this.tokenKey, res.token);
        localStorage.setItem(this.userKey, JSON.stringify(res.user));
        this.userSubject.next(res.user);
      })
    );
  }

  logout() {
    localStorage.removeItem(this.tokenKey);
    localStorage.removeItem(this.userKey);
    this.userSubject.next(null);
    this.router.navigate(['/login']);
  }

  isAuthenticated(): boolean {
    return !!localStorage.getItem(this.tokenKey);
  }

  private getUserFromStorage(): User | null {
    const user = localStorage.getItem(this.userKey);
    return user ? JSON.parse(user) : null;
  }
}
