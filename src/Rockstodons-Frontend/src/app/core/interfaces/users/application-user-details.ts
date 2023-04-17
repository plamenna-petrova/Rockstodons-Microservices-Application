import { IRole } from "../roles/roles";

export interface IApplicationUserDetails {
  id: string;
  userName: string;
  roles: IRole[];
  email: string;
}
