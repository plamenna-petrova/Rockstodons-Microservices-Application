import { Component } from '@angular/core';
import { NzNotificationService } from 'ng-zorro-antd/notification';
import { IAlbumType } from 'src/app/core/interfaces/album-types/album-type';
import { IGenre } from 'src/app/core/interfaces/genres/genre';
import { IPerformer } from 'src/app/core/interfaces/performers/performer';
import { AlbumTypesService } from 'src/app/core/services/album-types.service';
import { AlbumsService } from 'src/app/core/services/albums.service';
import { GenresService } from 'src/app/core/services/genres.service';
import { PerformersService } from 'src/app/core/services/performers.service';
import { TracksService } from 'src/app/core/services/tracks.service';
import { ITrack } from 'src/app/core/interfaces/tracks/track';
import { IAlbum } from 'src/app/core/interfaces/albums/album';
import { FileStorageService } from 'src/app/core/services/file-storage.service';

@Component({
  selector: 'app-recycle-bin',
  templateUrl: './recycle-bin.component.html',
  styleUrls: ['./recycle-bin.component.scss'],
})
export class RecycleBinComponent {
  recycledGenres!: IGenre[];
  recycledAlbumTypes!: IAlbumType[];
  recycledPerformers!: IPerformer[];
  recycledAlbums!: IAlbum[];
  recycledTracks!: ITrack[];

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
    {
      active: false,
      name: 'Tracks',
      disabled: false,
      group: 'tracks',
      list: [] as any[],
    },
  ];

  constructor(
    private genresService: GenresService,
    private albumTypesService: AlbumTypesService,
    private performersService: PerformersService,
    private albumsService: AlbumsService,
    private tracksService: TracksService,
    private fileStorageService: FileStorageService,
    private nzNotificationService: NzNotificationService
  ) {}

  deleteRecycledItemPermanently(item: any, group: string): void {
    switch (group) {
      case 'genres':
        this.genresService.deleteGenrePermanently(item.id).subscribe(() => {
          this.nzNotificationService.success(
            'Successful Operation',
            `The genre ${item.name} has been deleted permanently!`,
            {
              nzPauseOnHover: true,
            }
          );
          this.fileStorageService
            .deleteGenreImage(item.imageFileName)
            .subscribe(() => {});
          this.retrieveRecycledData();
        });
        break;
      case 'albumTypes':
        this.albumTypesService
          .deleteAlbumTypePermanently(item.id)
          .subscribe(() => {
            this.nzNotificationService.success(
              'Successful Operation',
              `The album type ${item.name} has been deleted permanently!`,
              {
                nzPauseOnHover: true,
              }
            );
            this.retrieveRecycledData();
          });
        break;
      case 'performers':
        this.performersService
          .deletePerformerPermanently(item.id)
          .subscribe(() => {
            this.nzNotificationService.success(
              'Successful Operation',
              `The performer ${item.name} has been deleted permanently!`,
              {
                nzPauseOnHover: true,
              }
            );
            this.fileStorageService
              .deletePerformerImage(item.imageFileName)
              .subscribe(() => {});
            this.retrieveRecycledData();
          });
        break;
      case 'albums':
        this.albumsService.deleteAlbumPermanently(item.id).subscribe({
          next: () => {
            this.nzNotificationService.success(
              'Successful Operation',
              `The album ${item.name} has been deleted permanently!`,
              {
                nzPauseOnHover: true,
              }
            );
            this.fileStorageService
              .deleteAlbumImage(item.imageFileName)
              .subscribe(() => {});
          },
          error: (error) => {
            this.nzNotificationService.error(
              'Error',
              "Couldn't delete the album. Please delete all the tracks " +
                'belonging to this album before trying again'
            );
          },
          complete: () => {
            this.retrieveRecycledData();
          },
        });

        break;
      case 'tracks':
        this.tracksService.deleteTrackPermanently(item.id).subscribe(() => {
          this.nzNotificationService.success(
            'Successful Operation',
            `The track ${item.name} has been deleted permanently!`,
            {
              nzPauseOnHover: true,
            }
          );
          this.retrieveRecycledData();
        });
        break;
    }
  }

  restoreRecycledItem(item: any, group: string): void {
    switch (group) {
      case 'genres':
        this.genresService.restoreGenre(item.id).subscribe(() => {
          this.nzNotificationService.success(
            'Successful Operation',
            `The genre ${item.name} has been restored!`,
            {
              nzPauseOnHover: true,
            }
          );
          this.retrieveRecycledData();
        });
        break;
      case 'albumTypes':
        this.albumTypesService.restoreAlbumType(item.id).subscribe(() => {
          this.nzNotificationService.success(
            'Successful Operation',
            `The album type ${item.name} has been restored!`,
            {
              nzPauseOnHover: true,
            }
          );
          this.retrieveRecycledData();
        });
        break;
      case 'performers':
        this.performersService.restorePerformer(item.id).subscribe(() => {
          this.nzNotificationService.success(
            'Successful Operation',
            `The performer ${item.name} has been restored!`,
            {
              nzPauseOnHover: true,
            }
          );
          this.retrieveRecycledData();
        });
        break;
      case 'albums':
        this.albumsService.restoreAlbum(item.id).subscribe(() => {
          this.nzNotificationService.success(
            'Successful Operation',
            `The album ${item.name} has been restored!`,
            {
              nzPauseOnHover: true,
            }
          );
          this.retrieveRecycledData();
        });
        break;
      case 'tracks':
        this.tracksService.restoreTrack(item.id).subscribe(() => {
          this.nzNotificationService.success(
            'Successful Operation',
            `The track ${item.name} has been restored!`,
            {
              nzPauseOnHover: true,
            }
          );
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
    this.tracksService.getTracksWithFullDetails().subscribe((data) => {
      this.recycledTracks = data.filter((track) => track.isDeleted);
      this.recycleBinPanels[4].list = this.recycledTracks;
    });
  }
}
