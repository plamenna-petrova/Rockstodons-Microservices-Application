import { Component, TemplateRef } from '@angular/core';
import {
  AbstractControl,
  FormArray,
  FormControl,
  FormGroup,
  FormRecord,
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
import { Observable, of, take } from 'rxjs';
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

  selectedExportFormatValue = null;

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

  albumTypesNamesForAutocomplete: string[] = [];
  genresNamesForAutocomplete: string[] = [];
  performersNamesForAutocomplete: string[] = [];

  filteredAlbumTypesNamesForAutocomplete: string[] = [];
  filteredGenresNamesForAutocomplete: string[] = [];
  filteredPerformersNamesForAutocomplete: string[] = [];

  constructor(
    private performersService: PerformersService,
    private albumsService: AlbumsService,
    private genresService: GenresService,
    private albumTypesService: AlbumTypesService,
    private tracksService: TracksService,
    private nzNotificationService: NzNotificationService,
    private nzModalService: NzModalService
  ) {
    this.buildAlbumsActionForms();
  }

  buildAlbumsActionForms(): void {
    this.albumsCreationForm = new FormGroup<IAlbumActionForm>({
      name: new FormControl('', {
        validators: Validators.compose([
          Validators.required,
          Validators.minLength(2),
          Validators.maxLength(25),
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
          Validators.maxLength(25),
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
    this.albumsCreationForm.reset();

    this.listOfTrackControls.forEach((control, i) => {
      if (i === 0) {
        const controlToErase = this.tracksActionFormGroup
          .controls[control.controlInstance];
        controlToErase.setValue(null);
      } else {
        const index = this.listOfTrackControls.indexOf(control);
        this.listOfTrackControls.splice(index, 1);
        this.tracksActionFormGroup.removeControl(
          control.controlInstance
        );
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

    const albumTracksNames = albumTableDatum.album.tracks.map(
      (track) => track.name
    );

    if (this.listOfTrackEditControls.length > 0) {
      this.listOfTrackEditControls.forEach((editControl) => {
        const index = this.listOfTrackEditControls.indexOf(editControl);
        this.listOfTrackEditControls.splice(index, 1);
        this.tracksEditActionFormGroup.removeControl(
          editControl.controlInstance
        );
      });
    }

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

    if (this.albumsCreationForm.value.numberOfTracks !== this.listOfTrackControls.length) {
      this.nzNotificationService.error(
        `Error`,
        `The number of tracks in the list must match the defined one for the album`
      );
      return;
    }

    const albumToCreate: IAlbumCreateDTO = {
      name: this.albumsCreationForm.value.name,
      yearOfRelease: this.albumsCreationForm.value.yearOfRelease,
      numberOfTracks: this.albumsCreationForm.value.numberOfTracks,
      description: this.albumsCreationForm.value.description,
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
              `Successful Operation`,
              `The album ${newAlbum.name} is created successfully!`,
              {
                nzPauseOnHover: true
              }
            );

            Object.values(this.tracksActionFormGroup.controls).forEach(
              (control: any) => {
                const trackToCreate: ITrackCreateDTO = {
                  name: control.value,
                  albumId: newAlbum.id,
                };

                this.tracksService
                  .createNewTrack(trackToCreate)
                  .subscribe((response) => {
                    let newTrack = response;
                    this.nzNotificationService.success(
                      `Successful Operation`,
                      `The track ${newTrack.name} is created successfully`,
                      {
                        nzPauseOnHover: true
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

  onAlbumsEditFormSubmit(albumId: string, tracks: ITrack[]): Observable<boolean> {
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

    const albumToEdit: IAlbumUpdateDTO = {
      id: albumId,
      name: this.albumsEditForm.value.name,
      yearOfRelease: this.albumsEditForm.value.yearOfRelease,
      numberOfTracks: this.albumsEditForm.value.numberOfTracks,
      description: this.albumsEditForm.value.description,
      albumTypeId: albumType.id,
      genreId: genre.id,
      performerId: performer.id,
    };

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
              nzPauseOnHover: true
            }
          );
          isAlbumsEditFormSubmitSuccessful = false;
          return;
        }

        if (!originalTracksNames.includes(control.value)) {
          tracksToCreateOnAlbumEdit.push({
            name: control.value,
            albumId: albumToEdit.id,
          });
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
            `Successful Operation`,
            `The album ${editedAlbum.name} is edited successfully`,
            {
              nzPauseOnHover: true
            }
          );

          if (tracksToCreateOnAlbumEdit.length !== 0) {
            for (const trackToCreate of tracksToCreateOnAlbumEdit) {
              this.tracksService
                .createNewTrack(trackToCreate)
                .subscribe((response) => {
                  let newTrack = response;
                  this.nzNotificationService.success(
                    `Successful Operation`,
                    `The track ${newTrack.name} is created successfully!`,
                    {
                      nzPauseOnHover: true
                    }
                  );
                });
            }
          }

          if (tracksToRemoveOnAlbumEdit.length !== 0) {
            for (const trackToRemove of tracksToRemoveOnAlbumEdit) {
              this.tracksService.deleteTrack(trackToRemove.id).subscribe(() => {
                this.nzNotificationService.success(
                  'Successful Operation',
                  `The track ${trackToRemove.name} has been removed!`,
                  {
                    nzPauseOnHover: true
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
      nzTitle: `Do you really wish to remove ${albumToRemove.name}?`,
      nzOkText: 'Yes',
      nzOkType: 'primary',
      nzOkDanger: true,
      nzOnOk: () => this.handleOkAlbumRemovalModal(albumToRemove),
      nzCancelText: 'No',
      nzOnCancel: () => this.handleCancelAlbumRemovalModal(),
    });
  }

  handleOkAlbumRemovalModal(albumToRemove: IAlbum): void {
    this.albumsService.deleteAlbum(albumToRemove.id).subscribe(() => {
      this.nzNotificationService.success(
        'Successful Operation',
        `The album ${albumToRemove.name} has been removed!`,
        {
          nzPauseOnHover: true
        }
      );
      this.retrieveAlbumsData();
    });
  }

  handleCancelAlbumRemovalModal(): void {
    this.nzNotificationService.info(
      `Aborted operation`,
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

  onExportAlbumsDataClick(): void {
    switch (this.selectedExportFormatValue!) {
      case 'pdf':
        break;
      case 'csv':
        break;
      case 'xlsx':
        break;
    }
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
            tracks: retrievedAlbum.tracks,
          },
          checked: false,
          expanded: false,
        } as IAlbumTableData);
      });
      this.albumsToManage = this.albumsToManage
        .sort((a, b) => a.album.yearOfRelease - b.album.yearOfRelease)
        .reverse();
      console.log('albums to manage');
      console.log(this.albumsToManage);
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
