import { Component, ElementRef, TemplateRef, ViewChild } from '@angular/core';
import {
  AbstractControl,
  FormArray,
  FormControl,
  FormGroup,
  UntypedFormControl,
  UntypedFormGroup,
  Validators,
} from '@angular/forms';
import { NzModalRef, NzModalService } from 'ng-zorro-antd/modal';
import { NzNotificationService } from 'ng-zorro-antd/notification';
import {
  NzTableLayout,
  NzTablePaginationPosition,
  NzTablePaginationType,
  NzTableSize,
} from 'ng-zorro-antd/table';
import { Observable, Observer, of, take } from 'rxjs';
import { IAlbum } from 'src/app/core/interfaces/albums/album';
import { IAlbumCreateDTO } from 'src/app/core/interfaces/albums/album-create-dto';
import { IAlbumType } from 'src/app/core/interfaces/album-types/album-type';
import { IAlbumUpdateDTO } from 'src/app/core/interfaces/albums/album-update-dto';
import { IGenre } from 'src/app/core/interfaces/genres/genre';
import { IPerformer } from 'src/app/core/interfaces/performers/performer';
import { AlbumTypesService } from 'src/app/core/services/album-types.service';
import { AlbumsService } from 'src/app/core/services/albums.service';
import { GenresService } from 'src/app/core/services/genres.service';
import { PerformersService } from 'src/app/core/services/performers.service';
import { TracksService } from 'src/app/core/services/tracks.service';
import { ITrackCreateDTO } from 'src/app/core/interfaces/tracks/track-create-dto';
import { ITrack } from 'src/app/core/interfaces/tracks/track';
import {
  operationSuccessMessage,
  recordRemovalConfirmationModalCancelText,
  recordRemovalConfirmationModalOkDanger,
  recordRemovalConfirmationModalOkText,
  recordRemovalConfirmationModalOkType,
  recordRemovalConfirmationModalTitle,
  removalOperationCancelMessage,
} from 'src/app/core/utils/global-constants';

import { FileSaverService } from 'ngx-filesaver';

import * as XLSX from 'xlsx';

import jsPDF from 'jspdf';
import autoTable from 'jspdf-autotable';

import {
  HeadingLevel,
  Packer,
  Paragraph,
  Document,
  TextRun,
  Table,
  TableCell,
  TableRow,
  PageOrientation,
  AlignmentType,
} from 'docx';

import pptxgen from 'pptxgenjs';
import html2canvas from 'html2canvas';
import { NzMessageService } from 'ng-zorro-antd/message';
import { NzUploadFile, NzUploadXHRArgs } from 'ng-zorro-antd/upload';
import {
  HttpClient,
  HttpEvent,
  HttpEventType,
  HttpRequest,
  HttpResponse,
} from '@angular/common/http';
import { FileStorageService } from 'src/app/core/services/file-storage.service';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-albums-management',
  templateUrl: './albums-management.component.html',
  styleUrls: ['./albums-management.component.scss'],
})
export class AlbumsManagementComponent {
  albumsManagementTableSettingsForm!: FormGroup;
  isLoading = false;
  performersForAlbums!: IPerformer[];
  genresForAlbums!: IGenre[];
  typesForAlbums!: IAlbumType[];
  albumsToManage: IAlbumTableData[] = [];
  albumsDisplayData: readonly IAlbumTableData[] = [];
  allSettingsChecked = false;
  indeterminate = false;
  fixedColumn = false;
  scrollX: string | null = null;
  scrollY: string | null = null;
  albumsManagementTableSetting!: IAlbumsManagementTableSetting;

  isVisible = false;
  isConfirmLoading = false;
  isAlbumCreationModalVisible = false;
  isAlbumRemovalModalVisible = false;

  albumsCreationForm!: FormGroup;
  albumsEditForm!: FormGroup;

  selectedExportFormatValue = 'csv';

  currentYear = new Date().getFullYear();

  listOfTrackControls: Array<{ id: number; controlInstance: string }> = [];
  listOfTrackEditControls: Array<{ id: number; controlInstance: string }> = [];

  isTemplateModalButtonLoading = false;

  listOfSwitches = [
    { name: 'Bordered', formControlName: 'isBordered' },
    { name: 'Pagination', formControlName: 'hasPagination' },
    { name: 'PageSizeChanger', formControlName: 'hasSizeChanger' },
    { name: 'Title', formControlName: 'hasTitle' },
    { name: 'Column Header', formControlName: 'hasHeader' },
    { name: 'Footer', formControlName: 'hasFooter' },
    { name: 'Expandable', formControlName: 'isExpandable' },
    { name: 'Checkbox', formControlName: 'withCheckbox' },
    { name: 'Fixed Header', formControlName: 'hasFixedHeader' },
    { name: 'Ellipsis', formControlName: 'withEllipsis' },
    { name: 'Simple Pagination', formControlName: 'isSimple' },
  ];

  listOfRadioButtons = [
    {
      name: 'Size',
      formControlName: 'tableSize',
      listOfOptions: [
        { value: 'default', label: 'Default' },
        { value: 'middle', label: 'Middle' },
        { value: 'small', label: 'Small' },
      ],
    },
    {
      name: 'Table Scroll',
      formControlName: 'tableScroll',
      listOfOptions: [
        { value: 'unset', label: 'Unset' },
        { value: 'scroll', label: 'Scroll' },
        { value: 'fixed', label: 'Fixed' },
      ],
    },
    {
      name: 'Table Layout',
      formControlName: 'tableLayout',
      listOfOptions: [
        { value: 'auto', label: 'Auto' },
        { value: 'fixed', label: 'Fixed' },
      ],
    },
    {
      name: 'Pagination Position',
      formControlName: 'position',
      listOfOptions: [
        { value: 'top', label: 'Top' },
        { value: 'bottom', label: 'Bottom' },
        { value: 'both', label: 'Both' },
      ],
    },
    {
      name: 'Pagination Type',
      formControlName: 'paginationType',
      listOfOptions: [
        { value: 'default', label: 'Default' },
        { value: 'small', label: 'Small' },
      ],
    },
  ];

  albumExportHeaders = [
    'Name',
    'Number Of Tracks',
    'Year Of Release',
    'Album Type',
    'Genre',
    'Performer',
  ];

