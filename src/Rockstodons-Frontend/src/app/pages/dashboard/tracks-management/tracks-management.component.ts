import {
  HttpClient,
  HttpEvent,
  HttpEventType,
  HttpRequest,
  HttpResponse,
} from '@angular/common/http';
import { Component } from '@angular/core';
import {
  AbstractControl,
  FormControl,
  FormGroup,
  Validators,
} from '@angular/forms';
import { NzMessageService } from 'ng-zorro-antd/message';
import { NzModalService } from 'ng-zorro-antd/modal';
import { NzNotificationService } from 'ng-zorro-antd/notification';
import { NzUploadFile, NzUploadXHRArgs } from 'ng-zorro-antd/upload';
import { Observable, of, take } from 'rxjs';
import { IAlbum } from 'src/app/core/interfaces/albums/album';
import { ITrack } from 'src/app/core/interfaces/tracks/track';
import { ITrackCreateDTO } from 'src/app/core/interfaces/tracks/track-create-dto';
import { ITrackUpdateDTO } from 'src/app/core/interfaces/tracks/track-update-dto';
import { AlbumsService } from 'src/app/core/services/albums.service';
import { FileStorageService } from 'src/app/core/services/file-storage.service';
import { TracksService } from 'src/app/core/services/tracks.service';
import {
  operationSuccessMessage,
  recordRemovalConfirmationModalCancelText,
  recordRemovalConfirmationModalOkDanger,
  recordRemovalConfirmationModalOkText,
  recordRemovalConfirmationModalOkType,
  recordRemovalConfirmationModalTitle,
  removalOperationCancelMessage,
} from 'src/app/core/utils/global-constants';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-tracks-management',
  templateUrl: './tracks-management.component.html',
  styleUrls: ['./tracks-management.component.scss'],
})
export class TracksManagementComponent {
  isLoading = false;
  isTracksSearchByNameTriggerVisible = false;
  isTracksSearchByAlbumTriggerVisible = false;
  searchByNameValue = '';
  searchByAlbumValue = '';
  tracksData!: ITrackTableData[];
  tracksDisplayData!: ITrackTableData[];
  isTrackCreationModalVisible = false;

  albumsForTracks!: IAlbum[];
  albumsNamesForAutocomplete: string[] = [];
  filteredAlbumsNamesForAutocomplete: string[] = [];

  tracksCreationForm!: FormGroup;
  tracksEditForm!: FormGroup;

  trackAudioFileAPIUrl = `${environment.apiUrl}/storage/upload/track-mp3-file`;
  isTrackAudioFileUploadButtonVisible = false;
  isTrackEditAudioFileUploadButtonVisible = false;
  trackAudioFileList: NzUploadFile[] = [];
  trackEditAudioFileList: NzUploadFile[] = [];

  constructor(
    private tracksService: TracksService,
    private albumsService: AlbumsService,
    private nzNotificationService: NzNotificationService,
    private nzModalService: NzModalService,
    private nzMessageService: NzMessageService,
    private httpClient: HttpClient,
    private fileStorageService: FileStorageService
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
    this.searchByNameValue = '';
    this.searchByAlbumValue = '';
    this.isTracksSearchByNameTriggerVisible = false;
    this.isTracksSearchByAlbumTriggerVisible = false;
    this.retrieveTracksData();
  }

  searchForTracks(): void {
    this.isTracksSearchByNameTriggerVisible = false;
    this.tracksDisplayData = this.tracksData.filter(
      (data: ITrackTableData) =>
        data.track.name
          .toLowerCase()
          .indexOf(this.searchByNameValue.toLowerCase()) !== -1
    );
  }

  searchForTracksByAlbum(): void {
    this.isTracksSearchByAlbumTriggerVisible = false;
    this.tracksDisplayData = this.tracksData.filter(
      (data: ITrackTableData) =>
        data.track.album?.name
          .toLowerCase()
          .indexOf(this.searchByAlbumValue.toLowerCase()) !== -1
    );
  }

