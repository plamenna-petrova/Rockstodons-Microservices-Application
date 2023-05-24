import {
  HttpClient,
  HttpEvent,
  HttpEventType,
  HttpRequest,
  HttpResponse,
} from '@angular/common/http';
import { Component, TemplateRef } from '@angular/core';
import {
  AbstractControl,
  FormControl,
  FormGroup,
  UntypedFormControl,
  Validators,
} from '@angular/forms';
import { NzMessageService } from 'ng-zorro-antd/message';
import { NzModalRef, NzModalService } from 'ng-zorro-antd/modal';
import { NzNotificationService } from 'ng-zorro-antd/notification';
import { NzUploadFile, NzUploadXHRArgs } from 'ng-zorro-antd/upload';
import { Observable, of, take } from 'rxjs';
import { IStream } from 'src/app/core/interfaces/streams/stream';
import { IStreamCreateDTO } from 'src/app/core/interfaces/streams/stream-create-dto';
import { IStreamUpdateDTO } from 'src/app/core/interfaces/streams/stream-update-dto';
import { ITrack } from 'src/app/core/interfaces/tracks/track';
import { FileStorageService } from 'src/app/core/services/file-storage.service';
import { StreamsService } from 'src/app/core/services/streams.service';
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
  selector: 'app-streams-management',
  templateUrl: './streams-management.component.html',
  styleUrls: ['./streams-management.component.scss'],
})
export class StreamsManagementComponent {
  searchValue = '';
  isLoading = false;
  isGenresSearchTriggerVisible = false;
  streamsData!: IStreamTableData[];
  streamsDisplayData!: IStreamTableData[];
  tracksForStreams!: ITrack[];

  isStreamCreationModalVisible = false;
  isStreamRemovalModalVisible = false;

  streamsCreationForm!: FormGroup;
  streamsEditForm!: FormGroup;

  streamCoverImageUploadAPIUrl = `${environment.apiUrl}/storage/upload/stream-image`;
  isStreamCreationCoverImageUploadButtonVisible = false;
  isStreamEditCoverImageUploadButtonVisible = false;
  previewImage: string | undefined = '';
  previewVisible = false;
  streamCoverImagesFileList: NzUploadFile[] = [];
  streamEditCoverImagesFileList: NzUploadFile[] = [];

  listOfTracksSelectOptions: Array<{ label: string; value: ITrack }> = [];
  listOfStreamTracksTagsOptions: ITrack[] = [];
  listOfTracksEditSelectOptions: Array<{ label: string; value: ITrack }> = [];
  listOfStreamTracksEditTagsOptions: ITrack[] = [];

  isTemplateModalButtonLoading = false;

  constructor(
    private streamsService: StreamsService,
    private tracksService: TracksService,
    private httpClient: HttpClient,
    private nzModalService: NzModalService,
    private nzNotificationService: NzNotificationService,
    private nzMessageService: NzMessageService,
    private fileStorageService: FileStorageService
  ) {
    this.buildStreamsActionForms();
  }

  buildStreamsActionForms(): void {
    this.streamsCreationForm = new FormGroup<IStreamActionForm>({
      name: new FormControl('', {
        validators: Validators.compose([
          Validators.required,
          Validators.minLength(5),
          Validators.maxLength(30),
        ]),
        nonNullable: true,
      }),
      tracks: new UntypedFormControl(),
    });
    this.streamsEditForm = new FormGroup<IStreamActionForm>({
      name: new FormControl('', {
        validators: Validators.compose([
          Validators.required,
          Validators.minLength(5),
          Validators.maxLength(30),
        ]),
        nonNullable: true,
      }),
      tracks: new UntypedFormControl(),
    });
  }

  get name(): AbstractControl {
    return this.streamsCreationForm.get('name')!;
  }

  get tracks(): AbstractControl {
    return this.streamsCreationForm.get('tracks')!;
  }

  showStreamCreationModal(): void {
    this.isStreamCreationModalVisible = true;
    this.isStreamCreationCoverImageUploadButtonVisible = true;
    this.streamsCreationForm.reset();
  }

  handleOkStreamCreationModal(): void {
    this.onStreamsCreationFormSubmit();
  }

  handleCancelStreamCreationModal(): void {
    this.isStreamCreationModalVisible = false;
  }

