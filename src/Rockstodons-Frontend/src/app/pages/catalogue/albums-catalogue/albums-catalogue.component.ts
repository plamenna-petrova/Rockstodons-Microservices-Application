import { Component } from '@angular/core';
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

  gridStyle = {
    width: '300px',
    textAlign: 'center'
  }

  constructor(private albumsService: AlbumsService) {

  }

  ngOnInit(): void {
    this.retrieveCatalogueData();
  }

  private retrieveCatalogueData(): void {
    this.isLoading = true;
    this.albumsService.getAllAlbums().subscribe((data) => {
      this.albumsForCatalogue = [...data];
      this.isLoading = false;
    });
  }
}
