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
import { IPerformer } from 'src/app/core/interfaces/performers/performer';
import { IPerformerCreateDTO } from 'src/app/core/interfaces/performers/performer-create-dto';
import { IPerformerUpdateDTO } from 'src/app/core/interfaces/performers/performer-update-dto';
import { PerformersService } from 'src/app/core/services/performers.service';
import { operationSuccessMessage, recordRemovalConfirmationModalCancelText, recordRemovalConfirmationModalOkDanger, recordRemovalConfirmationModalOkText, recordRemovalConfirmationModalOkType, recordRemovalConfirmationModalTitle, removalOperationCancelMessage } from 'src/app/core/utils/global-constants';

@Component({
  selector: 'app-performers-management',
  templateUrl: './performers-management.component.html',
  styleUrls: ['./performers-management.component.scss'],
})
export class PerformersManagementComponent {
  searchByNameValue = '';
  searchByCountryValue = '';
  isLoading = false;
  isPerformersNameSearchTriggerVisible = false;
  isPerformersCountrySearchTriggerVisible = false;
  performersData!: IPerformerTableData[];
  performersDisplayData!: IPerformerTableData[];
  isPerformersCreationModalVisible = false;

  performersCreationForm!: FormGroup;
  performersEditForm!: FormGroup;

  countryList: string[] = [
    "Afghanistan",
    "Albania",
    "Algeria",
    "American Samoa",
    "Andorra",
    "Angola",
    "Anguilla",
    "Antarctica",
    "Antigua and Barbuda",
    "Argentina",
    "Armenia",
    "Aruba",
    "Australia",
    "Austria",
    "Azerbaijan",
    "Bahamas",
    "Bahrain",
    "Bangladesh",
    "Barbados",
    "Belarus",
    "Belgium",
    "Belize",
    "Benin",
    "Bermuda",
    "Bhutan",
    "Bolivia",
    "Bonaire, Sint Eustatius and Saba",
    "Bosnia and Herzegovina",
    "Botswana",
    "Bouvet Island",
    "Brazil",
    "British Indian Ocean Territory ",
    "Brunei Darussalam",
    "Bulgaria",
    "Burkina Faso",
    "Burundi",
    "Cabo Verde",
    "Cambodia",
    "Cameroon",
    "Canada",
    "Cayman Islands ",
    "Central African Republic ",
    "Chad",
    "Chile",
    "China",
    "Christmas Island",
    "Cocos Islands",
    "Colombia",
    "Comoros ",
    "Cook Islands ",
    "Costa Rica",
    "Croatia",
    "Cuba",
    "Curaçao",
    "Cyprus",
    "Czechia",
    "Côte d'Ivoire",
    "Denmark",
    "Djibouti",
    "Dominica",
    "Dominican Republic ",
    "Ecuador",
    "Egypt",
    "El Salvador",
    "Equatorial Guinea",
    "Eritrea",
    "Estonia",
    "Eswatini",
    "Ethiopia",
    "Falkland Islands",
    "Faroe Islands ",
    "Fiji",
    "Finland",
    "France",
    "French Guiana",
    "French Polynesia",
    "French Southern Territories",
    "Gabon",
    "Gambia ",
    "Georgia",
    "Germany",
    "Ghana",
    "Gibraltar",
    "Greece",
    "Greenland",
    "Grenada",
    "Guadeloupe",
    "Guam",
    "Guatemala",
    "Guernsey",
    "Guinea",
    "Guinea-Bissau",
    "Guyana",
    "Haiti",
    "Heard Island and McDonald Islands",
    "Holy See ",
    "Honduras",
    "Hong Kong",
    "Hungary",
    "Iceland",
    "India",
    "Indonesia",
    "Iran",
    "Iraq",
    "Ireland",
    "Isle of Man",
    "Israel",
    "Italy",
    "Jamaica",
    "Japan",
    "Jersey",
    "Jordan",
    "Kazakhstan",
    "Kenya",
    "Kiribati",
    "Korea",
    "Kuwait",
    "Kyrgyzstan",
    "Lao People's Democratic Republic ",
    "Latvia",
    "Lebanon",
    "Lesotho",
    "Liberia",
    "Libya",
    "Liechtenstein",
    "Lithuania",
    "Luxembourg",
    "Macao",
    "Madagascar",
    "Malawi",
    "Malaysia",
    "Maldives",
    "Mali",
    "Malta",
    "Marshall Islands ",
    "Martinique",
    "Mauritania",
    "Mauritius",
    "Mayotte",
    "Mexico",
    "Micronesia",
    "Moldova",
    "Monaco",
    "Mongolia",
    "Montenegro",
    "Montserrat",
    "Morocco",
    "Mozambique",
    "Myanmar",
    "Namibia",
    "Nauru",
    "Nepal",
    "Netherlands",
    "New Caledonia",
    "New Zealand",
    "Nicaragua",
    "Niger ",
    "Nigeria",
    "Niue",
    "Norfolk Island",
    "Northern Mariana Islands",
    "Norway",
    "Oman",
    "Pakistan",
    "Palau",
    "Palestine, State of",
    "Panama",
    "Papua New Guinea",
    "Paraguay",
    "Peru",
    "Philippines",
    "Pitcairn",
    "Poland",
    "Portugal",
    "Puerto Rico",
    "Qatar",
    "Republic of North Macedonia",
    "Romania",
    "Russian Federation ",
    "Rwanda",
    "Réunion",
    "Saint Barthélemy",
    "Saint Helena, Ascension and Tristan da Cunha",
    "Saint Kitts and Nevis",
    "Saint Lucia",
    "Saint Martin",
    "Saint Pierre and Miquelon",
    "Saint Vincent and the Grenadines",
    "Samoa",
    "San Marino",
    "Sao Tome and Principe",
    "Saudi Arabia",
    "Senegal",
    "Serbia",
    "Seychelles",
    "Sierra Leone",
    "Singapore",
    "Sint Maarten",
    "Slovakia",
    "Slovenia",
    "Solomon Islands",
    "Somalia",
    "South Africa",
    "South Georgia and the South Sandwich Islands",
    "South Sudan",
    "Spain",
    "Sri Lanka",
    "Sudan ",
    "Suriname",
    "Svalbard and Jan Mayen",
    "Sweden",
    "Switzerland",
    "Syrian Arab Republic",
    "Taiwan",
    "Tajikistan",
    "Tanzania",
    "Thailand",
    "Timor-Leste",
    "Togo",
    "Tokelau",
    "Tonga",
    "Trinidad and Tobago",
    "Tunisia",
    "Turkey",
    "Turkmenistan",
    "Turks and Caicos Islands",
    "Tuvalu",
    "Uganda",
    "Ukraine",
    "United Arab Emirates",
    "United Kingdom of Great Britain and Northern Ireland",
    "United States Minor Outlying Islands",
    "United States of America",
    "Uruguay",
    "Uzbekistan",
    "Vanuatu",
    "Venezuela Bolivarian Republic",
    "Viet Nam",
    "Virgin Islands (British)",
    "Virgin Islands (U.S.)",
    "Wallis and Futuna",
    "Western Sahara",
    "Yemen",
    "Zambia",
    "Zimbabwe",
    "Åland Islands"
  ];