  showTrackCreationModal(): void {
    this.isTrackCreationModalVisible = true;
    this.isTrackAudioFileUploadButtonVisible = true;
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
    this.isTrackEditAudioFileUploadButtonVisible = true;
    this.trackEditAudioFileList = [];

    this.tracksEditForm.setValue({
      name: trackTableDatum.track.name,
      album: trackTableDatum.track.album?.name,
    });

    if (
      trackTableDatum.track.audioFileName !== null &&
      trackTableDatum.track.audioFileUrl !== null
    ) {
      this.trackEditAudioFileList[0] = {
        uid: '-1',
        name: trackTableDatum.track.audioFileName,
        status: 'done',
        url: trackTableDatum.track.audioFileUrl,
        thumbUrl: trackTableDatum.track.audioFileUrl,
      };

      this.isTrackEditAudioFileUploadButtonVisible = false;
    }
  }

  handleOkTrackEditModal(trackTableDatum: ITrackTableData): void {
    this.onTracksEditFormSubmit(trackTableDatum.track.id).subscribe(
      (success) => {
        if (success) {
          trackTableDatum.isEditingModalVisible = false;
        }
      }
    );
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
          nzPauseOnHover: true,
        }
      );
      return;
    }

    const uploadedTrackAudioFile = this.trackAudioFileList[0];

    const trackToCreate: ITrackCreateDTO = {
      name: trackName,
      albumId: album.id,
      audioFileName: uploadedTrackAudioFile.response.blobDTO.name,
      audioFileUrl: uploadedTrackAudioFile.response.blobDTO.uri,
    };

    if (this.tracksCreationForm.valid) {
      this.tracksService
        .createNewTrack(trackToCreate)
        .pipe(take(1))
        .subscribe((response) => {
          let newTrack = response;
          
          this.nzNotificationService.success(
            operationSuccessMessage,
            `The track ${newTrack.name} is created successfully!`,
            {
              nzPauseOnHover: true,
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

    const trackToEdit = {
      id: trackId,
      name: this.tracksEditForm.value.name,
      albumId: album.id,
    } as ITrackUpdateDTO;

    const uploadedTrackAudioFile = this.trackEditAudioFileList[0];

    if (uploadedTrackAudioFile !== undefined) {
      if (uploadedTrackAudioFile.name && uploadedTrackAudioFile.url) {
        trackToEdit.audioFileName = uploadedTrackAudioFile.name;
        trackToEdit.audioFileUrl = uploadedTrackAudioFile.url;
      } else {
        trackToEdit.audioFileName =
          uploadedTrackAudioFile.response.blobDTO.name;
        trackToEdit.audioFileUrl = uploadedTrackAudioFile.response.blobDTO.uri;
      }
    }

    if (this.tracksEditForm.valid) {
      this.tracksService
        .updateTrack(trackToEdit)
        .pipe(take(1))
        .subscribe((response) => {
          let editedTrack = response;

          this.nzNotificationService.success(
            operationSuccessMessage,
            `The track ${editedTrack.name} is edited successfully!`,
            {
              nzPauseOnHover: true,
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

  setMediaUploadHeaders = (nzUplaodFile: NzUploadFile) => {
    return {
      'Content-Type': 'multipart/form-data',
      Accept: 'application/json',
    };
  };

  executeCustomUploadRequest = (nzUploadXHRArgs: NzUploadXHRArgs): any => {
    this.fileStorageService.getTracksMP3Files().subscribe((data: any) => {
      if (
        data
          .map((mp3File: any) => mp3File.name)
          .includes(nzUploadXHRArgs.file.name)
      ) {
        this.nzMessageService.error(
          `Track mp3 file with the same file name` +
            `${nzUploadXHRArgs.file.name} already exists`
        );
        nzUploadXHRArgs.onError!(
          `Track mp3 file with the same file name`,
          nzUploadXHRArgs.file
        );
        return;
      }

      const formData = new FormData();
      formData.append('mp3FileToUpload', nzUploadXHRArgs.file as any);

      const fileUploadRequest = new HttpRequest(
        'POST',
        nzUploadXHRArgs.action!,
        formData,
        {
          reportProgress: true,
          withCredentials: false,
        }
      );

      return this.httpClient.request(fileUploadRequest).subscribe({
        next: (event: HttpEvent<unknown>) => {
          if (event.type === HttpEventType.UploadProgress) {
            if (event.total! > 0) {
              (event as any).percent = event.loaded;
            }
            nzUploadXHRArgs.onProgress!(event, nzUploadXHRArgs.file);
          } else if (event instanceof HttpResponse) {
            nzUploadXHRArgs.onSuccess!(event.body, nzUploadXHRArgs.file, event);
            this.isTrackAudioFileUploadButtonVisible = false;
            this.isTrackEditAudioFileUploadButtonVisible = false;
          }
        },
        error: (error) => {
          nzUploadXHRArgs.onError!(error, nzUploadXHRArgs.file);
        },
      });
    });
  };

  downloadTrackAudioFile(fileToDownload: NzUploadFile): void | undefined {
    let a = document.createElement('a');
    a.href = fileToDownload.thumbUrl!;
    a.download = fileToDownload.name!;
    a.click();
  }

  handleTrackAudioFileChange(info: any) {
    if (info.type === 'removed') {
      this.isTrackAudioFileUploadButtonVisible = true;
    } else {
      let fileList = [...info.fileList];

      fileList = fileList.map((file) => {
        if (file.response) {
          file.url = file.response.url;
        }
        return file;
      });

      this.trackAudioFileList = fileList;
    }
  }

  handleTrackEditAudioFileChange(info: any) {
    if (info.type === 'removed') {
      this.isTrackEditAudioFileUploadButtonVisible = true;
    } else {
      let fileList = [...info.fileList];

      fileList = fileList.map((file) => {
        if (file.response) {
          file.url = file.response.url;
        }
        return file;
      });

      this.trackEditAudioFileList = fileList;
    }
  }

  showTrackRemovalModal(trackToRemove: ITrack): void {
    this.nzModalService.confirm({
      nzTitle: recordRemovalConfirmationModalTitle(trackToRemove.name),
      nzOkText: recordRemovalConfirmationModalOkText,
      nzOkType: recordRemovalConfirmationModalOkType,
      nzOkDanger: recordRemovalConfirmationModalOkDanger,
      nzOnOk: () => this.handleOkTrackRemovalModal(trackToRemove),
      nzCancelText: recordRemovalConfirmationModalCancelText,
      nzOnCancel: () => this.handleCancelTrackRemovalModal(),
    });
  }

  handleOkTrackRemovalModal(trackToRemove: ITrack): void {
    this.tracksService.deleteTrack(trackToRemove.id).subscribe(() => {
      this.nzNotificationService.success(
        operationSuccessMessage,
        `The track ${trackToRemove.name} has been removed!`,
        {
          nzPauseOnHover: true,
        }
      );
      this.retrieveTracksData();
    });
  }

  handleCancelTrackRemovalModal(): void {
    this.nzNotificationService.info(
      removalOperationCancelMessage,
      `Track removal cancelled`
    );
  }

  onAlbumsAutocompleteChange(value: string): void {
    this.filteredAlbumsNamesForAutocomplete =
      this.albumsNamesForAutocomplete.filter(
        (albumName) =>
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
        .filter((album) => !album.isDeleted)
        .map((album) => album.name);
      this.filteredAlbumsNamesForAutocomplete = this.albumsNamesForAutocomplete;
    });

    this.tracksService.getTracksWithFullDetails().subscribe((data) => {
      this.tracksData = [];
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