  tableExportOptions = {
    csv: '.csv',
    txt: '.txt',
    xlsx: '.xlsx',
    pdf: '.pdf',
    docx: '.docx',
    json: '.json',
    xml: '.xml',
    pptx: '.pptx',
    png: '.png',
    jpg: '.jpg',
    jpeg: '.jpeg',
    webp: '.webp',
    gif: '.gif',
    apng: '.apng',
    avif: '.avif',
  };

  fileTypes = {
    csv: 'text/csv',
    tsv: 'text/tsv',
    txt: 'text/plain',
    xlsx: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet',
    pdf: 'application/pdf',
    docx: 'application/vnd.openxmlformats-officedocument.wordprocessingml.document',
    json: 'application/json',
    xml: 'application/xml',
    pptx: 'application/vnd.openxmlformats-officedocument.presentationml.presentation',
    png: 'image/png',
    jpg: 'image/jpg',
    jpeg: 'image/jpeg',
    webp: 'image/webp',
    gif: 'image/gif',
    apng: 'image/apng',
    avif: 'image/avif',
  };

  albumTypesNamesForAutocomplete: string[] = [];
  genresNamesForAutocomplete: string[] = [];
  performersNamesForAutocomplete: string[] = [];

  filteredAlbumTypesNamesForAutocomplete: string[] = [];
  filteredGenresNamesForAutocomplete: string[] = [];
  filteredPerformersNamesForAutocomplete: string[] = [];

  @ViewChild('albumsManagementDynamicTable', {
    static: false,
    read: ElementRef,
  })
  albumsManagementDynamicTable: ElementRef | undefined;

  @ViewChild('canvas', { static: false, read: ElementRef }) canvas:
    | ElementRef
    | undefined;

  @ViewChild('downloadLink', { static: false, read: ElementRef })
  downloadLink: ElementRef | undefined;

  existingAlbumCoverImages: any[] = [];

  albumCoverImageUploadAPIUrl = `${environment.apiUrl}/storage/upload/album-image`;
  isAlbumCreationCoverImageUploadButtonVisible = false;
  isAlbumEditCoverImageUploadButtonVisible = false;
  previewImage: string | undefined = '';
  previewVisible = false;
  albumCoverImagesFileList: NzUploadFile[] = [];
  albumEditCoverImagesFileList: NzUploadFile[] = [];

  constructor(
    private performersService: PerformersService,
    private albumsService: AlbumsService,
    private genresService: GenresService,
    private albumTypesService: AlbumTypesService,
    private tracksService: TracksService,
    private nzNotificationService: NzNotificationService,
    private nzModalService: NzModalService,
    private fileSaverService: FileSaverService,
    private nzMessageService: NzMessageService,
    private httpClient: HttpClient,
    private fileStorageService: FileStorageService
  ) {
    this.buildAlbumsActionForms();
  }

  buildAlbumsActionForms(): void {
    this.albumsCreationForm = new FormGroup<IAlbumActionForm>({
      name: new FormControl('', {
        validators: Validators.compose([
          Validators.required,
          Validators.minLength(2),
          Validators.maxLength(40),
        ]),
        nonNullable: true,
      }),
      yearOfRelease: new FormControl(this.currentYear, {
        validators: Validators.compose([
          Validators.required,
          Validators.min(1960),
          Validators.max(2023),
        ]),
        nonNullable: true,
      }),
      numberOfTracks: new FormControl(1, {
        validators: Validators.compose([
          Validators.required,
          Validators.min(1),
          Validators.max(20),
        ]),
        nonNullable: true,
      }),
      description: new FormControl('', {
        validators: Validators.compose([
          Validators.required,
          Validators.minLength(20),
          Validators.maxLength(500),
        ]),
        nonNullable: true,
      }),
      albumType: new FormControl('', {
        validators: Validators.compose([Validators.required]),
        nonNullable: true,
      }),
      genre: new FormControl('', {
        validators: Validators.compose([Validators.required]),
        nonNullable: true,
      }),
      performer: new FormControl('', {
        validators: Validators.compose([Validators.required]),
        nonNullable: true,
      }),
      tracksActionFormGroup: new UntypedFormGroup({}),
    });
    this.albumsEditForm = new FormGroup<IAlbumActionForm>({
      name: new FormControl('', {
        validators: Validators.compose([
          Validators.required,
          Validators.minLength(2),
          Validators.maxLength(40),
        ]),
        nonNullable: true,
      }),
      yearOfRelease: new FormControl(this.currentYear, {
        validators: Validators.compose([
          Validators.required,
          Validators.min(1960),
          Validators.max(2023),
        ]),
        nonNullable: true,
      }),
      numberOfTracks: new FormControl(1, {
        validators: Validators.compose([
          Validators.required,
          Validators.min(1),
          Validators.max(20),
        ]),
        nonNullable: true,
      }),
      description: new FormControl('', {
        validators: Validators.compose([
          Validators.required,
          Validators.minLength(20),
          Validators.maxLength(500),
        ]),
        nonNullable: true,
      }),
      albumType: new FormControl('', {
        validators: Validators.compose([Validators.required]),
        nonNullable: true,
      }),
      genre: new FormControl('', {
        validators: Validators.compose([Validators.required]),
        nonNullable: true,
      }),
      performer: new FormControl('', {
        validators: Validators.compose([Validators.required]),
        nonNullable: true,
      }),
      tracksActionFormGroup: new UntypedFormGroup({}),
    });
  }

  get isBordered(): AbstractControl {
    return this.albumsManagementTableSettingsForm.get('isBordered')!;
  }

  get withLoading(): AbstractControl {
    return this.albumsManagementTableSettingsForm.get('withLoading')!;
  }

  get hasPagination(): AbstractControl {
    return this.albumsManagementTableSettingsForm.get('hasPagination')!;
  }

  get hasSizeChanger(): AbstractControl {
    return this.albumsManagementTableSettingsForm.get('hasSizeChanger')!;
  }

  get hasTitle(): AbstractControl {
    return this.albumsManagementTableSettingsForm.get('hasTitle')!;
  }

  get hasHeader(): AbstractControl {
    return this.albumsManagementTableSettingsForm.get('hasHeader')!;
  }

  get hasFooter(): AbstractControl {
    return this.albumsManagementTableSettingsForm.get('hasFooter')!;
  }

