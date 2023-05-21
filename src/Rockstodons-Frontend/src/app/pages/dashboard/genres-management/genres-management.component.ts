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
import { Observable, of, retry, take } from 'rxjs';
import { IGenre } from 'src/app/core/interfaces/genres/genre';
import { IGenreCreateDTO } from 'src/app/core/interfaces/genres/genre-create-dto';
import { IGenreUpdateDTO } from 'src/app/core/interfaces/genres/genre-update-dto';
import { FileStorageService } from 'src/app/core/services/file-storage.service';
import { GenresService } from 'src/app/core/services/genres.service';
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
  selector: 'app-genres-management',
  templateUrl: './genres-management.component.html',
  styleUrls: ['./genres-management.component.scss'],
})
export class GenresManagementComponent {
  searchValue = '';
  isLoading = false;
  isGenresSearchTriggerVisible = false;
  genresData!: IGenreTableData[];
  genresDisplayData!: IGenreTableData[];
  isGenreCreationModalVisible = false;

  genresCreationForm!: FormGroup;
  genresEditForm!: FormGroup;

  genrePresentationImageUploadAPIUrl = `${environment.apiUrl}/storage/upload/genre-image`;
  isGenreCreationPresentationImageUploadButtonVisible = false;
  isGenreEditPresentationImageUploadButtonVisible = false;
  previewImage: string | undefined = '';
  previewVisible = false;
  genrePresentationImagesFileList: NzUploadFile[] = [];
  genreEditPresentationImagesFileList: NzUploadFile[] = [];

  constructor(
    private genresService: GenresService,
    private nzNotificationService: NzNotificationService,
    private nzModalService: NzModalService,
    private nzMessageService: NzMessageService,
    private httpClient: HttpClient,
    private fileStorageService: FileStorageService
  ) {
    this.buildGenresActionForms();
  }

  buildGenresActionForms(): void {
    this.genresCreationForm = new FormGroup<IGenreActionForm>({
      genreName: new FormControl('', {
        validators: Validators.compose([
          Validators.required,
          Validators.minLength(3),
          Validators.maxLength(20),
        ]),
        nonNullable: true,
      }),
    });
    this.genresEditForm = new FormGroup<IGenreActionForm>({
      genreName: new FormControl('', {
        validators: Validators.compose([
          Validators.required,
          Validators.minLength(3),
          Validators.maxLength(20),
        ]),
        nonNullable: true,
      }),
    });
  }

  get genreName(): AbstractControl {
    return this.genresCreationForm.get('genreName')!;
  }

  onLoadGenresDataClick(): void {
    this.retrieveGenresData();
  }

  resetGenresSearch(): void {
    this.searchValue = '';
    this.searchForGenres();
  }

  searchForGenres(): void {
    this.isGenresSearchTriggerVisible = false;
    this.genresDisplayData = this.genresData.filter(
      (data: IGenreTableData) =>
        data.genre.name
          .toLowerCase()
          .indexOf(this.searchValue.toLowerCase()) !== -1
    );
  }

  showGenreCreationModal(): void {
    this.isGenreCreationModalVisible = true;
    this.isGenreCreationPresentationImageUploadButtonVisible = true;
    this.genresCreationForm.reset();
  }

  handleOkGenreCreationModal(): void {
    this.onGenresCreationFormSubmit();
  }

  handleCancelGenreCreationModal(): void {
    this.isGenreCreationModalVisible = false;
  }

  showGenreEditModal(genreTableDatum: IGenreTableData): void {
    genreTableDatum.isEditingModalVisible = true;
    this.isGenreEditPresentationImageUploadButtonVisible = true;
    this.genreEditPresentationImagesFileList = [];

    this.genresEditForm.controls['genreName'].setValue(
      genreTableDatum.genre.name
    );

    if (
      genreTableDatum.genre.imageFileName !== null &&
      genreTableDatum.genre.imageUrl !== null
    ) {
      this.genreEditPresentationImagesFileList[0] = {
        uid: '-1',
        name: genreTableDatum.genre.imageFileName,
        status: 'done',
        url: genreTableDatum.genre.imageUrl,
        thumbUrl: genreTableDatum.genre.imageUrl,
      };

      this.isGenreEditPresentationImageUploadButtonVisible = false;
    }
  }

