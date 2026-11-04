import { FormsModule } from '@angular/forms';
import {
  Component,
  OnInit,
  inject,
  ChangeDetectorRef
} from '@angular/core';

import { MovieService } from '../../services/movie';
import { Movie } from '../../models/movie';

@Component({
  selector: 'app-movies',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './movies.html',
  styleUrl: './movies.css'
})
export class Movies implements OnInit {

  private movieService = inject(MovieService);
  private cdr = inject(ChangeDetectorRef);

  movies: Movie[] = [];
  searchText = '';
filteredMovies: Movie[] = [];

  errorMessage = '';
  isLoading = false;

  ngOnInit(): void {
    
    this.loadMovies();
  }
  searchMovies(): void {
  const search = this.searchText.trim().toLowerCase();

  this.filteredMovies = this.movies.filter(movie =>
    movie.title.toLowerCase().includes(search)
  );
}

  loadMovies(): void {
    this.isLoading = true;

    this.movieService.getMovies().subscribe({

      next: (data) => {
        this.movies = data;

        this.filteredMovies = data;
        this.isLoading = false;

        console.log('Movies loaded:', data);

        this.cdr.markForCheck();
      },

      error: (error) => {
        console.error('Error fetching movies:', error);

        this.errorMessage = 'Unable to load movies. Please try again.';
        this.isLoading = false;

        this.cdr.markForCheck();
      }

    });
  }
}
