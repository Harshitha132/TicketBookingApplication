import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Theatre } from '../models/theatre';

@Injectable({
  providedIn: 'root'
})
export class TheatreService {

  private http = inject(HttpClient);

  private apiUrl = 'https://localhost:7058/api/theatres';

  // Get all theatres
  getTheatres(): Observable<Theatre[]> {
    return this.http.get<Theatre[]>(this.apiUrl);
  }

  // Get theatre by ID
  getTheatreById(id: number): Observable<Theatre> {
    return this.http.get<Theatre>(`${this.apiUrl}/${id}`);
  }
}