  get isExpandable(): AbstractControl {
    return this.albumsManagementTableSettingsForm.get('isExpandable')!;
  }

  get withCheckbox(): AbstractControl {
    return this.albumsManagementTableSettingsForm.get('withCheckbox')!;
  }

  get hasFixedHeader(): AbstractControl {
    return this.albumsManagementTableSettingsForm.get('hasFixedHeader')!;
  }

  get withEllipsis(): AbstractControl {
    return this.albumsManagementTableSettingsForm.get('withEllipsis')!;
  }

  get isSimple(): AbstractControl {
    return this.albumsManagementTableSettingsForm.get('isSimple')!;
  }

  get tableSize(): AbstractControl {
    return this.albumsManagementTableSettingsForm.get('tableSize')!;
  }

  get paginationType(): AbstractControl {
    return this.albumsManagementTableSettingsForm.get('paginationType')!;
  }

  get tableScroll(): AbstractControl {
    return this.albumsManagementTableSettingsForm.get('tableScroll')!;
  }

  get tableLayout(): AbstractControl {
    return this.albumsManagementTableSettingsForm.get('tableLayout')!;
  }

  get position(): AbstractControl {
    return this.albumsManagementTableSettingsForm.get('position')!;
  }

  get name(): AbstractControl {
    return this.albumsCreationForm.get('name')!;
  }

  get yearOfRelease(): AbstractControl {
    return this.albumsCreationForm.get('yearOfRelease')!;
  }

  get description(): AbstractControl {
    return this.albumsCreationForm.get('description')!;
  }

  get albumType(): AbstractControl {
    return this.albumsCreationForm.get('albumType')!;
  }

  get genre(): AbstractControl {
    return this.albumsCreationForm.get('genre')!;
  }

  get performer(): AbstractControl {
    return this.albumsCreationForm.get('performer')!;
  }

  get tracksActionFormGroup(): any {
    return this.albumsCreationForm.get('tracksActionFormGroup')!;
  }

  get tracksEditActionFormGroup(): any {
    return this.albumsEditForm.get('tracksActionFormGroup')!;
  }

  onCurrentPageDataChange($event: readonly IAlbumTableData[]): void {
    this.albumsDisplayData = $event;
    this.refreshAlbumsManagementSettingsStatus();
  }

  checkAll(value: boolean): void {
    this.albumsDisplayData.forEach((data) => {
      if (!data.disabled) {
        data.checked = value;
      }
    });
    this.refreshAlbumsManagementSettingsStatus();
  }

  refreshAlbumsManagementSettingsStatus(): void {
    const validAlbumData = this.albumsDisplayData.filter(
      (value) => !value.disabled
    );
    const allChecked =
      validAlbumData.length > 0 &&
      validAlbumData.every((value) => value.checked);
    const allUnchecked = validAlbumData.every((value) => !value.checked);
    this.allSettingsChecked = allChecked;
    this.indeterminate = !allChecked && !allUnchecked;
  }

  showAlbumCreationModal(): void {
    this.isAlbumCreationModalVisible = true;
    this.isAlbumCreationCoverImageUploadButtonVisible = true;
    this.albumsCreationForm.reset();

    this.listOfTrackControls.forEach((control, i) => {
      if (i === 0) {
        const controlToErase =
          this.tracksActionFormGroup.controls[control.controlInstance];
        controlToErase.setValue(null);
      } else {
        const index = this.listOfTrackControls.indexOf(control);
        this.listOfTrackControls.splice(index, 1);
        this.tracksActionFormGroup.removeControl(control.controlInstance);
      }
    });
  }

  handleOkAlbumCreationModal(): void {
    this.onAlbumsCreationFormSubmit();
  }

  handleCancelAlbumCreationModal(): void {
    this.isAlbumCreationModalVisible = false;
  }

  createAlbumDetailsTemplateModal(
    albumDetailsTemplateTitle: TemplateRef<{}>,
    albumDetailsTemplateContent: TemplateRef<{}>,
    albumDetailsTemplateFooter: TemplateRef<{}>
  ): void {
    this.nzModalService.create({
      nzTitle: albumDetailsTemplateTitle,
      nzContent: albumDetailsTemplateContent,
      nzFooter: albumDetailsTemplateFooter,
      nzMaskClosable: true,
      nzClosable: true,
    });
  }

  destroyAlbumDetailsTemplateModal(nzModalRef: NzModalRef): void {
    this.isTemplateModalButtonLoading = true;
    setTimeout(() => {
      this.isTemplateModalButtonLoading = false;
      nzModalRef.destroy();
    }, 100);
  }

  showAlbumEditModal(albumTableDatum: IAlbumTableData): void {
    albumTableDatum.isEditingModalVisible = true;

    this.albumsEditForm.patchValue({
      name: albumTableDatum.album.name,
      yearOfRelease: albumTableDatum.album.yearOfRelease,
      numberOfTracks: albumTableDatum.album.numberOfTracks,
      description: albumTableDatum.album.description,
      albumType: albumTableDatum.album.albumType.name,
      genre: albumTableDatum.album.genre.name,
      performer: albumTableDatum.album.performer.name,
    });

    this.isAlbumEditCoverImageUploadButtonVisible = true;
    this.albumEditCoverImagesFileList = [];

    if (
      albumTableDatum.album.imageFileName !== null &&
      albumTableDatum.album.imageUrl !== null
    ) {
      this.albumEditCoverImagesFileList[0] = {
        uid: '-1',
        name: albumTableDatum.album.imageFileName,
        status: 'done',
        url: albumTableDatum.album.imageUrl,
        thumbUrl: albumTableDatum.album.imageUrl
      };

      this.isAlbumEditCoverImageUploadButtonVisible = false;
    }

    const albumTracksNames = albumTableDatum.album.tracks.map(
      (track) => track.name
    );

    this.tracksEditActionFormGroup.reset();
    this.listOfTrackEditControls = [];

    albumTracksNames.forEach((trackName, i) => {
      const control = {
        id: i,
        controlInstance: `editTrack${i}`,
      };

      this.listOfTrackEditControls.push(control);

      this.tracksEditActionFormGroup.addControl(
        control.controlInstance,
        new UntypedFormControl(
          trackName,
          Validators.compose([
            Validators.required,
            Validators.minLength(2),
            Validators.maxLength(40),
          ])
        )
      );
    });

    this.listOfTrackEditControls.forEach((control, i) => {
      const controlToFill =
          this.tracksEditActionFormGroup.controls[control.controlInstance];
        controlToFill.setValue(albumTracksNames[i]);
    });
  }

