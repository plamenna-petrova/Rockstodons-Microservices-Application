import { Component } from '@angular/core';
import {
  AbstractControl,
  FormControl,
  FormGroup,
  Validators,
} from '@angular/forms';
import { NzModalService } from 'ng-zorro-antd/modal';
import { NzNotificationService } from 'ng-zorro-antd/notification';
import { retry, take } from 'rxjs';
import { IGenre } from 'src/app/core/interfaces/genres/genre';
import { IGenreCreateDTO } from 'src/app/core/interfaces/genres/genre-create-dto';
import { IGenreUpdateDTO } from 'src/app/core/interfaces/genres/genre-update-dto';
import { GenresService } from 'src/app/core/services/genres.service';

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

  constructor(
    private genresService: GenresService,
    private nzNotificationService: NzNotificationService,
    private nzModalService: NzModalService
  ) {
    this.buildGenresActionForms();
  }

  buildGenresActionForms(): void {
    const genresActionFormGroup = new FormGroup<IGenreActionForm>({
      genreName: new FormControl('', {
        validators: Validators.compose([
          Validators.required,
          Validators.minLength(3),
          Validators.maxLength(20),
        ]),
        nonNullable: true,
      }),
    });
    this.genresCreationForm = this.genresEditForm = genresActionFormGroup;
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
  }

  handleOkGenreCreationModal(): void {
    this.isGenreCreationModalVisible = false;
    this.onGenresCreationFormSubmit();
  }

  handleCancelGenreCreationModal(): void {
    this.isGenreCreationModalVisible = false;
  }

  showGenreEditModal(genreTableDatum: IGenreTableData): void {
    genreTableDatum.isEditingModalVisible = true;
    this.genresEditForm.controls['genreName'].setValue(
      genreTableDatum.genre.name
    );
  }

  handleOkGenreEditModal(genreTableDatum: IGenreTableData): void {
    genreTableDatum.isEditingModalVisible = false;
    this.onGenresEditFormSubmit(genreTableDatum.genre.id);
  }

  handleCancelGenreEditModal(genreTableDatum: IGenreTableData): void {
    genreTableDatum.isEditingModalVisible = false;
  }

  onGenresCreationFormSubmit(): void {
    const genreName: string = this.genresCreationForm.value.genreName;

    const genreToCreate: IGenreCreateDTO = {
      name: genreName,
    };

    const isGenreExisting = this.genresData.some(
      (data) => data.genre.name === genreToCreate.name
    );

    if (isGenreExisting) {
      this.nzNotificationService.error(
        `Error`,
        `The genre ${genreName} already exists!`
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
            `Successful Operation`,
            `The genre ${newGenre.name} is created successfully!`
          );
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

  onGenresEditFormSubmit(genreId: string): void {
    const genreToEdit: IGenreUpdateDTO = {
      id: genreId,
      name: this.genresEditForm.value.genreName,
    };

    if (this.genresEditForm.valid) {
      this.genresService
        .updateGenre(genreToEdit)
        .pipe(take(1))
        .subscribe((response) => {
          let editedGenre = response;
          this.nzNotificationService.success(
            `Successful Operation`,
            `The genre ${editedGenre.name} is edited successfully!`
          );
          this.retrieveGenresData();
        });
    } else {
      Object.values(this.genresEditForm.controls).forEach((control) => {
        if (control.invalid) {
          control.markAsDirty();
          control.updateValueAndValidity({ onlySelf: true });
        }
      });
    }
  }

  showGenreRemovalModal(genreToRemove: IGenre): void {
    this.nzModalService.confirm({
      nzTitle: `Do you really wish to remove ${genreToRemove.name}?`,
      nzOkText: 'Yes',
      nzOkType: 'primary',
      nzOkDanger: true,
      nzOnOk: () => this.handleOkGenreRemovalModal(genreToRemove),
      nzCancelText: 'No',
      nzOnCancel: () => this.handleCancelGenreRemovalModal()
    });
  }

  handleOkGenreRemovalModal(genreToRemove: IGenre): void {
    this.genresService.deleteGenre(genreToRemove.id).subscribe(() => {
      this.nzNotificationService.success(
        'Successful Operation',
        `The genre ${genreToRemove.name} has been removed!`
      );
      this.retrieveGenresData();
    });
  }

  handleCancelGenreRemovalModal(): void {
    this.nzNotificationService.info(
      `Aborted operation`,
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
