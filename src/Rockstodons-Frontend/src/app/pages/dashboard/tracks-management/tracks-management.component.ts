import { Component } from '@angular/core';
import {
  AbstractControl,
  FormControl,
  FormGroup,
  Validators,
} from '@angular/forms';
import { NzModalService } from 'ng-zorro-antd/modal';
import { NzNotificationService } from 'ng-zorro-antd/notification';
import { Observable, of, take } from 'rxjs';
import { IAlbum } from 'src/app/core/interfaces/albums/album';
import { ITrack } from 'src/app/core/interfaces/tracks/track';
import { ITrackCreateDTO } from 'src/app/core/interfaces/tracks/track-create-dto';
import { ITrackUpdateDTO } from 'src/app/core/interfaces/tracks/track-update-dto';
import { AlbumsService } from 'src/app/core/services/albums.service';
import { TracksService } from 'src/app/core/services/tracks.service';

@Component({
  selector: 'app-tracks-management',
  templateUrl: './tracks-management.component.html',
  styleUrls: ['./tracks-management.component.scss'],
})
export class TracksManagementComponent {
  searchValue = '';
  isLoading = false;
  isTracksSearchTriggerVisible = false;
  tracksData!: ITrackTableData[];
  tracksDisplayData!: ITrackTableData[];
  isTrackCreationModalVisible = false;

  albumsForTracks!: IAlbum[];
  albumsNamesForAutocomplete: string[] = [];
  filteredAlbumsNamesForAutocomplete: string[] = [];

  tracksCreationForm!: FormGroup;
  tracksEditForm!: FormGroup;

  constructor(
    private tracksService: TracksService,
    private albumsService: AlbumsService,
    private nzNotificationService: NzNotificationService,
    private nzModalService: NzModalService
  ) {
    this.buildTracksActionForms();
  }

  buildTracksActionForms(): void {
    this.tracksCreationForm = new FormGroup<ITrackActionForm>({
      name: new FormControl('', {
        validators: Validators.compose([
          Validators.required,
          Validators.minLength(2),
          Validators.maxLength(40),
        ]),
        nonNullable: true,
      }),
      album: new FormControl('', {
        validators: Validators.compose([Validators.required]),
        nonNullable: true,
      }),
    });
    this.tracksEditForm = new FormGroup<ITrackActionForm>({
      name: new FormControl('', {
        validators: Validators.compose([
          Validators.required,
          Validators.minLength(2),
          Validators.maxLength(40),
        ]),
        nonNullable: true,
      }),
      album: new FormControl('', {
        validators: Validators.compose([Validators.required]),
        nonNullable: true,
      }),
    });
  }

  get name(): AbstractControl {
    return this.tracksCreationForm.get('name')!;
  }

  onLoadTracksDataClick(): void {
    this.retrieveTracksData();
  }

  resetTracksSearch(): void {
    this.searchValue = '';
    this.searchForTracks();
  }

  searchForTracks(): void {
    this.isTracksSearchTriggerVisible = false;
    this.tracksDisplayData = this.tracksData.filter(
      (data: ITrackTableData) =>
        data.track.name
          .toLowerCase()
          .indexOf(this.searchValue.toLowerCase()) !== -1
    );
  }

  showTrackCreationModal(): void {
    this.isTrackCreationModalVisible = true;
    this.tracksCreationForm.reset();
  }

  handleOkTrackCreationModal(): void {
    this.onTracksCreationFormSubmit();
  }

  handleCancelTrackCreationModal(): void {
    this.isTrackCreationModalVisible = false;
  }

  showTrackEditModal(trackTableDatum: ITrackTableData): void {
    trackTableDatum.isEditingModalVisible = true;
    this.tracksEditForm.setValue({
      name: trackTableDatum.track.name,
      album: trackTableDatum.track.album?.name,
    });
  }

  handleOkTrackEditModal(trackTableDatum: ITrackTableData): void {
    this.onTracksEditFormSubmit(trackTableDatum.track.id).subscribe((success) => {
      if (success) {
        trackTableDatum.isEditingModalVisible = false;
      }
    });
  }

  handleCancelTrackEditModal(trackTableDatum: ITrackTableData): void {
    trackTableDatum.isEditingModalVisible = false;
  }

