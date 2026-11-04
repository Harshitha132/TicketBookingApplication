import { Routes } from '@angular/router';

import { Home } from './pages/home/home';
import { Movies } from './pages/movies/movies';
import { Theatres } from './pages/theatres/theatres';
import { Login } from './pages/login/login';
import { Register } from './pages/register/register';
export const routes: Routes = [
  { path: '', component: Home },
  { path: 'movies', component: Movies },
  { path: 'theatres', component: Theatres },
  { path: 'login', component: Login },
  { path: 'register', component: Register }
];