  filteredCountriesList: string[] = [];

  listOfPerformersColumns = [
    {
      title: 'Name',
      compare: (a: IPerformer, b: IPerformer) => a.name.localeCompare(b.name),
      priority: 1,
      isSearchTriggerVisible: this.isPerformersNameSearchTriggerVisible,
      searchValue: this.searchByNameValue,
    },
    {
      title: 'Country',
      compare: (a: IPerformer, b: IPerformer) =>
        a.country.localeCompare(b.country),
      priority: 2,
      isSearchTriggerVisible: this.isPerformersCountrySearchTriggerVisible,
      searchValue: this.searchByCountryValue,
    },
  ];

  constructor(
    private performersService: PerformersService,
    private nzNotificationService: NzNotificationService,
    private nzModalService: NzModalService
  ) {
    this.buildPerformersActionForms();
    this.filteredCountriesList = this.countryList;
  }

  buildPerformersActionForms(): void {
    this.performersCreationForm = new FormGroup<IPerformerActionForm>({
      name: new FormControl('', {
        validators: Validators.compose([
          Validators.required,
          Validators.minLength(2),
          Validators.maxLength(30),
        ]),
        nonNullable: true,
      }),
      country: new FormControl('', {
        validators: Validators.compose([
          Validators.required,
          Validators.minLength(4),
          Validators.maxLength(58),
        ]),
        nonNullable: true,
      }),
      history: new FormControl('', {
        validators: Validators.compose([
          Validators.required,
          Validators.minLength(20),
          Validators.maxLength(500),
        ]),
        nonNullable: true,
      }),
    });
    this.performersEditForm = new FormGroup<IPerformerActionForm>({
      name: new FormControl('', {
        validators: Validators.compose([
          Validators.required,
          Validators.minLength(2),
          Validators.maxLength(30),
        ]),
        nonNullable: true,
      }),
      country: new FormControl('', {
        validators: Validators.compose([
          Validators.required,
          Validators.minLength(4),
          Validators.maxLength(58),
        ]),
        nonNullable: true,
      }),
      history: new FormControl('', {
        validators: Validators.compose([
          Validators.required,
          Validators.minLength(20),
          Validators.maxLength(500),
        ]),
        nonNullable: true,
      }),
    });
  }