  onTracksCreationFormSubmit(): void {
    const trackName: string = this.tracksCreationForm.value.name;
    const album = this.albumsForTracks.find(
      (album) => album.name === this.tracksCreationForm.value.album
    )!;

    const isTrackExisting = album.tracks.some(
      (track) => track.name === trackName
    );

    if (isTrackExisting) {
      this.nzNotificationService.error(
        `Error`,
        `The track ${trackName} already exists under the album ${album.name}!`,
        {
          nzPauseOnHover: true
        }
      );
      return;
    }

    const trackToCreate: ITrackCreateDTO = {
      name: trackName,
      albumId: album.id,
    };

    if (this.tracksCreationForm.valid) {
      this.tracksService
        .createNewTrack(trackToCreate)
        .pipe(take(1))
        .subscribe((response) => {
          let newTrack = response;
          this.nzNotificationService.success(
            `Successful Operation`,
            `The track ${newTrack.name} is created successfully!`,
            {
              nzPauseOnHover: true
            }
          );

          this.isTrackCreationModalVisible = false;
          this.retrieveTracksData();
        });
    } else {
      Object.values(this.tracksCreationForm.controls).forEach((control) => {
        if (control.invalid) {
          control.markAsDirty();
          control.updateValueAndValidity({ onlySelf: true });
        }
      });
    }
  }

  onTracksEditFormSubmit(trackId: string): Observable<boolean> {
    let isTracksEditFormSubmitSuccessful: boolean = true;

    const album = this.albumsForTracks.find(
      (album) => album.name === this.tracksEditForm.value.album
    )!;

    console.log('album');
    console.log(album);

    const trackToEdit: ITrackUpdateDTO = {
      id: trackId,
      name: this.tracksEditForm.value.name,
      albumId: album.id,
    };

    if (this.tracksEditForm.valid) {
      this.tracksService
        .updateTrack(trackToEdit)
        .pipe(take(1))
        .subscribe((response) => {
          let editedTrack = response;
          this.nzNotificationService.success(
            `Successful Operation`,
            `The track ${editedTrack.name} is edited successfully!`,
            {
              nzPauseOnHover: true
            }
          );

          this.retrieveTracksData();
        });
    } else {
      isTracksEditFormSubmitSuccessful = false;
      Object.values(this.tracksEditForm.controls).forEach((control) => {
        if (control.invalid) {
          control.markAsDirty();
          control.updateValueAndValidity({ onlySelf: true });
        }
      });
    }

    return of(isTracksEditFormSubmitSuccessful);
  }

  showTrackRemovalModal(trackToRemove: ITrack): void {
    this.nzModalService.confirm({
      nzTitle: `Do you really wish to remove ${trackToRemove.name}?`,
      nzOkText: 'Yes',
      nzOkType: 'primary',
      nzOkDanger: true,
      nzOnOk: () => this.handleOkTrackRemovalModal(trackToRemove),
      nzCancelText: 'No',
      nzOnCancel: () => this.handleCancelTrackRemovalModal(),
    });
  }

  handleOkTrackRemovalModal(trackToRemove: ITrack): void {
    this.tracksService.deleteTrack(trackToRemove.id).subscribe(() => {
      this.nzNotificationService.success(
        'Successful Operation',
        `The track ${trackToRemove.name} has been removed!`,
        {
          nzPauseOnHover: true
        }
      );
      this.retrieveTracksData();
    });
  }

  handleCancelTrackRemovalModal(): void {
    this.nzNotificationService.info(
      `Aborted Operation`,
      `Track removal cancelled`
    );
  }

  onAlbumsAutocompleteChange(value: string): void {
    this.filteredAlbumsNamesForAutocomplete =
      this.albumsNamesForAutocomplete.filter((albumName) =>
        albumName.toLowerCase().indexOf(value.toLowerCase()) !== -1
      );
  }

  ngOnInit(): void {
    this.retrieveTracksData();
  }

  private retrieveTracksData(): void {
    this.isLoading = true;

    this.albumsService.getAllAlbums().subscribe((data) => {
      this.albumsForTracks = data;
      this.albumsNamesForAutocomplete = this.albumsForTracks
        .filter(album => !album.isDeleted).map(
        (album) => album.name
      );
      this.filteredAlbumsNamesForAutocomplete = this.albumsNamesForAutocomplete;
      console.log('albums for tracks');
      console.log(this.albumsForTracks);
    });

    this.tracksService.getTracksWithFullDetails().subscribe((data) => {
      this.tracksData = [];
      console.log('data');
      console.log(data);
      data
        .filter((track) => !track.isDeleted)
        .map((track) => {
          this.tracksData.push({
            track: track,
            isEditingModalVisible: false,
          });
        });
      this.tracksDisplayData = [...this.tracksData];
      this.isLoading = false;
    });
  }
}

export interface ITrackTableData {
  track: ITrack;
  isEditingModalVisible: boolean;
}

export interface ITrackActionForm {
  name: FormControl<string>;
  album: FormControl<string>;
}
