import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Movie } from '../models/movie';


@Injectable({
  providedIn: 'root'
})
export class MovieService {

  private http = inject(HttpClient);

private apiUrl = 'https://localhost:7058/api/movies';
  // GET all movies
  getMovies(): Observable<Movie[]> {
    return this.http.get<Movie[]>(this.apiUrl);
  }

  // GET movie by ID
  getMovieById(id: number): Observable<Movie> {
    return this.http.get<Movie>(`${this.apiUrl}/${id}`);
  }

  // POST create movie
  createMovie(movie: Movie): Observable<Movie> {
    return this.http.post<Movie>(this.apiUrl, movie);
  }

  // PUT update movie
  updateMovie(id: number, movie: Movie): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/${id}`, movie);
  }

  // DELETE movie
  deleteMovie(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}