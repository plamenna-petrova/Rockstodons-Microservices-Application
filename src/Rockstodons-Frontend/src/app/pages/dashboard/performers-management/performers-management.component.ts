import { Component } from '@angular/core';
import { IPerformer } from 'src/app/core/interfaces/performer';
import { PerformersService } from 'src/app/core/services/performers.service';

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
  performersData!: IPerformer[];
  performersDisplayData!: IPerformer[];

  listOfPerformersColumns = [
    {
      title: 'Name',
      compare: (a: IPerformer, b: IPerformer) => a.name.localeCompare(b.name),
      priority: 1,
      isSearchTriggerVisible: this.isPerformersNameSearchTriggerVisible,
      searchValue: this.searchByNameValue
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

  constructor(private performersService: PerformersService) {}

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
      (performer: IPerformer) =>
        performer.name
          .toLowerCase()
          .indexOf(this.searchByNameValue.toLowerCase()) !== -1
    );
  }

  searchForPerformersByCountry(): void {
    this.isPerformersCountrySearchTriggerVisible = false;
    this.performersDisplayData = this.performersData.filter(
      (performer: IPerformer) =>
        performer.country
          .toLowerCase()
          .indexOf(this.searchByCountryValue.toLowerCase()) !== -1
    );
  }

  ngOnInit(): void {
    this.retrievePerformersData();
  }

  private retrievePerformersData(): void {
    this.isLoading = true;
    this.performersService.getPerformersWithFullDetails().subscribe((data) => {
      this.performersData = data;
      this.performersDisplayData = [...this.performersData];
      this.isLoading = false;
    });
  }
}
