import { IRole } from "../roles/roles";

export interface IApplicationUserDetails {
  userName: string;
  roles: IRole[];
  email: string;
}
