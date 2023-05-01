import { Component } from '@angular/core';
import { Observable } from 'rxjs';
import { IAlbum } from 'src/app/core/interfaces/albums/album';
import { IApplicationUser } from 'src/app/core/interfaces/users/application-user';
import { AlbumsService } from 'src/app/core/services/albums.service';
import { AuthService } from 'src/app/core/services/auth.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent {
  albumsSelection!: IAlbum[];
  albumsPrimarySelection!: IAlbum[];
  albumsSecondarySelection!: IAlbum[];

  mastheadBackgroundImageUrl!: String

  isLoading = false;

  gridStyleAlbumsPrimarySelection = {
    width: '95%',
    textAlign: 'center'
  }

  gridStyleAlbumsSecondarySelection = {
    width: '95%',
    textAlign: 'center'
  }

  constructor(private albumsService: AlbumsService) {

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
      this.albumsSecondarySelection = this.albumsSelection.slice(4, 11);
      this.isLoading = false;
    });
  }
}