  handleOkGenreEditModal(genreTableDatum: IGenreTableData): void {
    this.onGenresEditFormSubmit(genreTableDatum.genre.id).subscribe(
      (success) => {
        if (success) {
          genreTableDatum.isEditingModalVisible = false;
        }
      }
    );
  }

  handleCancelGenreEditModal(genreTableDatum: IGenreTableData): void {
    genreTableDatum.isEditingModalVisible = false;
  }

  onGenresCreationFormSubmit(): void {
    const genreName: string = this.genresCreationForm.value.genreName;

    const isGenreExisting = this.genresData.some(
      (data) => data.genre.name === genreName
    );

    const uploadedGenrePresentationImage =
      this.genrePresentationImagesFileList[0];

    const genreToCreate: IGenreCreateDTO = {
      name: genreName,
      imageFileName: uploadedGenrePresentationImage.response.blobDTO.name,
      imageUrl: uploadedGenrePresentationImage.response.blobDTO.uri,
    };

    if (isGenreExisting) {
      this.nzNotificationService.error(
        `Error`,
        `The genre ${genreName} already exists!`,
        {
          nzPauseOnHover: true,
        }
      );
      return;
    }

    if (this.genresCreationForm.valid) {
      this.genresService
        .createNewGenre(genreToCreate)
        .pipe(take(1))
        .subscribe((response) => {
          let newGenre = response;
          this.nzNotificationService.success(
            operationSuccessMessage,
            `The genre ${newGenre.name} is created successfully!`,
            {
              nzPauseOnHover: true,
            }
          );

          this.isGenreCreationModalVisible = false;
          this.retrieveGenresData();
        });
    } else {
      Object.values(this.genresCreationForm.controls).forEach((control) => {
        if (control.invalid) {
          control.markAsDirty();
          control.updateValueAndValidity({ onlySelf: true });
        }
      });
    }
  }

  onGenresEditFormSubmit(genreId: string): Observable<boolean> {
    let isGenresEditFormSubmitSuccessful: boolean = true;

    const genreToEdit = {
      id: genreId,
      name: this.genresEditForm.value.genreName,
    } as IGenreUpdateDTO;

    const uploadedGenrePresentationImage =
      this.genreEditPresentationImagesFileList[0];

    if (uploadedGenrePresentationImage !== undefined) {
      if (
        uploadedGenrePresentationImage.name &&
        uploadedGenrePresentationImage.url
      ) {
        genreToEdit.imageFileName = uploadedGenrePresentationImage.name;
        genreToEdit.imageUrl = uploadedGenrePresentationImage.url;
      } else {
        genreToEdit.imageFileName =
          uploadedGenrePresentationImage.response.blobDTO.name;
        genreToEdit.imageUrl =
          uploadedGenrePresentationImage.response.blobDTO.uri;
      }
    }

    if (this.genresEditForm.valid) {
      this.genresService
        .updateGenre(genreToEdit)
        .pipe(take(1))
        .subscribe((response) => {
          let editedGenre = response;
          this.nzNotificationService.success(
            operationSuccessMessage,
            `The genre ${editedGenre.name} is edited successfully!`,
            {
              nzPauseOnHover: true,
            }
          );
          this.retrieveGenresData();
        });
    } else {
      isGenresEditFormSubmitSuccessful = false;
      Object.values(this.genresEditForm.controls).forEach((control) => {
        if (control.invalid) {
          control.markAsDirty();
          control.updateValueAndValidity({ onlySelf: true });
        }
      });
    }

    return of(isGenresEditFormSubmitSuccessful);
  }

