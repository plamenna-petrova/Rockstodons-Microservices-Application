import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { Observable } from 'rxjs';
import { IAlbum } from 'src/app/core/interfaces/albums/album';
import { IGenre } from 'src/app/core/interfaces/genres/genre';
import { IPerformer } from 'src/app/core/interfaces/performers/performer';
import { AlbumsService } from 'src/app/core/services/albums.service';
import { GenresService } from 'src/app/core/services/genres.service';
import { PerformersService } from 'src/app/core/services/performers.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent {
  albumsSelection!: IAlbum[];
  albumsPrimarySelection!: IAlbum[];
  albumsSecondarySelection!: IAlbum[];
  performersPrimarySelection!: IPerformer[];
  genresPrimarySelection!: IGenre[];

  mastheadBackgroundImageUrl!: String

  isLoading = false;

  gridSelectionStyle = {
    width: '95%',
    textAlign: 'center'
  }

  fallback = '../../../assets/images/alternative-image.png';

  constructor(
    private albumsService: AlbumsService,
    private performersService: PerformersService,
    private genresService: GenresService,
    private router: Router
  ) {

  }

  getAlbumDetails(albumId: string): void {
    const albumDetailsRouteToNavigate = `/album-details/${albumId}`
    this.router.navigate([`${albumDetailsRouteToNavigate}`]);
  }

  getPerformerDetails(performerId: string): void {
    const performerDetailsRouteToNavigate = `/performer-details/${performerId}`;
    this.router.navigate([`${performerDetailsRouteToNavigate}`]);
  }

  ngOnInit(): void {
    this.mastheadBackgroundImageUrl = "../../../assets/images/pexels-edward-eyer-811838.jpg";
    this.retrieveCatalogueData();
  }

  private retrieveCatalogueData(): void {
    this.isLoading = true;
    this.albumsService.getAllAlbums().subscribe((data) => {
      this.albumsSelection = [...data].slice(0, 11);
      this.albumsPrimarySelection = this.albumsSelection.slice(0, 4);
      this.albumsSecondarySelection = this.albumsSelection.slice(4, 10);
    });
    this.performersService.getAllPerformers().subscribe((data) => {
      this.performersPrimarySelection = [...data].slice(0, 6);
    });
    this.genresService.getAllGenres().subscribe((data) => {
      this.genresPrimarySelection = [...data].slice(0, 6);
    });
    this.isLoading = false;
  }
}
