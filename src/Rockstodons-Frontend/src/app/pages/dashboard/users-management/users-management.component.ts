import { Component } from '@angular/core';
import { NzModalService } from 'ng-zorro-antd/modal';
import { NzNotificationService } from 'ng-zorro-antd/notification';
import { Observable } from 'rxjs';
import { IRole } from 'src/app/core/interfaces/roles/roles';
import { IApplicationUser } from 'src/app/core/interfaces/users/application-user';
import { IApplicationUserDetails } from 'src/app/core/interfaces/users/application-user-details';
import { AuthService } from 'src/app/core/services/auth.service';
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

  applicationUser$: Observable<IApplicationUser | null>;
  username?: string;
  role?: string;

  constructor(
    private usersService: UsersService,
    private rolesService: RolesService,
    private authService: AuthService,
    private nzNotificationService: NzNotificationService,
    private nzModalService: NzModalService
  ) {
    this.applicationUser$ = this.authService.currentUser$;
    this.applicationUser$.subscribe(user => {
      this.username = user?.username;
      this.role = user?.role;
    });
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

  isCurrentUser(user: IApplicationUserDetails): boolean {
    return user.userName === this.username &&
      user.roles.map(role => role.name).includes(this.role!);
  }

  showUserRemovalModal(userToRemove: IApplicationUserDetails): void {
    this.nzModalService.confirm({
      nzTitle: `Do you really wish to remove ${userToRemove.userName}?`,
      nzOkText: 'Yes',
      nzOkType: 'primary',
      nzOkDanger: true,
      nzOnOk: () => this.handleOkUserRemovalModal(userToRemove),
      nzCancelText: 'No',
      nzOnCancel: () => this.handleCancelUserRemovalModal()
    })
  }

  handleOkUserRemovalModal(userToRemove: IApplicationUserDetails): void {
    this.usersService.deleteUserPermanently(userToRemove.id).subscribe(() => {
      this.nzNotificationService.success(
        'Successful Operation',
        `The user ${userToRemove.userName} has been removed!`,
        {
          nzPauseOnHover: true
        }
      );
      this.retrieveUsersData();
    });
  }

  handleCancelUserRemovalModal(): void {
    this.nzNotificationService.info(
      `Aborted operation`,
      `User removal cancelled`,
      {
        nzPauseOnHover: true
      }
    );
  }

  ngOnInit(): void {
    this.retrieveUsersData();
  }

  private retrieveUsersData(): void {
    this.isLoading = true;
    this.rolesService.getRolesWithFullDetails().subscribe((data) => {
      this.rolesData = data;
    });
    this.usersService.getAllUsers().subscribe((data) => {
      this.usersData = [];
      const mappedRoles = this.rolesData.map(({ id, name }) => ({ id, name }));

      data.map((user: any) => {
        const { id, userName, email, roles } = user;
        const userRoles: IRole[] = [];
        const userAssignedRolesIds = roles.map((role: any) => role.roleId);

        for (const role of mappedRoles) {
          if (userAssignedRolesIds.includes(role.id)) {
            userRoles.push(role);
          }
        }

        const mappedUser: IApplicationUserDetails = {
          id: id,
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
