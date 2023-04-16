import { Component } from '@angular/core';
import {
  AbstractControl,
  FormControl,
  FormGroup,
  Validators,
} from '@angular/forms';
import { NzModalService } from 'ng-zorro-antd/modal';
import { NzNotificationService } from 'ng-zorro-antd/notification';
import { IAlbumType } from 'src/app/core/interfaces/album-types/album-type';
import { AlbumTypesService } from 'src/app/core/services/album-types.service';
import { IAlbumTypeCreateDTO } from 'src/app/core/interfaces/album-types/album-type-create-dto';
import { take } from 'rxjs';
import { IAlbumTypeUpdateDTO } from 'src/app/core/interfaces/album-types/album-type-update-dto';

@Component({
  selector: 'app-album-types-management',
  templateUrl: './album-types-management.component.html',
  styleUrls: ['./album-types-management.component.scss'],
})
export class AlbumTypesManagementComponent {
  searchValue = '';
  isLoading = false;
  isAlbumTypesSearchTriggerVisible = false;
  albumTypesData!: IAlbumTypeTableData[];
  albumTypesDisplayData!: IAlbumTypeTableData[];
  isAlbumTypeCreationModalVisible = false;

  albumTypesCreationForm!: FormGroup;
  albumTypesEditForm!: FormGroup;

  constructor(
    private albumTypesService: AlbumTypesService,
    private nzNotificationService: NzNotificationService,
    private nzModalService: NzModalService
  ) {
    this.buildAlbumTypesActionForms();
  }

  buildAlbumTypesActionForms(): void {
    const albumTypesActionFormGroup = new FormGroup<IAlbumTypeActionForm>({
      albumTypeName: new FormControl('', {
        validators: Validators.compose([
          Validators.required,
          Validators.minLength(3),
          Validators.maxLength(20),
        ]),
        nonNullable: true,
      }),
    });
    this.albumTypesCreationForm = this.albumTypesEditForm = albumTypesActionFormGroup;
  }

  get albumTypeName(): AbstractControl {
    return this.albumTypesCreationForm.get('albumTypeName')!;
  }

  onLoadAlbumTypesDataClick(): void {
    this.retrieveAlbumTypesData();
  }

  resetAlbumTypesSearch(): void {
    this.searchValue = '';
    this.searchForAlbumTypes();
  }

  searchForAlbumTypes(): void {
    this.isAlbumTypesSearchTriggerVisible = false;
    this.albumTypesDisplayData = this.albumTypesData.filter(
      (data: IAlbumTypeTableData) =>
        data.albumType.name
          .toLowerCase()
          .indexOf(this.searchValue.toLocaleLowerCase()) !== -1
    );
  }

  showAlbumTypeCreationModal(): void {
    this.isAlbumTypeCreationModalVisible = true;
  }

  handleOkAlbumTypeCreationModal(): void {
    this.isAlbumTypeCreationModalVisible = false;
    this.onAlbumTypesCreationFormSubmit();
  }

  handleCancelAlbumTypeCreationModal(): void {
    this.isAlbumTypeCreationModalVisible = false;
  }

  showAlbumTypeEditModal(albumTypeTableDatum: IAlbumTypeTableData): void {
    albumTypeTableDatum.isEditingModalVisible = true;
    this.albumTypesEditForm.controls['albumTypeName'].setValue(
      albumTypeTableDatum.albumType.name
    );
  }

  handleOkAlbumTypeEditModal(albumTypeTableDatum: IAlbumTypeTableData): void {
    albumTypeTableDatum.isEditingModalVisible = false;
    this.onAlbumTypesEditFormSubmit(albumTypeTableDatum.albumType.id);
  }

  handleCancelAlbumTypeEditModal(
    albumTypeTableDatum: IAlbumTypeTableData
  ): void {
    albumTypeTableDatum.isEditingModalVisible = false;
  }

