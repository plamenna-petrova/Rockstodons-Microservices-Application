import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { Observable } from 'rxjs';
import { IAlbum } from 'src/app/core/interfaces/albums/album';
import { IGenre } from 'src/app/core/interfaces/genres/genre';
import { IPerformer } from 'src/app/core/interfaces/performers/performer';
import { IStream } from 'src/app/core/interfaces/streams/stream';
import { AlbumsService } from 'src/app/core/services/albums.service';
import { GenresService } from 'src/app/core/services/genres.service';
import { PerformersService } from 'src/app/core/services/performers.service';
import { StreamsService } from 'src/app/core/services/streams.service';

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
  streamsPrimarySelection!: IStream[];
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
    private streamsService: StreamsService,
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

  getStreamDetails(streamId: string): void {
    const streamDetailsRouteToNavigate = `/stream-details/${streamId}`;
    this.router.navigate([`${streamDetailsRouteToNavigate}`]);
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
    this.streamsService.getStreamsWithFullDetails().subscribe((data) => {
      this.streamsPrimarySelection = [...data].slice(0, 6);
    });
    this.genresService.getAllGenres().subscribe((data) => {
      this.genresPrimarySelection = [...data].slice(0, 6);
    });
    this.isLoading = false;
  }
}
