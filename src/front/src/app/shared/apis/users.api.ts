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
}
