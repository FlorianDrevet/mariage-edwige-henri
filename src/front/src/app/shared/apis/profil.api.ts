import { Injectable } from '@angular/core';
import {AxiosService} from "../services/axios.service";
import {UserModel} from "../models/user.model";
import {MethodEnum} from "../enums/method.enum";

@Injectable({
  providedIn: 'root'
})
export class ProfilApi {

  constructor(private axiosService: AxiosService) { }

  public putGuestIsComing(guestId: string, isComing: boolean): Promise<UserModel[]> {
    return this.axiosService.request(MethodEnum.PUT, `/user-infos/is-coming`, {
      "isComing": isComing,
      "guestId": guestId
    });
  }
}
