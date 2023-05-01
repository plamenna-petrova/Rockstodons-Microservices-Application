import { Component } from '@angular/core';
import { IGenre } from 'src/app/core/interfaces/genres/genre';
import { GenresService } from 'src/app/core/services/genres.service';

@Component({
  selector: 'app-genres-catalogue',
  templateUrl: './genres-catalogue.component.html',
  styleUrls: ['./genres-catalogue.component.scss']
})
export class GenresCatalogueComponent {
  genresForCatalogue!: IGenre[];

  isLoading = false;

  gridSelectionStyle = {
    width: '95%',
    textAlign: 'center'
  }

  fallback = '../../../assets/images/alternative-image.png';

  constructor(private genresService: GenresService) {

  }

  ngOnInit(): void {
    this.retrieveCatalogueData();
  }

  private retrieveCatalogueData(): void {
    this.isLoading = true;
    this.genresService.getAllGenres().subscribe((data) => {
      this.genresForCatalogue = [...data];
      this.isLoading = false;
    });
  }
}
