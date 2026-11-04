import { Component, inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';

import { AuthService } from '../../services/auth';
import { LoginRequest } from '../../models/auth';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [FormsModule, RouterLink],
  templateUrl: './login.html',
  styleUrl: './login.css'
})
export class Login {

  private authService = inject(AuthService);
  private router = inject(Router);

  email = '';
  password = '';

  isLoading = false;
  errorMessage = '';

  login(): void {

    this.errorMessage = '';
    this.isLoading = true;

    const request: LoginRequest = {
      email: this.email,
      password: this.password
    };

    this.authService.login(request).subscribe({

      next: (response) => {
        console.log('Login successful:', response);

        this.isLoading = false;

        if (response.roles.includes('Admin')) {
          this.router.navigate(['/theatres']);
        } else {
          this.router.navigate(['/movies']);
        }
      },

      error: (error) => {
        console.error('Login failed:', error);

        this.errorMessage =
          error.error?.message || 'Invalid email or password.';

        this.isLoading = false;
      }

    });
  }
}