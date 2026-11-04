import { Component, OnInit, inject } from '@angular/core';
import { TheatreService } from '../../services/theatre';
import { Theatre } from '../../models/theatre';

@Component({
  selector: 'app-theatres',
  standalone: true,
  imports: [],
  templateUrl: './theatres.html',
  styleUrl: './theatres.css'
})
export class Theatres implements OnInit {

  private theatreService = inject(TheatreService);

  theatres: Theatre[] = [];
  isLoading = false;
  errorMessage = '';

  ngOnInit(): void {
    this.loadTheatres();
  }

  loadTheatres(): void {
    this.isLoading = true;

    this.theatreService.getTheatres().subscribe({
      next: (data) => {
        this.theatres = data;
        this.isLoading = false;

        console.log('Theatres loaded:', data);
      },
      error: (error) => {
        console.error('Error fetching theatres:', error);

        this.errorMessage = 'Unable to load theatres.';
        this.isLoading = false;
      }
    });
  }
}