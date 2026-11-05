import { Component, DestroyRef, inject, OnInit } from '@angular/core';
import { NavigationEnd, Router, RouterLink } from '@angular/router';
import { filter } from 'rxjs';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';

import { AuthService } from '../../services/auth';
import { AuthResponse } from '../../models/auth';

@Component({
  imports: [RouterLink],
  selector: 'app-navbar',
  standalone: true,
  styleUrl: './navbar.css',
  templateUrl: './navbar.html',
})
export class Navbar implements OnInit {

  private authService = inject(AuthService);
  private router = inject(Router);
  private destroyRef = inject(DestroyRef);

  isLoggedIn = false;
  fullName = '';

  ngOnInit(): void {

    this.updateAuthStatus();

    this.router.events
      .pipe(
        filter(event => event instanceof NavigationEnd),
        takeUntilDestroyed(this.destroyRef)
      )
      .subscribe(() => {
        this.updateAuthStatus();
      });
  }

  updateAuthStatus(): void {

    this.isLoggedIn = this.authService.isLoggedIn();

    const user = localStorage.getItem('user');

    if (user) {
      const authResponse: AuthResponse = JSON.parse(user);
      this.fullName = authResponse.fullName;
    } else {
      this.fullName = '';
    }
  }

  logout(): void {

    this.authService.logout();

    this.isLoggedIn = false;
    this.fullName = '';

    this.router.navigate(['/login']);
  }
}