  get name(): AbstractControl {
    return this.performersCreationForm.get('name')!;
  }

  get country(): AbstractControl {
    return this.performersCreationForm.get('country')!;
  }

  get history(): AbstractControl {
    return this.performersCreationForm.get('history')!;
  }

  onLoadPerformersDataClick(): void {
    this.retrievePerformersData();
  }

  resetPerformersSearch() {
    this.searchByNameValue = '';
    this.searchByCountryValue = '';
    this.isPerformersNameSearchTriggerVisible = false;
    this.isPerformersCountrySearchTriggerVisible = false;
    this.retrievePerformersData();
  }

  searchForPerformersByName(): void {
    this.isPerformersNameSearchTriggerVisible = false;
    this.performersDisplayData = this.performersData.filter(
      (data: IPerformerTableData) =>
        data.performer.name
          .toLowerCase()
          .indexOf(this.searchByNameValue.toLowerCase()) !== -1
    );
  }

  searchForPerformersByCountry(): void {
    this.isPerformersCountrySearchTriggerVisible = false;
    this.performersDisplayData = this.performersData.filter(
      (data: IPerformerTableData) =>
        data.performer.country
          .toLowerCase()
          .indexOf(this.searchByCountryValue.toLowerCase()) !== -1
    );
  }

  showPerformerCreationModal(): void {
    this.isPerformersCreationModalVisible = true;
    this.performersCreationForm.reset();
  }

  handleOkPerformerCreationModal(): void {
    this.onPerformersCreationFormSubmit();
  }

  handleCancelPerformerCreationModal(): void {
    this.isPerformersCreationModalVisible = false;
  }

  showPerformerEditModal(performerTableDatum: IPerformerTableData): void {
    performerTableDatum.isEditingModalVisible = true;
    this.performersEditForm.setValue({
      name: performerTableDatum.performer.name,
      country: performerTableDatum.performer.country,
      history: performerTableDatum.performer.history,
    });
  }

  handleOkPerformerEditModal(performerTableDatum: IPerformerTableData): void {
    this.onPerformersEditFormSubmit(performerTableDatum.performer.id).subscribe((success) => {
      if (success) {
        performerTableDatum.isEditingModalVisible = false;
      }
    });
  }

  handleCancelPerformerEditModal(performerTableDatum: IPerformerTableData): void {
    performerTableDatum.isEditingModalVisible = false;
  }

