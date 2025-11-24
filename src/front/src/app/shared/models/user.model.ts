import {GuestModel} from "./guest.model";

export interface UserModel {
  id: string;
  username: string;
  email: string | null;
  guests: GuestModel[];
}