  handleOkAlbumEditModal(albumTableDatum: IAlbumTableData): void {
    albumTableDatum.isEditingModalVisible = false;
    this.onAlbumsEditFormSubmit(
      albumTableDatum.album.id,
      albumTableDatum.album.tracks
    );
  }

  handleCancelAlbumEditModal(albumTableDatum: IAlbumTableData): void {
    albumTableDatum.isEditingModalVisible = false;
  }

  onAlbumsCreationFormSubmit(): void {
    const name: string = this.albumsCreationForm.value.name;

    const isAlbumExisting = this.albumsDisplayData.some(
      (data) => data.album.name === name
    );

    if (isAlbumExisting) {
      this.nzNotificationService.error(
        `Error`,
        `The album ${name} already exists!`
      );
      return;
    }

    let hasInvalidTracksControls = false;

    Object.values(this.tracksActionFormGroup.controls).forEach(
      (control: any) => {
        if (control.invalid) {
          control.markAsDirty();
          control.updateValueAndValidity({ onlySelf: true });
          hasInvalidTracksControls = true;
        }
      }
    );

    if (hasInvalidTracksControls) {
      this.nzNotificationService.error(
        'Error',
        'Please, validate the tracks before continuing'
      );
      return;
    }

    const albumType = this.typesForAlbums.find(
      (albumType) => albumType.name === this.albumsCreationForm.value.albumType
    )!;
    const genre = this.genresForAlbums.find(
      (genre) => genre.name === this.albumsCreationForm.value.genre
    )!;
    const performer = this.performersForAlbums.find(
      (performer) => performer.name === this.albumsCreationForm.value.performer
    )!;

    if (
      this.albumsCreationForm.value.numberOfTracks !==
      this.listOfTrackControls.length
    ) {
      this.nzNotificationService.error(
        `Error`,
        `The number of tracks in the list must match the defined one for the album`
      );
      return;
    }

    const uploadedAlbumCoverImage = this.albumCoverImagesFileList[0];

    const albumToCreate: IAlbumCreateDTO = {
      name: this.albumsCreationForm.value.name,
      yearOfRelease: this.albumsCreationForm.value.yearOfRelease,
      numberOfTracks: this.albumsCreationForm.value.numberOfTracks,
      description: this.albumsCreationForm.value.description,
      imageFileName: uploadedAlbumCoverImage.response.blobDTO.name,
      imageUrl: uploadedAlbumCoverImage.response.blobDTO.uri,
      albumTypeId: albumType.id,
      genreId: genre.id,
      performerId: performer.id,
    };

    if (this.albumsCreationForm.valid) {
      this.albumsService
        .createNewAlbum(albumToCreate)
        .pipe(take(1))
        .subscribe({
          next: (response) => {
            let newAlbum = response;
            this.nzNotificationService.success(
              operationSuccessMessage,
              `The album ${newAlbum.name} is created successfully!`,
              {
                nzPauseOnHover: true,
              }
            );

            Object.values(this.tracksActionFormGroup.controls).forEach(
              (control: any) => {
                const trackToCreate = {
                  name: control.value,
                  albumId: newAlbum.id,
                } as ITrackCreateDTO;

                this.tracksService
                  .createNewTrack(trackToCreate)
                  .subscribe((response) => {
                    let newTrack = response;
                    this.nzNotificationService.success(
                      operationSuccessMessage,
                      `The track ${newTrack.name} is created successfully`,
                      {
                        nzPauseOnHover: true,
                      }
                    );
                  });
              }
            );

            this.isAlbumCreationModalVisible = false;
            this.retrieveAlbumsData();
          },
          error: (error) => {
            console.log(error);
          },
        });
    } else {
      Object.values(this.albumsCreationForm.controls).forEach((control) => {
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
    this.fileStorageService.getAlbumsImages().subscribe((data: any) => {
      if (
        data.map((image: any) => image.name).includes(nzUploadXHRArgs.file.name)
      ) {
        this.nzMessageService.error(
          `Album Cover Image with the same file name` +
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
            this.isAlbumCreationCoverImageUploadButtonVisible = false;
            this.isAlbumEditCoverImageUploadButtonVisible = false;
          }
        },
        (error) => {
          nzUploadXHRArgs.onError!(error, nzUploadXHRArgs.file);
        }
      );
    });
  };

  handleAlbumImagePreview = async (
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

  downloadAlbumCoverImage(fileToDownload: NzUploadFile): void | undefined {
    let a = document.createElement('a');
    a.href = fileToDownload.thumbUrl!;
    a.download = fileToDownload.name!;
    a.click();
  }

  handleAlbumCoverImageChange(info: any) {
    if (info.type === 'removed') {
      this.isAlbumCreationCoverImageUploadButtonVisible = true;
    } else {
      let fileList = [...info.fileList];

      fileList = fileList.map((file) => {
        if (file.response) {
          file.url = file.response.url;
        }
        return file;
      });

      this.albumCoverImagesFileList = fileList;
    }
  }

  handleAlbumEditCoverImageChange(info: any) {
    if (info.type === 'removed') {
      this.isAlbumEditCoverImageUploadButtonVisible = true;
    } else {
      let fileList = [...info.fileList];

      fileList = fileList.map((file) => {
        if (file.response) {
          file.url = file.response.url;
        }
        return file;
      });

      this.albumEditCoverImagesFileList = fileList;
    }
  }

  onAlbumsEditFormSubmit(
    albumId: string,
    tracks: ITrack[]
  ): Observable<boolean> {
    let isAlbumsEditFormSubmitSuccessful = true;

    const albumType = this.typesForAlbums.find(
      (albumType) => albumType.name === this.albumsEditForm.value.albumType
    )!;
    const genre = this.genresForAlbums.find(
      (genre) => genre.name === this.albumsEditForm.value.genre
    )!;
    const performer = this.performersForAlbums.find(
      (performer) => performer.name === this.albumsEditForm.value.performer
    )!;

    const albumToEdit = {
      id: albumId,
      name: this.albumsEditForm.value.name,
      yearOfRelease: this.albumsEditForm.value.yearOfRelease,
      numberOfTracks: this.albumsEditForm.value.numberOfTracks,
      description: this.albumsEditForm.value.description,
      albumTypeId: albumType.id,
      genreId: genre.id,
      performerId: performer.id,
    } as IAlbumUpdateDTO;

    const uploadedAlbumCoverImage = this.albumEditCoverImagesFileList[0];

    if (uploadedAlbumCoverImage !== undefined) {
      if (uploadedAlbumCoverImage.name && uploadedAlbumCoverImage.url) {
        albumToEdit.imageFileName = uploadedAlbumCoverImage.name;
        albumToEdit.imageUrl = uploadedAlbumCoverImage.url;
      } else {
        albumToEdit.imageFileName = uploadedAlbumCoverImage.response.blobDTO.name;
        albumToEdit.imageUrl = uploadedAlbumCoverImage.response.blobDTO.uri;
      }
    }

    const originalTracks = [...tracks];
    const originalTracksNames = originalTracks.map(
      (orginalTrack) => orginalTrack.name
    );

    const tracksToCreateOnAlbumEdit: ITrackCreateDTO[] = [];
    const existingTracksIds: string[] = [];
    const tracksToRemoveOnAlbumEdit: ITrack[] = [];

    Object.values(this.tracksEditActionFormGroup.controls).forEach(
      (control: any) => {
        if (!control.value) {
          this.nzNotificationService.error(
            `Error`,
            `Please enter valid data for tracks`,
            {
              nzPauseOnHover: true,
            }
          );
          isAlbumsEditFormSubmitSuccessful = false;
          return;
        }

        if (!originalTracksNames.includes(control.value)) {
          tracksToCreateOnAlbumEdit.push({
            name: control.value,
            albumId: albumToEdit.id,
          } as ITrackCreateDTO);
        } else {
          const foundExistingTrackId = originalTracks.find(
            (track) => track.name === control.value
          )!.id;
          existingTracksIds.push(foundExistingTrackId);
        }
      }
    );

    for (const originalTrack of originalTracks) {
      if (!existingTracksIds.includes(originalTrack.id)) {
        tracksToRemoveOnAlbumEdit.push(originalTrack);
      }
    }

    if (this.albumsEditForm.valid) {
      this.albumsService
        .updateAlbum(albumToEdit)
        .pipe(take(1))
        .subscribe((response) => {
          let editedAlbum = response;
          this.nzNotificationService.success(
            operationSuccessMessage,
            `The album ${editedAlbum.name} is edited successfully`,
            {
              nzPauseOnHover: true,
            }
          );

          if (tracksToCreateOnAlbumEdit.length !== 0) {
            for (const trackToCreate of tracksToCreateOnAlbumEdit) {
              this.tracksService
                .createNewTrack(trackToCreate)
                .subscribe((response) => {
                  let newTrack = response;
                  this.nzNotificationService.success(
                    operationSuccessMessage,
                    `The track ${newTrack.name} is created successfully!`,
                    {
                      nzPauseOnHover: true,
                    }
                  );
                });
            }
          }

          if (tracksToRemoveOnAlbumEdit.length !== 0) {
            for (const trackToRemove of tracksToRemoveOnAlbumEdit) {
              this.tracksService.deleteTrack(trackToRemove.id).subscribe(() => {
                this.nzNotificationService.success(
                  operationSuccessMessage,
                  `The track ${trackToRemove.name} has been removed!`,
                  {
                    nzPauseOnHover: true,
                  }
                );
              });
            }
          }

          this.retrieveAlbumsData();
        });
    } else {
      isAlbumsEditFormSubmitSuccessful = false;
      Object.values(this.albumsEditForm.controls).forEach((control) => {
        if (control.invalid) {
          control.markAsDirty();
          control.updateValueAndValidity({ onlySelf: true });
        }
      });
    }

    return of(isAlbumsEditFormSubmitSuccessful);
  }

  onLoadAlbumsDataClick(): void {
    this.retrieveAlbumsData();
  }

  showAlbumRemovalModal(albumToRemove: IAlbum): void {
    this.nzModalService.confirm({
      nzTitle: recordRemovalConfirmationModalTitle(albumToRemove.name),
      nzOkText: recordRemovalConfirmationModalOkText,
      nzOkType: recordRemovalConfirmationModalOkType,
      nzOkDanger: recordRemovalConfirmationModalOkDanger,
      nzOnOk: () => this.handleOkAlbumRemovalModal(albumToRemove),
      nzCancelText: recordRemovalConfirmationModalCancelText,
      nzOnCancel: () => this.handleCancelAlbumRemovalModal(),
    });
  }

  handleOkAlbumRemovalModal(albumToRemove: IAlbum): void {
    this.albumsService.deleteAlbum(albumToRemove.id).subscribe(() => {
      this.nzNotificationService.success(
        operationSuccessMessage,
        `The album ${albumToRemove.name} has been removed!`,
        {
          nzPauseOnHover: true,
        }
      );
      this.retrieveAlbumsData();
    });
  }

  handleCancelAlbumRemovalModal(): void {
    this.nzNotificationService.info(
      removalOperationCancelMessage,
      `Album removal cancelled`
    );
  }

  onAlbumTypeAutocompleteChange(value: string): void {
    this.filteredAlbumTypesNamesForAutocomplete =
      this.albumTypesNamesForAutocomplete.filter(
        (albumTypeName) =>
          albumTypeName.toLowerCase().indexOf(value.toLowerCase()) !== -1
      );
  }

  onGenreAutocompleteChange(value: string): void {
    this.filteredGenresNamesForAutocomplete =
      this.genresNamesForAutocomplete.filter(
        (genreName) =>
          genreName.toLowerCase().indexOf(value.toLowerCase()) !== -1
      );
  }

  onPerformerAutocompleteChange(value: string): void {
    this.filteredPerformersNamesForAutocomplete =
      this.performersNamesForAutocomplete.filter(
        (performerName) =>
          performerName.toLowerCase().indexOf(value.toLowerCase()) !== -1
      );
  }

  onExportAlbumsDataClick() {
    this.createFileExportContent(
      this.fileTypes[this.selectedExportFormatValue as keyof {}]
    );
  }

  createFileExportContent(fileType: string): void {
    let separator;
    let fileContent: string;

    let mappedAlbumsDataForExport = this.albumsToManage
      .filter((atm) => atm.checked)
      .map((atm) => atm.album)
      .map((a) => {
        const albumsExportDatum = {
          Name: a.name,
          NumberOfTracks: a.numberOfTracks,
          YearOfRelease: a.yearOfRelease,
          AlbumType: a.albumType.name,
          Genre: a.genre.name,
          Performer: a.performer.name,
        };
        return albumsExportDatum;
      });

    if (mappedAlbumsDataForExport.length === 0) {
      this.nzNotificationService.error(
        'Error',
        'Please select album entries for export'
      );
      return;
    }

    const mappedAlbumHeadersKeys = this.albumExportHeaders.map((header) =>
      header.replaceAll(' ', '')
    );

    mappedAlbumsDataForExport = this.excludeMappedAlbumsPropertiesForFileExport(
      mappedAlbumsDataForExport,
      mappedAlbumHeadersKeys
    );

    switch (fileType) {
      case this.fileTypes.csv:
        separator = ',';
        fileContent =
          mappedAlbumHeadersKeys.join(separator) +
          '\n' +
          this.mapAlbumsForExport(
            mappedAlbumsDataForExport,
            mappedAlbumHeadersKeys,
            separator
          );
        this.exportFile(fileContent, fileType);
        break;
      case this.fileTypes.txt:
        separator = '\t';
        fileContent = this.mapAlbumsForExport(
          mappedAlbumsDataForExport,
          mappedAlbumHeadersKeys,
          separator
        );
        this.exportFile(fileContent, fileType);
        break;
      case this.fileTypes.tsv:
        separator = '\t';
        fileContent =
          this.albumExportHeaders.join(separator) +
          '\n' +
          this.mapAlbumsForExport(
            mappedAlbumsDataForExport,
            this.albumExportHeaders,
            separator
          );
        this.exportFile(fileContent, fileType);
        break;
      case this.fileTypes.xlsx:
        const workSheet: XLSX.WorkSheet = XLSX.utils.json_to_sheet(
          mappedAlbumsDataForExport
        );
        const workBook: XLSX.WorkBook = XLSX.utils.book_new();
        XLSX.utils.book_append_sheet(workBook, workSheet, 'Sheet1');
        XLSX.writeFile(
          workBook,
          `${this.generateExportFileName(this.tableExportOptions.xlsx)}`
        );
        break;
      case this.fileTypes.pdf:
        const pdfDocument = new jsPDF('l', 'mm', 'a3');
        const pdfTableHeaders: string[][] = [this.albumExportHeaders];
        const pdfTableRows: any[] = [];
        mappedAlbumsDataForExport.forEach((item) => {
          const mappedItemToArray: (string | number)[] = [
            ...Object.values(item),
          ];
          pdfTableRows.push(mappedItemToArray);
        });
        autoTable(pdfDocument, {
          head: pdfTableHeaders,
          body: pdfTableRows,
        });
        pdfDocument.save(
          `${this.generateExportFileName(this.tableExportOptions.pdf)}`
        );
        break;
      case this.fileTypes.json:
        this.exportFile(JSON.stringify(mappedAlbumsDataForExport), fileType);
        break;
      case this.fileTypes.xml:
        let xmlContent = '';
        xmlContent += `<?xml version='1.0'?>`;
        xmlContent += `<AlbumsList>`;
        mappedAlbumsDataForExport.forEach((mapedAlbumsDatumForExport) => {
          xmlContent += `<Album>`;
          for (const [key, value] of Object.entries(
            mapedAlbumsDatumForExport
          )) {
            xmlContent += `<${key}>${value}</${key}>`;
          }
          xmlContent += `</Album>`;
        });
        xmlContent += `</AlbumsList>`;
        this.exportFile(xmlContent, fileType);
        break;
      case this.fileTypes.docx:
        const wordTableRows: TableRow[] = [];
        const wordTableCells: TableCell[] = [];
        this.albumExportHeaders.forEach((item) => {
          const headerCellTextRun = new TextRun({
            text: item,
            size: 24,
            bold: true,
          });
          wordTableCells.push(
            new TableCell({
              children: [
                new Paragraph({
                  children: [headerCellTextRun],
                }),
              ],
            })
          );
        });
        const wordTableHeaderRow: TableRow = new TableRow({
          children: wordTableCells,
        });
        wordTableRows.push(wordTableHeaderRow);
        mappedAlbumsDataForExport.forEach((item) => {
          const articlesRangedListWordTableCells: TableCell[] = [];
          for (const cellValue of Object.values(item)) {
            const cellValueTextRun = new TextRun({
              text: cellValue.toString(),
              size: 24,
            });
            articlesRangedListWordTableCells.push(
              new TableCell({
                children: [
                  new Paragraph({
                    children: [cellValueTextRun],
                  }),
                ],
              })
            );
          }
          const wordTableRow: TableRow = new TableRow({
            children: articlesRangedListWordTableCells,
          });
          wordTableRows.push(wordTableRow);
        });
        const wordTable = new Table({
          rows: wordTableRows,
        });
        const wordDocument = new Document({
          sections: [
            {
              properties: {
                page: {
                  size: {
                    orientation: PageOrientation.LANDSCAPE,
                  },
                },
              },
              children: [
                new Paragraph({
                  text: 'Rockstodons Albums',
                  heading: HeadingLevel.HEADING_1,
                  alignment: AlignmentType.CENTER,
                  spacing: {
                    after: 200,
                  },
                }),
                wordTable,
              ],
            },
          ],
          styles: {
            paragraphStyles: [
              {
                id: 'style',
                run: {
                  size: 26,
                },
              },
            ],
          },
        });
        Packer.toBlob(wordDocument).then((blob) => {
          this.fileSaverService.save(
            blob,
            `${this.generateExportFileName(this.tableExportOptions.docx)}`
          );
        });
        break;
      case this.fileTypes.pptx:
        const presentation = new pptxgen();
        const presentationSlide = presentation.addSlide();
        const textboxText = 'Rockstodons Albums';
        presentationSlide.addText([
          { text: textboxText, options: { x: 1, y: 1, align: 'center' } },
        ]);
        const rows = [];
        const headerCellArray: any[] = [];
        this.albumExportHeaders.forEach((item) => {
          headerCellArray.push({
            text: item.toString(),
            options: { bold: true },
          });
        });
        rows.push(headerCellArray);
        mappedAlbumsDataForExport.forEach((item) => {
          const cellArray = [];
          for (const value of Object.values(item)) {
            cellArray.push({
              text: value.toString(),
              options: { color: '363636' },
            });
          }
          rows.push(cellArray);
        });
        presentationSlide.addTable(rows, {
          x: 0.5,
          y: 1.0,
          w: 9.0,
          color: '363636',
          autoPage: true,
          border: {
            type: 'solid',
          },
        });
        presentation.writeFile({
          fileName: `${this.generateExportFileName(
            this.tableExportOptions.pptx
          )}`,
        });
        break;
      case this.fileTypes.apng:
      case this.fileTypes.avif:
      case this.fileTypes.jpg:
      case this.fileTypes.jpeg:
      case this.fileTypes.png:
      case this.fileTypes.gif:
        this.convertHTMLToCanvas(
          this.albumsManagementDynamicTable!.nativeElement,
          fileType
        );
        break;
    }
  }

  excludeMappedAlbumsPropertiesForFileExport(
    albumsForExport: any,
    keys: string[]
  ): any {
    return albumsForExport.map((item: any) => {
      for (const property of Object.keys(item)) {
        if (!keys.includes(property)) {
          delete item[property as keyof IAlbum];
        }
      }
      return item;
    });
  }

  mapAlbumsForExport(
    arrayForExport: any,
    headers: any,
    separator: string
  ): string {
    return arrayForExport
      .map((rowData: IAlbum) => {
        return headers
          .map((headerKey: string) => {
            return rowData[headerKey.replaceAll(' ', '') as keyof IAlbum] ===
              null ||
              rowData[headerKey.replaceAll(' ', '') as keyof IAlbum] ===
                undefined
              ? ''
              : rowData[headerKey.replaceAll(' ', '') as keyof IAlbum];
          })
          .join(separator);
      })
      .join('\n');
  }

  exportFile(data: any, fileType: string) {
    const blob = new Blob([data], { type: fileType });
    const fileExtension =
      `.` +
      Object.keys(this.fileTypes).find(
        (k) => this.fileTypes[k as keyof object] === fileType
      );
    this.fileSaverService.save(
      blob,
      `${this.generateExportFileName(fileExtension)}`
    );
  }

  generateExportFileName(fileExtension: string): string {
    const currentTimeForFileExport: Date = new Date();
    const generatedFileName =
      `Rockstodons_Albums_` +
      `${
        currentTimeForFileExport.getDate() < 10 ? '0' : ''
      }${currentTimeForFileExport.getDate()}.` +
      `${currentTimeForFileExport.getMonth() + 1 < 10 ? '0' : ''}${
        currentTimeForFileExport.getMonth() + 1
      }.` +
      `${currentTimeForFileExport.getFullYear()}_` +
      `${
        currentTimeForFileExport.getHours() < 10 ? '0' : ''
      }${currentTimeForFileExport.getHours()}_` +
      `${
        currentTimeForFileExport.getMinutes() < 10 ? '0' : ''
      }${currentTimeForFileExport.getMinutes()}_` +
      `${
        currentTimeForFileExport.getSeconds() < 10 ? '0' : ''
      }${currentTimeForFileExport.getSeconds()}` +
      `${fileExtension ? `${fileExtension}` : ''}`;
    return generatedFileName;
  }

  convertHTMLToCanvas(nativeElement: any, fileType: string): void {
    html2canvas(nativeElement).then((canvas) => {
      this.canvas!.nativeElement.src = canvas.toDataURL();
      this.downloadLink!.nativeElement.href = canvas.toDataURL(fileType);
      const downloadImageName = `articles-general-registers.${
        fileType.split('/')[1]
      }`;
      this.downloadLink!.nativeElement.download = downloadImageName;
      this.downloadLink!.nativeElement.click();
      this.canvas!.nativeElement.src = '';
    });
  }

  addTrackField(mouseEvent?: MouseEvent): void {
    if (mouseEvent) {
      mouseEvent.preventDefault();
    }
    const id =
      this.listOfTrackControls.length > 0
        ? this.listOfTrackControls[this.listOfTrackControls.length - 1].id + 1
        : 0;

    const control = {
      id,
      controlInstance: `track${id}`,
    };

    const index = this.listOfTrackControls.push(control);

    this.tracksActionFormGroup.addControl(
      this.listOfTrackControls[index - 1].controlInstance,
      new UntypedFormControl(
        null,
        Validators.compose([
          Validators.required,
          Validators.minLength(2),
          Validators.maxLength(40),
        ])
      )
    );
  }

  removeTrackField(
    trackEntry: { id: number; controlInstance: string },
    mouseEvent: MouseEvent
  ): void {
    mouseEvent.preventDefault();

    if (this.listOfTrackControls.length > 1) {
      const index = this.listOfTrackControls.indexOf(trackEntry);
      this.listOfTrackControls.splice(index, 1);
      this.tracksActionFormGroup.removeControl(trackEntry.controlInstance);
    }
  }

  addEditTrackField(mouseEvent?: MouseEvent): void {
    if (mouseEvent) {
      mouseEvent.preventDefault();
    }

    const id =
      this.listOfTrackEditControls.length > 0
        ? this.listOfTrackEditControls[this.listOfTrackEditControls.length - 1]
            .id + 1
        : 0;

    const control = {
      id,
      controlInstance: `editTrack${id}`,
    };

    const index = this.listOfTrackEditControls.push(control);

    this.tracksEditActionFormGroup.addControl(
      this.listOfTrackEditControls[index - 1].controlInstance,
      new UntypedFormControl(
        null,
        Validators.compose([
          Validators.required,
          Validators.minLength(2),
          Validators.maxLength(40),
        ])
      )
    );
  }

  removeEditTrackField(
    editTrackEntry: { id: number; controlInstance: string },
    mouseEvent: MouseEvent
  ): void {
    mouseEvent.preventDefault();

    if (this.listOfTrackEditControls.length > 1) {
      const index = this.listOfTrackEditControls.indexOf(editTrackEntry);
      this.listOfTrackEditControls.splice(index, 1);
      this.tracksEditActionFormGroup.removeControl(
        editTrackEntry.controlInstance
      );
    } else if (this.listOfTrackEditControls.length === 1) {
      const firstEditTrackControl =
        this.tracksEditActionFormGroup.controls[
          this.listOfTrackEditControls[0].controlInstance
        ];

      if (firstEditTrackControl.value) {
        firstEditTrackControl.setValue(null);
      }
    }
  }

  ngOnInit(): void {
    this.albumsManagementTableSettingsForm =
      new FormGroup<IAlbumsManagementTableSettingsForm>({
        isBordered: new FormControl(false, { nonNullable: true }),
        hasPagination: new FormControl(true, { nonNullable: true }),
        hasSizeChanger: new FormControl(false, { nonNullable: true }),
        hasTitle: new FormControl(true, { nonNullable: true }),
        hasHeader: new FormControl(true, { nonNullable: true }),
        hasFooter: new FormControl(true, { nonNullable: true }),
        isExpandable: new FormControl(true, { nonNullable: true }),
        withCheckbox: new FormControl(true, { nonNullable: true }),
        hasFixedHeader: new FormControl(false, { nonNullable: true }),
        withEllipsis: new FormControl(false, { nonNullable: true }),
        isSimple: new FormControl(false, { nonNullable: true }),
        tableSize: new FormControl('small', { nonNullable: true }),
        paginationType: new FormControl('default', { nonNullable: true }),
        tableScroll: new FormControl('unset', { nonNullable: true }),
        tableLayout: new FormControl('auto', { nonNullable: true }),
        position: new FormControl('bottom', { nonNullable: true }),
      });
    this.retrieveAlbumsData();
    this.albumsManagementTableSetting =
      this.albumsManagementTableSettingsForm?.value;
    this.albumsManagementTableSettingsForm?.valueChanges.subscribe((value) => {
      this.albumsManagementTableSetting = value;
    });
    this.tableScroll.valueChanges.subscribe((scroll) => {
      this.fixedColumn = scroll === 'fixed';
      this.scrollX = scroll === 'scroll' || scroll === 'fixed' ? '100vw' : null;
    });
    this.hasFixedHeader.valueChanges.subscribe((isFixed) => {
      this.scrollY = isFixed ? '240px' : null;
    });
    this.addTrackField();
  }

  private retrieveAlbumsData(): void {
    this.isLoading = true;
    this.albumsService.getAllAlbums().subscribe((data) => {
      this.albumsToManage = [];
      data.map((retrievedAlbum) => {
        this.albumsToManage.push({
          album: {
            id: retrievedAlbum.id,
            name: retrievedAlbum.name,
            yearOfRelease: retrievedAlbum.yearOfRelease,
            numberOfTracks: retrievedAlbum.numberOfTracks,
            description: retrievedAlbum.description,
            performer: retrievedAlbum.performer,
            genre: retrievedAlbum.genre,
            albumType: retrievedAlbum.albumType,
            createdOn: retrievedAlbum.createdOn,
            imageFileName: retrievedAlbum.imageFileName,
            imageUrl: retrievedAlbum.imageUrl,
            tracks: retrievedAlbum.tracks,
          },
          checked: false,
          expanded: false,
        } as IAlbumTableData);
      });
      this.albumsToManage = this.albumsToManage
        .sort((a, b) => a.album.yearOfRelease - b.album.yearOfRelease)
        .reverse();
      this.performersService.getAllPerformers().subscribe((data) => {
        this.performersForAlbums = data;
        this.performersNamesForAutocomplete = this.performersForAlbums.map(
          (performer) => performer.name
        );
        this.filteredPerformersNamesForAutocomplete =
          this.performersNamesForAutocomplete;
      });
      this.genresService.getAllGenres().subscribe((data) => {
        this.genresForAlbums = data;
        this.genresNamesForAutocomplete = this.genresForAlbums.map(
          (genre) => genre.name
        );
        this.filteredGenresNamesForAutocomplete =
          this.genresNamesForAutocomplete;
      });
      this.albumTypesService.getAllAlbumTypes().subscribe((data) => {
        this.typesForAlbums = data;
        this.albumTypesNamesForAutocomplete = this.typesForAlbums.map(
          (albumType) => albumType.name
        );
        this.filteredAlbumTypesNamesForAutocomplete =
          this.albumTypesNamesForAutocomplete;
      });
      this.isLoading = false;
    });
  }
}

export interface IAlbumTableData {
  album: IAlbum;
  checked: boolean;
  expanded: boolean;
  disabled?: boolean;
  isDetailsModalVisible: boolean;
  isEditingModalVisible: boolean;
}

export interface IAlbumsManagementTableSetting {
  isBordered: boolean;
  hasPagination: boolean;
  hasSizeChanger: boolean;
  hasTitle: boolean;
  hasHeader: boolean;
  hasFooter: boolean;
  isExpandable: boolean;
  withCheckbox: boolean;
  hasFixedHeader: boolean;
  withEllipsis: boolean;
  isSimple: boolean;
  tableSize: NzTableSize;
  tableScroll: string;
  tableLayout: NzTableLayout;
  position: NzTablePaginationPosition;
  paginationType: NzTablePaginationType;
}

export interface IAlbumsManagementTableSettingsForm {
  isBordered: FormControl<boolean>;
  hasPagination: FormControl<boolean>;
  hasSizeChanger: FormControl<boolean>;
  hasTitle: FormControl<boolean>;
  hasHeader: FormControl<boolean>;
  hasFooter: FormControl<boolean>;
  isExpandable: FormControl<boolean>;
  withCheckbox: FormControl<boolean>;
  hasFixedHeader: FormControl<boolean>;
  withEllipsis: FormControl<boolean>;
  isSimple: FormControl<boolean>;
  tableSize: FormControl<string>;
  tableScroll: FormControl<string>;
  tableLayout: FormControl<string>;
  position: FormControl<string>;
  paginationType: FormControl<string>;
}

export interface IAlbumActionForm {
  name: FormControl<string>;
  yearOfRelease: FormControl<number>;
  numberOfTracks: FormControl<number>;
  description: FormControl<string>;
  albumType: FormControl<string>;
  genre: FormControl<string>;
  performer: FormControl<string>;
  tracksActionFormGroup?: UntypedFormGroup;
}