  showStreamEditModal(streamTableDatum: IStreamTableData): void {
    streamTableDatum.isEditingModalVisible = true;

    this.streamsEditForm.patchValue({
      name: streamTableDatum.stream.name,
    });

    this.isStreamEditCoverImageUploadButtonVisible = true;
    this.streamEditCoverImagesFileList = [];

    if (
      streamTableDatum.stream.imageFileName !== null &&
      streamTableDatum.stream.imageUrl !== null
    ) {
      this.streamEditCoverImagesFileList[0] = {
        uid: '-1',
        name: streamTableDatum.stream.imageFileName,
        status: 'done',
        url: streamTableDatum.stream.imageUrl,
      };

      this.isStreamEditCoverImageUploadButtonVisible = false;
    }

    const tracksEditSelectList: Array<{ label: string; value: ITrack }> = [];
    const existingStreamTracksToSelect: ITrack[] = [];

    const streamTracksIds = streamTableDatum.stream.tracks.map(track => track.id);

    this.tracksForStreams.forEach((trackForStream) => {
      tracksEditSelectList.push({
        label:
          `${trackForStream.name} (from ${trackForStream.album!.name} ` +
          `by ${trackForStream.album!.performer.name})`,
        value: trackForStream,
      });

      if (streamTracksIds.includes(trackForStream.id)) {
        existingStreamTracksToSelect.push(trackForStream);
      }
    });

    this.listOfTracksEditSelectOptions = [...tracksEditSelectList];

    this.streamsEditForm.patchValue({
      tracks: existingStreamTracksToSelect,
    });

    this.listOfStreamTracksEditTagsOptions = [...existingStreamTracksToSelect];
  }

  handleOkStreamEditModal(streamTableDatum: IStreamTableData): void {
    this.onStreamsEditFormSubmit(streamTableDatum.stream.id).subscribe(
      (success) => {
        if (success) {
          streamTableDatum.isEditingModalVisible = false;
        }
      }
    );
  }

  handleCancelStreamEditModal(streamTableDatum: IStreamTableData): void {
    streamTableDatum.isEditingModalVisible = false;
  }

  onStreamsCreationFormSubmit(): void {
    const name: string = this.streamsCreationForm.value.name;

    const isStreamExisting = this.streamsDisplayData.some(
      (data) => data.stream.name === name
    );

    if (isStreamExisting) {
      this.nzNotificationService.error(
        `Error`,
        `The stream ${name} already exists!`
      );
      return;
    }

    const uploadedStreamCoverImage = this.streamCoverImagesFileList[0];

    const streamToCreate: IStreamCreateDTO = {
      name: this.streamsCreationForm.value.name,
      imageFileName: uploadedStreamCoverImage.response.blobDTO.name,
      imageUrl: uploadedStreamCoverImage.response.blobDTO.uri,
      tracks: this.streamsCreationForm.value.tracks,
    };

    if (this.streamsCreationForm.valid) {
      this.streamsService
        .createNewStream(streamToCreate)
        .pipe(take(1))
        .subscribe({
          next: (response) => {
            let newStream = response;

            this.nzNotificationService.success(
              operationSuccessMessage,
              `The stream ${newStream.name} is created successfully!`,
              {
                nzPauseOnHover: true,
              }
            );

            this.isStreamCreationModalVisible = false;
            this.retrieveStreamsData();
          },
          error: (error) => {
            console.log(error);
          },
        });
    } else {
      Object.values(this.streamsCreationForm.controls).forEach((control) => {
        if (control.invalid) {
          control.markAsDirty();
          control.updateValueAndValidity({ onlySelf: true });
        }
      });
    }
  }

  setMediaUploadHeaders = (nzUplaodFile: NzUploadFile) => {
    return {
      'Content-Type': 'multipart/form-data',
      Accept: 'application/json',
    };
  };