  onAlbumTypesCreationFormSubmit(): void {
    const albumTypeName: string =
      this.albumTypesCreationForm.value.albumTypeName;

    const albumTypeToCreate: IAlbumTypeCreateDTO = {
      name: albumTypeName,
    };

    const isAlbumTypeExisting = this.albumTypesData.some(
      (data) => data.albumType.name === albumTypeName
    );

    if (isAlbumTypeExisting) {
      this.nzNotificationService.error(
        `Error`,
        `The album type ${albumTypeName} already exists!`
      );
      return;
    }

    if (this.albumTypesCreationForm.valid) {
      this.albumTypesService
        .createNewAlbumType(albumTypeToCreate)
        .pipe(take(1))
        .subscribe((response) => {
          let newAlbumType = response;
          this.nzNotificationService.success(
            `Successful Operation`,
            `The album type ${newAlbumType.name} is created successfully!`
          );
          this.retrieveAlbumTypesData();
        });
    } else {
      Object.values(this.albumTypesCreationForm.controls).forEach((control) => {
        if (control.invalid) {
          control.markAsDirty();
          control.updateValueAndValidity({ onlySelf: true });
        }
      });
    }
  }

  onAlbumTypesEditFormSubmit(albumTypeId: string): void {
    const albumTypeToEdit: IAlbumTypeUpdateDTO = {
      id: albumTypeId,
      name: this.albumTypesEditForm.value.albumTypeName,
    };

    if (this.albumTypesEditForm.valid) {
      this.albumTypesService
        .updateAlbumType(albumTypeToEdit)
        .pipe(take(1))
        .subscribe((response) => {
          let editedAlbumType = response;
          this.nzNotificationService.success(
            `Successful Operation`,
            `The album type ${editedAlbumType.name} is edited successfully!`
          );
          this.retrieveAlbumTypesData();
        });
    } else {
      Object.values(this.albumTypesEditForm.controls).forEach((control) => {
        if (control.invalid) {
          control.markAsDirty();
          control.updateValueAndValidity({ onlySelf: true });
        }
      });
    }
  }

  showAlbumTypeRemovalModal(albumTypeToRemove: IAlbumType): void {
    this.nzModalService.confirm({
      nzTitle: `Do you really wish to remove ${albumTypeToRemove.name}?`,
      nzOkText: 'Yes',
      nzOkType: 'primary',
      nzOkDanger: true,
      nzOnOk: () => this.handleOkAlbumTypeRemovalModal(albumTypeToRemove),
      nzCancelText: 'No',
      nzOnCancel: () => this.handleCancelAlbumTypeRemovalModal(),
    });
  }

  handleOkAlbumTypeRemovalModal(albumTypeToRemove: IAlbumType): void {
    this.albumTypesService
      .deleteAlbumType(albumTypeToRemove.id)
      .subscribe(() => {
        this.nzNotificationService.success(
          'Successful Operation',
          `The album type ${albumTypeToRemove.name} has been removed!`
        );
        this.retrieveAlbumTypesData();
      });
  }

  handleCancelAlbumTypeRemovalModal(): void {
    this.nzNotificationService.info(
      `Aborted Operation`,
      `Album Type removal cancelled`
    );
  }

  ngOnInit(): void {
    this.retrieveAlbumTypesData();
  }

  private retrieveAlbumTypesData(): void {
    this.isLoading = true;
    this.albumTypesService.getAlbumTypesWithFullDetails().subscribe((data) => {
      this.albumTypesData = [];
      data
        .filter((albumType) => !albumType.isDeleted)
        .map((albumType) => {
          this.albumTypesData.push({
            albumType: albumType,
            isEditingModalVisible: false,
          });
        });
      this.albumTypesDisplayData = [...this.albumTypesData];
      this.isLoading = false;
    });
  }
}

export interface IAlbumTypeTableData {
  albumType: IAlbumType;
  isEditingModalVisible: boolean;
}

export interface IAlbumTypeActionForm {
  albumTypeName: FormControl<string>;
}
