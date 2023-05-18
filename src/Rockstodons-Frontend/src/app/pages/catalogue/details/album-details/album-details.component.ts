import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { NzDescriptionsSize } from 'ng-zorro-antd/descriptions';
import { take } from 'rxjs';
import { IAlbum } from 'src/app/core/interfaces/albums/album';
import { AlbumsService } from 'src/app/core/services/albums.service';

@Component({
  selector: 'app-album-details',
  templateUrl: './album-details.component.html',
  styleUrls: ['./album-details.component.scss']
})
export class AlbumDetailsComponent {
  id!: string;
  albumDetails!: IAlbum;
  size: NzDescriptionsSize = 'default';

  fallback = '../../../assets/images/alternative-image.png';

  constructor(
    private albumsService: AlbumsService,
    private activatedRoute: ActivatedRoute
  ) {
    this.id = this.activatedRoute.snapshot.paramMap.get('id')!;
  }

  ngOnInit(): void {
    this.albumsService.getAlbumById(this.id).pipe(
      take(1)
    ).subscribe((response) => {
      this.albumDetails = response;
    });
  }
}