  executeCustomUploadRequest = (nzUploadXHRArgs: NzUploadXHRArgs): any => {
    this.fileStorageService.getStreamsImages().subscribe((data: any) => {
      if (
        data.map((image: any) => image.name).includes(nzUploadXHRArgs.file.name)
      ) {
        this.nzMessageService.error(
          `Stream Cover Image with the same file name` +
            `${nzUploadXHRArgs.file.name} already exists`
        );
        nzUploadXHRArgs.onError!(
          `Album Cover Image with the same file name`,
          nzUploadXHRArgs.file
        );
        return;
      }

      const formData = new FormData();
      formData.append('imageToUpload', nzUploadXHRArgs.file as any);

      const fileUploadRequest = new HttpRequest(
        'POST',
        nzUploadXHRArgs.action!,
        formData,
        {
          reportProgress: true,
          withCredentials: false,
        }
      );

      return this.httpClient.request(fileUploadRequest).subscribe(
        (event: HttpEvent<unknown>) => {
          if (event.type === HttpEventType.UploadProgress) {
            if (event.total! > 0) {
              (event as any).percent = event.loaded;
            }
            nzUploadXHRArgs.onProgress!(event, nzUploadXHRArgs.file);
          } else if (event instanceof HttpResponse) {
            nzUploadXHRArgs.onSuccess!(event.body, nzUploadXHRArgs.file, event);
            this.isStreamCreationCoverImageUploadButtonVisible = false;
            this.isStreamEditCoverImageUploadButtonVisible = false;
          }
        },
        (error) => {
          nzUploadXHRArgs.onError!(error, nzUploadXHRArgs.file);
        }
      );
    });
  };

  handleStreamImagePreview = async (
    nzUploadFile: NzUploadFile
  ): Promise<void> => {
    if (!nzUploadFile.url && !nzUploadFile['preview']) {
      nzUploadFile['preview'] = await this.getBase64(
        nzUploadFile.originFileObj!
      );
    }
    this.previewImage = nzUploadFile.url || nzUploadFile['preview'];
    this.previewVisible = true;
  };

  getBase64 = (file: File): Promise<string | ArrayBuffer | null> =>
    new Promise((resolve, reject) => {
      const fileReader = new FileReader();
      fileReader.readAsDataURL(file);
      fileReader.onload = () => resolve(fileReader.result);
      fileReader.onerror = (error) => reject(error);
    });

  downloadStreamCoverImage(fileToDownload: NzUploadFile): void | undefined {
    let a = document.createElement('a');
    a.href = fileToDownload.thumbUrl!;
    a.download = fileToDownload.name!;
    a.click();
  }

  handleStreamCoverImageChange(info: any) {
    if (info.type === 'removed') {
      this.isStreamCreationCoverImageUploadButtonVisible = true;
    } else {
      let fileList = [...info.fileList];

      fileList = fileList.map((file) => {
        if (file.response) {
          file.url = file.response.url;
        }
        return file;
      });

      this.streamCoverImagesFileList = fileList;
    }
  }

  handleStreamEditCoverImageChange(info: any) {
    if (info.type === 'removed') {
      this.isStreamEditCoverImageUploadButtonVisible = true;
    } else {
      let fileList = [...info.fileList];

      fileList = fileList.map((file) => {
        if (file.response) {
          file.url = file.response.url;
        }
        return file;
      });

      this.streamEditCoverImagesFileList = fileList;
    }
  }

  createStreamTracksDetailsModal(
    streamTracksDetailsTemplateTitle: TemplateRef<{}>,
    streamTracksDetailsTemplateContent: TemplateRef<{}>,
    streamTracksDetailsTemplateFooter: TemplateRef<{}>
  ): void {
    this.nzModalService.create({
      nzTitle: streamTracksDetailsTemplateTitle,
      nzContent: streamTracksDetailsTemplateContent,
      nzFooter: streamTracksDetailsTemplateFooter,
      nzMaskClosable: true,
      nzClosable: true,
    });
  }

  destroyStreamTracksDetailsTemplateModal(nzModalRef: NzModalRef): void {
    this.isTemplateModalButtonLoading = true;
    setTimeout(() => {
      this.isTemplateModalButtonLoading = false;
      nzModalRef.destroy();
    }, 100);
  }

