import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { IAlbum } from 'src/app/core/interfaces/albums/album';
import { AlbumsService } from 'src/app/core/services/albums.service';

@Component({
  selector: 'app-albums-catalogue',
  templateUrl: './albums-catalogue.component.html',
  styleUrls: ['./albums-catalogue.component.scss']
})
export class AlbumsCatalogueComponent {
  albumsForCatalogue!: IAlbum[];

  isLoading = false;

  gridSelectionStyle = {
    width: '95%',
    textAlign: 'center'
  }

  fallback = '../../../assets/images/alternative-image.png';

  constructor(
    private albumsService: AlbumsService,
    private router: Router
  ) {

  }

  getAlbumDetails(albumId: string): void {
    const albumDetailsRouteToNavigate = `/album-details/${albumId}`
    this.router.navigate([`${albumDetailsRouteToNavigate}`]);
  }

  ngOnInit(): void {
    this.retrieveAlbumsCatalogueData();
  }

  private retrieveAlbumsCatalogueData(): void {
    this.isLoading = true;
    this.albumsService.getAllAlbums().subscribe((data) => {
      this.albumsForCatalogue = [...data];
      this.isLoading = false;
    });
  }
}
