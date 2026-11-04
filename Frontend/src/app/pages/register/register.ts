import { Component, inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';

import { AuthService } from '../../services/auth';
import { RegisterRequest } from '../../models/auth';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [FormsModule, RouterLink],
  templateUrl: './register.html',
  styleUrl: './register.css'
})
export class Register {

  private authService = inject(AuthService);
  private router = inject(Router);

  fullName = '';
  email = '';
  password = '';
  confirmPassword = '';

  isLoading = false;
  errorMessage = '';

  register(): void {

    this.errorMessage = '';

    if (this.password !== this.confirmPassword) {
      this.errorMessage = 'Passwords do not match.';
      return;
    }

    if (this.password.length < 8) {
      this.errorMessage = 'Password must contain at least 8 characters.';
      return;
    }

    this.isLoading = true;

    const request: RegisterRequest = {
      fullName: this.fullName,
      email: this.email,
      password: this.password,
      confirmPassword: this.confirmPassword
    };

    this.authService.register(request).subscribe({

      next: (response) => {
        console.log('Registration successful:', response);

        this.isLoading = false;

        if (response.roles.includes('Admin')) {
          this.router.navigate(['/theatres']);
        } else {
          this.router.navigate(['/movies']);
        }
      },

      error: (error) => {
        console.error('Registration failed:', error);

        this.errorMessage =
          error.error?.message || 'Registration failed. Please try again.';

        this.isLoading = false;
      }

    });
  }
}