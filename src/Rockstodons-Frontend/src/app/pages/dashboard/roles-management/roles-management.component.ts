import { Component } from '@angular/core';
import { IRole } from 'src/app/core/interfaces/roles/roles';
import { RolesService } from 'src/app/core/services/roles.service';

@Component({
  selector: 'app-roles-management',
  templateUrl: './roles-management.component.html',
  styleUrls: ['./roles-management.component.scss']
})
export class RolesManagementComponent {
  searchValue = '';
  isLoading = false;
  isRolesSearchTriggerVisible = false;
  rolesData!: IRoleTableData[];
  rolesDisplayData!: IRoleTableData[];

  constructor(private rolesService: RolesService) {

  }

  onLoadRolesDataClick(): void {
    this.retrieveRolesData();
  }

  resetRolesSearch(): void {
    this.searchValue = '';
    this.searchForRoles();
  }

  searchForRoles(): void {
    this.isRolesSearchTriggerVisible = false;
    this.rolesDisplayData = this.rolesData
      .filter((data: IRoleTableData) =>
        data.role.name
          .toLowerCase()
          .indexOf(this.searchValue.toLowerCase()) !== -1);
  }

  ngOnInit(): void {
    this.retrieveRolesData();
  }

  private retrieveRolesData(): void {
    this.isLoading = true;
    this.rolesService.getAllRoles().subscribe((data) => {
      this.rolesData = [];
      data.map((role) => {
        this.rolesData.push({
          role: role,
          isEditingModalVisible: false
        });
      });
      this.rolesDisplayData = [...this.rolesData];
      this.isLoading = false;
    })
  }
}

export interface IRoleTableData {
  role: IRole;
  isEditingModalVisible: boolean;
}