  setMediaUploadHeaders = (nzUplaodFile: NzUploadFile) => {
    return {
      'Content-Type': 'multipart/form-data',
      Accept: 'application/json',
    };
  };

  executeCustomUploadRequest = (nzUploadXHRArgs: NzUploadXHRArgs): any => {
    this.fileStorageService.getGenresImages().subscribe((data: any) => {
      if (
        data.map((image: any) => image.name).includes(nzUploadXHRArgs.file.name)
      ) {
        this.nzMessageService.error(
          `Genre presentation image with the same file name` +
            `${nzUploadXHRArgs.file.name} already exists`
        );
        nzUploadXHRArgs.onError!(
          `Genre presentation image with the same file name`,
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
            this.isGenreCreationPresentationImageUploadButtonVisible = false;
            this.isGenreEditPresentationImageUploadButtonVisible = false;
          }
        },
        (error) => {
          nzUploadXHRArgs.onError!(error, nzUploadXHRArgs.file);
        }
      );
    });
  };

  handleGenreImagePreview = async (
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

  downloadGenrePresentationImage(
    fileToDownload: NzUploadFile
  ): void | undefined {
    let a = document.createElement('a');
    a.href = fileToDownload.thumbUrl!;
    a.download = fileToDownload.name!;
    a.click();
  }

  handleGenrePresentationImageChange(info: any) {
    if (info.type === 'removed') {
      this.isGenreCreationPresentationImageUploadButtonVisible = true;
    } else {
      let fileList = [...info.fileList];

      fileList = fileList.map((file) => {
        if (file.response) {
          file.url = file.response.url;
        }
        return file;
      });

      this.genrePresentationImagesFileList = fileList;
    }
  }

  handleGenreEditPresentationImageChange(info: any) {
    if (info.type === 'removed') {
      this.isGenreEditPresentationImageUploadButtonVisible = true;
    } else {
      let fileList = [...info.fileList];

      fileList = fileList.map((file) => {
        if (file.response) {
          file.url = file.response.url;
        }
        return file;
      });

      this.genreEditPresentationImagesFileList = fileList;
    }
  }

  showGenreRemovalModal(genreToRemove: IGenre): void {
    this.nzModalService.confirm({
      nzTitle: recordRemovalConfirmationModalTitle(genreToRemove.name),
      nzOkText: recordRemovalConfirmationModalOkText,
      nzOkType: recordRemovalConfirmationModalOkType,
      nzOkDanger: recordRemovalConfirmationModalOkDanger,
      nzOnOk: () => this.handleOkGenreRemovalModal(genreToRemove),
      nzCancelText: recordRemovalConfirmationModalCancelText,
      nzOnCancel: () => this.handleCancelGenreRemovalModal(),
    });
  }

  handleOkGenreRemovalModal(genreToRemove: IGenre): void {
    this.genresService.deleteGenre(genreToRemove.id).subscribe(() => {
      this.nzNotificationService.success(
        operationSuccessMessage,
        `The genre ${genreToRemove.name} has been removed!`,
        {
          nzPauseOnHover: true,
        }
      );
      this.retrieveGenresData();
    });
  }

  handleCancelGenreRemovalModal(): void {
    this.nzNotificationService.info(
      removalOperationCancelMessage,
      `Genre removal cancelled`
    );
  }

  ngOnInit(): void {
    this.retrieveGenresData();
  }

  private retrieveGenresData(): void {
    this.isLoading = true;
    this.genresService.getGenresWithFullDetails().subscribe((data) => {
      this.genresData = [];
      data
        .filter((genre) => !genre.isDeleted)
        .map((genre) => {
          this.genresData.push({
            genre: genre,
            isEditingModalVisible: false,
          });
        });
      this.genresDisplayData = [...this.genresData];
      this.isLoading = false;
    });
  }
}

export interface IGenreTableData {
  genre: IGenre;
  isEditingModalVisible: boolean;
}

export interface IGenreActionForm {
  genreName: FormControl<string>;
}
