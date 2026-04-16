import {Injectable} from '@angular/core';
import {AxiosService} from "../services/axios.service";
import {MethodEnum} from "../enums/method.enum";
import {UserModel} from "../models/user.model";
import {GuestModel, PostGuestModel} from "../models/guest.model";

@Injectable({
  providedIn: 'root'
})
export class UsersApi {

  constructor(private axiosService: AxiosService) {
  }

  public getUsers(): Promise<UserModel[]> {
    return this.axiosService.request(MethodEnum.GET, '/user-infos', null);
  }

  public getUserProfils(): Promise<UserModel> {
    return this.axiosService.request(MethodEnum.GET, '/user-infos/profils', null);
  }

  public postAddGuests(userId: string, guests: PostGuestModel[]): Promise<UserModel> {
    console.log(userId)
    return this.axiosService.request(MethodEnum.POST, '/user-infos/guests', {
      "userId": userId,
      "guests": guests
    });
  }

  public deleteUser(userId: string): Promise<void> {
    return this.axiosService.request(MethodEnum.DELETE, `/user-infos/${userId}`, null);
  }

  public updateGuest(userId: string, guestId: string, guest: PostGuestModel): Promise<UserModel> {
    return this.axiosService.request(MethodEnum.PUT, `/user-infos/${userId}/guests/${guestId}`, guest);
  }

  public deleteGuest(userId: string, guestId: string): Promise<UserModel> {
    return this.axiosService.request(MethodEnum.DELETE, `/user-infos/${userId}/guests/${guestId}`, null);
  }
}
