import { Component } from '@angular/core';
import { AbstractControl, FormControl, FormGroup } from '@angular/forms';
import { NzModalService } from 'ng-zorro-antd/modal';
import { NzNotificationService } from 'ng-zorro-antd/notification';
import { NzTableLayout, NzTablePaginationPosition, NzTablePaginationType, NzTableSize } from 'ng-zorro-antd/table';
import { IAlbum } from 'src/app/core/interfaces/album';
import { IAlbumType } from 'src/app/core/interfaces/album-type';
import { IGenre } from 'src/app/core/interfaces/genre';
import { IPerformer } from 'src/app/core/interfaces/performer';
import { AlbumTypesService } from 'src/app/core/services/album-types.service';
import { AlbumsService } from 'src/app/core/services/albums.service';
import { GenresService } from 'src/app/core/services/genres.service';
import { PerformersService } from 'src/app/core/services/performers.service';

@Component({
  selector: 'app-albums-management',
  templateUrl: './albums-management.component.html',
  styleUrls: ['./albums-management.component.scss']
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

  isAlbumRemovalModalVisible = false;

  isVisible = false;
  isConfirmLoading = false;

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
    { name: 'No Result', formControlName: 'noResult' },
    { name: 'Ellipsis', formControlName: 'withEllipsis' },
    { name: 'Simple Pagination', formControlName: 'isSimple' }
  ];

  listOfRadioButtons = [
    {
      name: 'Size',
      formControlName: 'tableSize',
      listOfOptions: [
        { value: 'default', label: 'Default' },
        { value: 'middle', label: 'Middle' },
        { value: 'small', label: 'Small' }
      ]
    },
    {
      name: 'Table Scroll',
      formControlName: 'tableScroll',
      listOfOptions: [
        { value: 'unset', label: 'Unset' },
        { value: 'scroll', label: 'Scroll' },
        { value: 'fixed', label: 'Fixed' }
      ]
    },
    {
      name: 'Table Layout',
      formControlName: 'tableLayout',
      listOfOptions: [
        { value: 'auto', label: 'Auto' },
        { value: 'fixed', label: 'Fixed' }
      ]
    },
    {
      name: 'Pagination Position',
      formControlName: 'position',
      listOfOptions: [
        { value: 'top', label: 'Top' },
        { value: 'bottom', label: 'Bottom' },
        { value: 'both', label: 'Both' }
      ]
    },
    {
      name: 'Pagination Type',
      formControlName: 'paginationType',
      listOfOptions: [
        { value: 'default', label: 'Default' },
        { value: 'small', label: 'Small' }
      ]
    }
  ];

  constructor(
    private performersService: PerformersService,
    private albumsService: AlbumsService,
    private genresService: GenresService,
    private albumTypesService: AlbumTypesService,
    private nzNotificationService: NzNotificationService,
    private nzModalService: NzModalService
  ) {

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

  get noResult(): AbstractControl {
    return this.albumsManagementTableSettingsForm.get('noResult')!;
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

  onCurrentPageDataChange($event: readonly IAlbumTableData[]): void {
    this.albumsDisplayData = $event;
    this.refreshAlbumsManagementSettingsStatus();
  }

  checkAll(value: boolean): void {
    this.albumsDisplayData.forEach(data => {
      if (!data.disabled) {
        data.checked = value;
      }
    });
    this.refreshAlbumsManagementSettingsStatus();
  }

  refreshAlbumsManagementSettingsStatus(): void {
    const validAlbumData = this.albumsDisplayData.filter(value => !value.disabled);
    const allChecked = validAlbumData.length > 0 && validAlbumData.every(value => value.checked);
    const allUnchecked = validAlbumData.every(value => !value.checked);
    this.allSettingsChecked = allChecked;
    this.indeterminate = !allChecked && !allUnchecked;
  }

  ngOnInit(): void {
    this.albumsManagementTableSettingsForm = new FormGroup<IAlbumsManagementTableSettingsForm>({
      isBordered: new FormControl(false, { nonNullable: true }),
      hasPagination: new FormControl(true, { nonNullable: true }),
      hasSizeChanger: new FormControl(false, { nonNullable: true }),
      hasTitle: new FormControl(true, { nonNullable: true }),
      hasHeader: new FormControl(true, { nonNullable: true }),
      hasFooter: new FormControl(true, { nonNullable: true }),
      isExpandable: new FormControl(true, { nonNullable: true }),
      withCheckbox: new FormControl(true, { nonNullable: true }),
      hasFixedHeader: new FormControl(false, { nonNullable: true }),
      noResult: new FormControl(false, { nonNullable: true }),
      withEllipsis: new FormControl(false, { nonNullable: true }),
      isSimple: new FormControl(false, { nonNullable: true }),
      tableSize: new FormControl('small', { nonNullable: true }),
      paginationType: new FormControl('default', { nonNullable: true }),
      tableScroll: new FormControl('unset', { nonNullable: true }),
      tableLayout: new FormControl('auto', { nonNullable: true }),
      position: new FormControl('bottom', { nonNullable: true })
    });
    this.retrieveAlbumsData();
    this.albumsManagementTableSetting = this.albumsManagementTableSettingsForm?.value;
    this.albumsManagementTableSettingsForm?.valueChanges.subscribe(value => {
      this.albumsManagementTableSetting = value;
    });
    this.tableScroll.valueChanges.subscribe(scroll => {
      this.fixedColumn = scroll === 'fixed';
      this.scrollX = scroll === 'scroll' || scroll === 'fixed' ? '100vw' : null;
    });
    this.hasFixedHeader.valueChanges.subscribe(isFixed => {
      this.scrollY = isFixed ? '240px' : null;
    });
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
      nzOnCancel: () => this.handleCancelAlbumRemovalModal()
    })
  }

  handleOkAlbumRemovalModal(albumToRemove: IAlbum) {
    this.albumsService.deleteAlbum(albumToRemove.id).subscribe(() => {
      this.nzNotificationService.success(
        'Successful Operation',
        `The album ${albumToRemove.name} has been removed!`
      );
      this.retrieveAlbumsData();
    });
  }

  handleCancelAlbumRemovalModal() {
    this.nzNotificationService.info(`Aborted operation`, `Album removal cancelled`);
  }

  private retrieveAlbumsData(): void {
    this.isLoading = true;
    this.performersService.getAllPerformers().subscribe(data => {
      this.performersForAlbums = data;
    });
    this.genresService.getAllGenres().subscribe(data => {
      this.genresForAlbums = data;
    });
    this.albumTypesService.getAllAlbumTypes().subscribe(data => {
      this.typesForAlbums = data;
    });
    this.albumsService.getAlbumsWithFullDetails().subscribe(data => {
      this.albumsToManage = [];
      data.filter(album => !album.isDeleted).map(retrievedAlbum => {
        this.albumsToManage.push({
          album: {
            id: retrievedAlbum.id,
            name: retrievedAlbum.name,
            yearOfRelease: retrievedAlbum.yearOfRelease,
            description: retrievedAlbum.description,
            performer: this.performersForAlbums
              .find(albumPerformer => albumPerformer.id === retrievedAlbum.performerId),
            genre: this.genresForAlbums
              .find(albumGenre => albumGenre.id === retrievedAlbum.genreId),
            albumType: this.typesForAlbums
              .find(type => type.id === retrievedAlbum.albumTypeId)
          },
          checked: false,
          expanded: false
        } as IAlbumTableData)
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
  noResult: boolean;
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
  noResult: FormControl<boolean>;
  withEllipsis: FormControl<boolean>;
  isSimple: FormControl<boolean>;
  tableSize: FormControl<string>;
  tableScroll: FormControl<string>;
  tableLayout: FormControl<string>;
  position: FormControl<string>;
  paginationType: FormControl<string>;
}