  onPerformersCreationFormSubmit(): void {
    const name: string =
      this.performersCreationForm.value.name;

    const performerToCreate: IPerformerCreateDTO = {
      ...this.performersCreationForm.value,
    };

    const isPerformerExisting = this.performersData.some(
      (data) => data.performer.name === name
    );

    if (isPerformerExisting) {
      this.nzNotificationService.error(
        `Error`,
        `The performer ${name} already exists!`,
        {
          nzPauseOnHover: true
        }
      );
      return;
    }

    if (this.performersCreationForm.valid) {
      this.performersService
        .createNewPerformer(performerToCreate)
        .pipe(take(1))
        .subscribe((response) => {
          let newPerformer = response;
          this.nzNotificationService.success(
            operationSuccessMessage,
            `The performer ${newPerformer.name} is created successfully!`,
            {
              nzPauseOnHover: true
            }
          );

          this.isPerformersCreationModalVisible = false;
          this.retrievePerformersData();
        });
    } else {
      Object.values(this.performersCreationForm.controls).forEach((control) => {
        if (control.invalid) {
          control.markAsDirty();
          control.updateValueAndValidity({ onlySelf: true });
        }
      });
    }
  }

  onPerformersEditFormSubmit(performerId: string): Observable<boolean> {
    let isPerformersEditFormSubmitSuccessful: boolean = true;

    const performerToEdit: IPerformerUpdateDTO = {
      id: performerId,
      ...this.performersEditForm.value,
    };

    if (this.performersEditForm.valid) {
      this.performersService
        .updatePerformer(performerToEdit)
        .pipe(take(1))
        .subscribe((response) => {
          let editedPerformer = response;
          this.nzNotificationService.success(
            operationSuccessMessage,
            `The performer ${editedPerformer.name} is edited successfully!`,
            {
              nzPauseOnHover: true
            }
          );
          this.retrievePerformersData();
        });
    } else {
      isPerformersEditFormSubmitSuccessful = false;
      Object.values(this.performersEditForm.controls).forEach((control) => {
        if (control.invalid) {
          control.markAsDirty();
          control.updateValueAndValidity({ onlySelf: true });
        }
      });
    }

    return of(isPerformersEditFormSubmitSuccessful);
  }

  showPerformerRemovalModal(performerToRemove: IPerformer): void {
    this.nzModalService.confirm({
      nzTitle: recordRemovalConfirmationModalTitle(performerToRemove.name),
      nzOkText: recordRemovalConfirmationModalOkText,
      nzOkType: recordRemovalConfirmationModalOkType,
      nzOkDanger: recordRemovalConfirmationModalOkDanger,
      nzOnOk: () => this.handleOkPerformerRemovalModal(performerToRemove),
      nzCancelText: recordRemovalConfirmationModalCancelText,
      nzOnCancel: () => this.handleCancelPerformerRemovalModal()
    });
  }

  handleOkPerformerRemovalModal(performerToRemove: IPerformer): void {
    this.performersService.deletePerformer(performerToRemove.id).subscribe(() => {
      this.nzNotificationService.success(
        operationSuccessMessage,
        `The performer ${performerToRemove.name} has been removed!`,
        {
          nzPauseOnHover: true
        }
      );
      this.retrievePerformersData();
    });
  }

  handleCancelPerformerRemovalModal(): void{
    this.nzNotificationService.info(
      removalOperationCancelMessage,
      `Performer removal cancelled`
    );
  }

  onCountryAutocompleteChange(value: string): void {
    this.filteredCountriesList = this.countryList
      .filter(country => country.toLowerCase().indexOf(value.toLowerCase()) !== -1);
  }

  ngOnInit(): void {
    this.retrievePerformersData();
  }

  private retrievePerformersData(): void {
    this.isLoading = true;
    this.performersService.getPerformersWithFullDetails().subscribe((data) => {
      this.performersData = [];
      data
        .filter((performer) => !performer.isDeleted)
        .map((performer) => {
          this.performersData.push({
            performer: { ...performer },
            expanded: false,
            isEditingModalVisible: false,
          });
        });
      this.performersDisplayData = [...this.performersData];
      this.isLoading = false;
    });
  }
}

export interface IPerformerTableData {
  performer: IPerformer;
  expanded: boolean;
  isEditingModalVisible: boolean;
}

export interface IPerformerActionForm {
  name: FormControl<string>;
  country: FormControl<string>;
  history: FormControl<string>;
}
