import { Routes } from '@angular/router';

import { Home } from './pages/home/home';
import { Movies } from './pages/movies/movies';
import { Theatres } from './pages/theatres/theatres';
import { Login } from './pages/login/login';
export const routes: Routes = [
  { path: '', component: Home },
  { path: 'movies', component: Movies },
  { path: 'theatres', component: Theatres },
  { path: 'login', component: Login }
];