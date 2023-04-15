import { Component } from '@angular/core';
import { IGenre } from 'src/app/core/interfaces/genre';
import { GenresService } from 'src/app/core/services/genres.service';

@Component({
  selector: 'app-genres-management',
  templateUrl: './genres-management.component.html',
  styleUrls: ['./genres-management.component.scss'],
})
export class GenresManagementComponent {
  searchValue = '';
  isLoading = false;
  isGenresSearchTriggerVisible = false;
  genresData!: IGenre[];
  genresDisplayData!: IGenre[];

  constructor(private genresService: GenresService) {

  }

  onLoadGenresDataClick(): void {
    this.retrieveGenresData();
  }

  resetGenresSearch(): void {
    this.searchValue = '';
    this.searchForGenres();
  }

  searchForGenres(): void {
    this.isGenresSearchTriggerVisible = false;
    this.genresDisplayData = this.genresData.filter(
      (genre: IGenre) =>
        genre.name
          .toLowerCase()
          .indexOf(this.searchValue.toLocaleLowerCase()) !== -1
    );
  }

  ngOnInit(): void {
    this.retrieveGenresData();
  }

  private retrieveGenresData(): void {
    this.isLoading = true;
    this.genresService.getGenresWithFullDetails().subscribe((data) => {
      this.genresData = data;
      this.genresDisplayData = [...this.genresData];
      this.isLoading = false;
    });
  }
}
