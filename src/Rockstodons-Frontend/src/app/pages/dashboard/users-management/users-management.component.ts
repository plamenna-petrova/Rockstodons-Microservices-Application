import { Component } from '@angular/core';
import { IRole } from 'src/app/core/interfaces/roles/roles';
import { IApplicationUser } from 'src/app/core/interfaces/users/application-user';
import { IApplicationUserDetails } from 'src/app/core/interfaces/users/application-user-details';
import { RolesService } from 'src/app/core/services/roles.service';
import { UsersService } from 'src/app/core/services/users.service';

@Component({
  selector: 'app-users-management',
  templateUrl: './users-management.component.html',
  styleUrls: ['./users-management.component.scss'],
})
export class UsersManagementComponent {
  searchValue = '';
  isLoading = false;
  isUsersSearchTriggerVisible = false;
  rolesData!: IRole[];
  usersData!: IUserTableData[];
  usersDisplayData!: IUserTableData[];

  constructor(
    private usersService: UsersService,
    private rolesService: RolesService
  ) {

  }

  onLoadUsersDataClick(): void {
    this.retrieveUsersData();
  }

  resetUsersSearch(): void {
    this.searchValue = '';
    this.searchForUsers();
  }

  searchForUsers(): void {
    this.isUsersSearchTriggerVisible = false;
    this.usersDisplayData = this.usersData
      .filter((data: IUserTableData) =>
        data.applicationUser.userName
          .toLowerCase()
          .indexOf(this.searchValue.toLowerCase()) !== -1
      );
  }

  ngOnInit(): void {
    this.retrieveUsersData();
  }

  private retrieveUsersData(): void {
    this.isLoading = true;
    this.rolesService.getRolesWithFullDetails().subscribe((data) => {
      this.rolesData = data;
      console.log('roles data');
      console.log(this.rolesData);
    });
    this.usersService.getAllUsers().subscribe((data) => {
      this.usersData = [];
      const mappedRoles = this.rolesData.map(({ id, name }) => ({ id, name }));
      console.log('mapped role');
      console.log(mappedRoles);
      data.map((user: any) => {
        const { userName, email, roles } = user;
        const userRoles: IRole[] = [];
        const userAssignedRolesIds = roles.map((role: any) => role.roleId);

        for (const role of mappedRoles) {
          if (userAssignedRolesIds.includes(role.id)) {
            userRoles.push(role);
          }
        }

        const mappedUser: IApplicationUserDetails = {
          userName: userName,
          email: email,
          roles: userRoles
        }

        this.usersData.push({
          applicationUser: mappedUser,
          isEditingModalVisible: false,
        });
      });

      this.usersDisplayData = [...this.usersData];
      this.isLoading = false;
    });
  }
}

export interface IUserTableData {
  applicationUser: IApplicationUserDetails;
  isEditingModalVisible: boolean;
}
