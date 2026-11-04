import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, tap } from 'rxjs';

import { LoginRequest, AuthResponse } from '../models/auth';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  private http = inject(HttpClient);

  private apiUrl = 'https://localhost:7058/api/auth';

  // Login
  login(request: LoginRequest): Observable<AuthResponse> {
    return this.http.post<AuthResponse>(
      `${this.apiUrl}/login`,
      request
    ).pipe(
      tap(response => {
        localStorage.setItem('token', response.token);
        localStorage.setItem('user', JSON.stringify(response));
      })
    );
  }

  // Logout
  logout(): void {
    localStorage.removeItem('token');
    localStorage.removeItem('user');
  }

  // Get stored token
  getToken(): string | null {
    return localStorage.getItem('token');
  }

  // Check login status
  isLoggedIn(): boolean {
    return !!this.getToken();
  }

  // Check Admin role
  isAdmin(): boolean {
    const user = localStorage.getItem('user');

    if (!user) {
      return false;
    }

    const authResponse: AuthResponse = JSON.parse(user);

    return authResponse.roles.includes('Admin');
  }
}