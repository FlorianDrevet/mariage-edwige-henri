import {GuestModel} from "./guest.model";
import {UserAccommodationModel} from "./accommodation.model";

export interface UserModel {
  id: string;
  username: string;
  email: string | null;
  guests: GuestModel[];
  accommodation: UserAccommodationModel | null;
}