  onStreamsEditFormSubmit(streamId: string): Observable<boolean> {
    let isStreamsEditFormSubmitSuccessful = true;

    const streamToEdit = {
      id: streamId,
      name: this.streamsEditForm.value.name,
      tracks: this.listOfStreamTracksEditTagsOptions,
    } as IStreamUpdateDTO;

    const uploadedStreamCoverImage = this.streamEditCoverImagesFileList[0];

    if (uploadedStreamCoverImage !== undefined) {
      if (uploadedStreamCoverImage.name && uploadedStreamCoverImage.url) {
        streamToEdit.imageFileName = uploadedStreamCoverImage.name;
        streamToEdit.imageUrl = uploadedStreamCoverImage.url;
      } else {
        streamToEdit.imageFileName =
          uploadedStreamCoverImage.response.blobDTO.name;
        streamToEdit.imageUrl = uploadedStreamCoverImage.response.blobDTO.uri;
      }
    }

    if (this.streamsEditForm.valid) {
      this.streamsService
        .updateStream(streamToEdit)
        .pipe(take(1))
        .subscribe((response) => {
          let editedStream = response;

          this.nzNotificationService.success(
            operationSuccessMessage,
            `The stream ${editedStream.name} is edited successfully`,
            {
              nzPauseOnHover: true,
            }
          );

          this.retrieveStreamsData();
        });
    } else {
      isStreamsEditFormSubmitSuccessful = false;
      Object.values(this.streamsEditForm.controls).forEach((control) => {
        if (control.invalid) {
          control.markAsDirty();
          control.updateValueAndValidity({ onlySelf: true });
        }
      });
    }

    return of(isStreamsEditFormSubmitSuccessful);
  }

  onLoadStreamsDataClick(): void {
    this.retrieveStreamsData();
  }

  showStreamRemovalModal(streamToRemove: IStream): void {
    this.nzModalService.confirm({
      nzTitle: recordRemovalConfirmationModalTitle(streamToRemove.name),
      nzOkText: recordRemovalConfirmationModalOkText,
      nzOkType: recordRemovalConfirmationModalOkType,
      nzOkDanger: recordRemovalConfirmationModalOkDanger,
      nzOnOk: () => this.handleOkStreamRemovalModal(streamToRemove),
      nzCancelText: recordRemovalConfirmationModalCancelText,
      nzOnCancel: () => this.handleCancelStreamRemovalModal(),
    });
  }

  handleOkStreamRemovalModal(streamToRemove: IStream): void {
    this.streamsService.deleteStream(streamToRemove.id).subscribe(() => {
      this.nzNotificationService.success(
        operationSuccessMessage,
        `The stream ${streamToRemove.name} has been removed!`,
        {
          nzPauseOnHover: true,
        }
      );
      this.retrieveStreamsData();
    });
  }

  handleCancelStreamRemovalModal(): void {
    this.nzNotificationService.info(
      removalOperationCancelMessage,
      `Stream removal cancelled`
    );
  }

  ngOnInit(): void {
    this.retrieveStreamsData();
  }

  private retrieveStreamsData(): void {
    this.isLoading = true;
    this.streamsService.getStreamsWithFullDetails().subscribe((data) => {
      this.streamsData = [];
      data
        .filter((stream) => !stream.isDeleted)
        .map((stream: any) => {
          this.streamsData.push({
            stream: {
              id: stream.id,
              name: stream.name,
              imageFileName: stream.imageFileName,
              imageUrl: stream.imageUrl,
              tracks: stream.streamTracks.map(
                (streamTrack: any) => streamTrack.track
              ),
            } as IStream,
            isEditingModalVisible: false,
          });
        });
      this.streamsDisplayData = [...this.streamsData];
      this.tracksService.getAllTracks().subscribe((data) => {
        this.tracksForStreams = data.filter(
          (track) => track.audioFileName !== null && track.audioFileUrl !== null
        );
        const tracksSelectList: Array<{ label: string; value: ITrack }> = [];
        this.tracksForStreams.forEach((trackForStream) => {
          tracksSelectList.push({
            label:
              `${trackForStream.name} (from ${trackForStream.album!.name} ` +
              `by ${trackForStream.album!.performer.name})`,
            value: trackForStream,
          });
        });
        this.listOfTracksSelectOptions = tracksSelectList;
        this.isLoading = false;
      });
    });
  }
}

export interface IStreamTableData {
  stream: IStream;
  isEditingModalVisible: boolean;
}

export interface IStreamActionForm {
  name: FormControl<string>;
  tracks: UntypedFormControl;
}
