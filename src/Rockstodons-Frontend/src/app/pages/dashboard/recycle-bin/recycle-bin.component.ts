import { Component } from '@angular/core';
import { NzNotificationService } from 'ng-zorro-antd/notification';
import { IAlbum } from 'src/app/core/interfaces/album';
import { IAlbumDetails } from 'src/app/core/interfaces/album-details';
import { IAlbumType } from 'src/app/core/interfaces/album-type';
import { IGenre } from 'src/app/core/interfaces/genre';
import { IPerformer } from 'src/app/core/interfaces/performer';
import { AlbumTypesService } from 'src/app/core/services/album-types.service';
import { AlbumsService } from 'src/app/core/services/albums.service';
import { GenresService } from 'src/app/core/services/genres.service';
import { PerformersService } from 'src/app/core/services/performers.service';

@Component({
  selector: 'app-recycle-bin',
  templateUrl: './recycle-bin.component.html',
  styleUrls: ['./recycle-bin.component.scss'],
})
export class RecycleBinComponent {
  recycledGenres!: IGenre[];
  recycledAlbumTypes!: IAlbumType[];
  recycledPerformers!: IPerformer[];
  recycledAlbums!: IAlbumDetails[];

  recycleBinPanels = [
    {
      active: false,
      name: 'Genres',
      disabled: false,
      group: 'genres',
      list: [] as any[],
    },
    {
      active: false,
      name: 'Album Types',
      disabled: false,
      group: 'albumTypes',
      list: [] as any[],
    },
    {
      active: false,
      name: 'Performers',
      disabled: false,
      group: 'performers',
      list: [] as any[],
    },
    {
      active: false,
      name: 'Albums',
      disabled: false,
      group: 'albums',
      list: [] as any[],
    },
  ];

  constructor(
    private genresService: GenresService,
    private albumTypesService: AlbumTypesService,
    private performersService: PerformersService,
    private albumsService: AlbumsService,
    private nzNotificationService: NzNotificationService
  ) {}

  deleteRecycledItemPermanently(item: any, group: string): void {
    console.log(item);
    console.log(group);
    switch (group) {
      case 'genres':
        this.genresService.deleteGenrePermanently(item.id).subscribe(() => {
          this.nzNotificationService.success(
            'Successful Operation',
            `The genre ${item.name} has been deleted permanently!`
          );
          this.retrieveRecycledData();
        });
        break;
      case 'albumTypes':
        this.albumTypesService
          .deleteAlbumTypePermanently(item.id)
          .subscribe(() => {
            this.nzNotificationService.success(
              'Success Operation',
              `The album type ${item.name} has been deleted permanently!`
            );
            this.retrieveRecycledData();
          });
        break;
      case 'performers':
        this.performersService
          .deletePerformerPermanently(item.id)
          .subscribe(() => {
            this.nzNotificationService.success(
              'Success Operation',
              `The performer ${item.name} has been deleted permanently!`
            );
            this.retrieveRecycledData();
          });
        break;
      case 'albums':
        this.albumsService.deleteAlbumPermanently(item.id).subscribe(() => {
          this.nzNotificationService.success(
            'Success Operation',
            `The album ${item.name} has been deleted permanently!`
          );
          this.retrieveRecycledData();
        });
        break;
    }
  }

  ngOnInit(): void {
    this.retrieveRecycledData();
  }

  private retrieveRecycledData(): void {
    this.genresService.getGenresWithFullDetails().subscribe((data) => {
      this.recycledGenres = data.filter((genre) => genre.isDeleted);
      this.recycleBinPanels[0].list = this.recycledGenres;
    });
    this.albumTypesService.getAlbumTypesWithFullDetails().subscribe((data) => {
      this.recycledAlbumTypes = data.filter((albumType) => albumType.isDeleted);
      this.recycleBinPanels[1].list = this.recycledAlbumTypes;
    });
    this.performersService.getPerformersWithFullDetails().subscribe((data) => {
      this.recycledPerformers = data.filter((performer) => performer.isDeleted);
      this.recycleBinPanels[2].list = this.recycledPerformers;
    });
    this.albumsService.getAlbumsWithFullDetails().subscribe((data) => {
      this.recycledAlbums = data.filter((album) => album.isDeleted);
      this.recycleBinPanels[3].list = this.recycledAlbums;
    });
  }
}
