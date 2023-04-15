import { Component } from '@angular/core';
import { IAlbumType } from 'src/app/core/interfaces/album-type';
import { AlbumTypesService } from 'src/app/core/services/album-types.service';

@Component({
  selector: 'app-album-types-management',
  templateUrl: './album-types-management.component.html',
  styleUrls: ['./album-types-management.component.scss']
})
export class AlbumTypesManagementComponent {
  searchValue = '';
  isLoading = false;
  isAlbumTypesSearchTriggerVisible = false;
  albumTypesData!: IAlbumType[];
  albumTypesDisplayData!: IAlbumType[];

  constructor(private albumTypesService: AlbumTypesService) {

  }

  onLoadAlbumTypesDataClick(): void {
    this.retrievealbumTypesData();
  }

  resetAlbumTypesSearch(): void {
    this.searchValue = '';
    this.searchForalbumTypes();
  }

  searchForalbumTypes(): void {
    this.isAlbumTypesSearchTriggerVisible = false;
    this.albumTypesDisplayData = this.albumTypesData.filter(
      (albumType: IAlbumType) =>
        albumType.name
          .toLowerCase()
          .indexOf(this.searchValue.toLocaleLowerCase()) !== -1
    );
  }

  ngOnInit(): void {
    this.retrievealbumTypesData();
  }

  private retrievealbumTypesData(): void {
    this.isLoading = true;
    this.albumTypesService.getAlbumTypesWithFullDetails().subscribe((data) => {
      this.albumTypesData = data;
      this.albumTypesDisplayData = [...this.albumTypesData];
      this.isLoading = false;
    });
  }
}
