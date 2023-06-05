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
import { StreamsService } from 'src/app/core/services/streams.service';
import { IStream } from 'src/app/core/interfaces/streams/stream';
import { operationSuccessMessage } from 'src/app/core/utils/global-constants';
import { NzModalRef, NzModalService } from 'ng-zorro-antd/modal';

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
  recycledStreams!: IStream[];

  recycleBinConfirmationModal!: NzModalRef;

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
    {
      active: false,
      name: 'Streams',
      disabled: false,
      group: 'stream',
      list: [] as any[],
    },
  ];

  constructor(
    private genresService: GenresService,
    private albumTypesService: AlbumTypesService,
    private performersService: PerformersService,
    private albumsService: AlbumsService,
    private tracksService: TracksService,
    private streamsService: StreamsService,
    private fileStorageService: FileStorageService,
    private nzModalService: NzModalService,
    private nzNotificationService: NzNotificationService
  ) {

  }

  deleteRecycledItemPermanently(item: any, group: string): void {
    switch (group) {
      case 'genres':
        this.recycleBinConfirmationModal = this.nzModalService.confirm({
          nzTitle: 'Do you really wish to delete this genre?',
          nzContent:
            'When you click on the OK button, the genre will be deleted permanently',
          nzOnOk: () => {
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
          },
        });
        break;
      case 'albumTypes':
        this.recycleBinConfirmationModal = this.nzModalService.confirm({
          nzTitle: 'Do you really wish to delete this album type?',
          nzContent:
            'When you click on the OK button, the album type will be deleted permanently',
          nzOnOk: () => {
            this.albumTypesService
              .deleteAlbumTypePermanently(item.id)
              .subscribe(() => {
                this.nzNotificationService.success(
                  operationSuccessMessage,
                  `The album type ${item.name} has been deleted permanently!`,
                  {
                    nzPauseOnHover: true,
                  }
                );
                this.retrieveRecycledData();
              });
          },
        });
        break;
      case 'performers':
        this.recycleBinConfirmationModal = this.nzModalService.confirm({
          nzTitle: 'Do you really wish to delete this performer?',
          nzContent:
            'When you click on the OK button, the performer will be deleted permanently',
          nzOnOk: () => {
            this.performersService
              .deletePerformerPermanently(item.id)
              .subscribe(() => {
                this.nzNotificationService.success(
                  operationSuccessMessage,
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
          },
        });
        break;
      case 'albums':
        this.recycleBinConfirmationModal = this.nzModalService.confirm({
          nzTitle: 'Do you really wish to delete this album?',
          nzContent:
            'When you click on the OK button, the album will be deleted permanently',
          nzOnOk: () => {
            this.albumsService.deleteAlbumPermanently(item.id).subscribe({
              next: () => {
                this.nzNotificationService.success(
                  operationSuccessMessage,
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
                console.log(error);
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
          },
        });
        break;
      case 'tracks':
        this.recycleBinConfirmationModal = this.nzModalService.create({
          nzTitle: 'Do you really wish to delete this track?',
          nzContent:
            'When you click on the OK button, the track will be deleted permanently',
          nzOnOk: () => {
            this.tracksService.deleteTrackPermanently(item.id).subscribe({
              next: () => {
                this.nzNotificationService.success(
                  operationSuccessMessage,
                  `The track ${item.name} has been deleted permanently!`,
                  {
                    nzPauseOnHover: true,
                  }
                );
                this.fileStorageService
                  .deleteTrackMP3File(item.audioFileName)
                  .subscribe(() => {});
              },
              error: (error) => {
                console.log(error);
              },
              complete: () => {
                this.retrieveRecycledData();
              },
            });
          },
        });
        break;
      case 'streams':
        this.recycleBinConfirmationModal = this.nzModalService.create({
          nzTitle: 'Do you really wish to delete this stream?',
          nzContent:
            'When you click on the OK button, the stream will be deleted permanently',
          nzOnOk: () => {
            this.streamsService.deleteStreamPermanently(item.id).subscribe({
              next: () => {
                this.nzNotificationService.success(
                  operationSuccessMessage,
                  `The stream ${item.name} has been deleted permanently!`,
                  {
                    nzPauseOnHover: true,
                  }
                );
                this.fileStorageService
                  .deleteStreamImage(item.imageFileName)
                  .subscribe(() => {});
              },
              error: (error) => {
                console.log(error);
              },
              complete: () => {
                this.retrieveRecycledData();
              },
            });
          },
        });
        break;
    }
  }

  restoreRecycledItem(item: any, group: string): void {
    switch (group) {
      case 'genres':
        this.genresService.restoreGenre(item.id).subscribe(() => {
          this.nzNotificationService.success(
            operationSuccessMessage,
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
            operationSuccessMessage,
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
            operationSuccessMessage,
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
            operationSuccessMessage,
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
            operationSuccessMessage,
            `The track ${item.name} has been restored!`,
            {
              nzPauseOnHover: true,
            }
          );
          this.retrieveRecycledData();
        });
        break;
      case 'streams':
        this.streamsService.restoreStream(item.id).subscribe(() => {
          this.nzNotificationService.success(
            operationSuccessMessage,
            `The stream ${item.name} has been restored!`,
            {
              nzPauseOnHover: true,
            }
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
    this.tracksService.getTracksWithFullDetails().subscribe((data) => {
      this.recycledTracks = data.filter((track) => track.isDeleted);
      this.recycleBinPanels[4].list = this.recycledTracks;
    });
    this.streamsService.getStreamsWithFullDetails().subscribe((data) => {
      this.recycledStreams = data.filter((stream) => stream.isDeleted);
      this.recycleBinPanels[5].list = this.recycledStreams;
    });
  }
}
