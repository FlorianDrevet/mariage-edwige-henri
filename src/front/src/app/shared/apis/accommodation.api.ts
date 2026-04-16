import {Injectable} from '@angular/core';
import {AxiosService} from "../services/axios.service";
import {MethodEnum} from "../enums/method.enum";
import {AccommodationModel} from "../models/accommodation.model";
import {UserModel} from "../models/user.model";

@Injectable({
  providedIn: 'root'
})
export class AccommodationApi {

  constructor(private axiosService: AxiosService) {
  }

  public getAll(): Promise<AccommodationModel[]> {
    return this.axiosService.request(MethodEnum.GET, '/accommodations', null);
  }

  public create(title: string, description: string, urlImage: string): Promise<AccommodationModel> {
    return this.axiosService.request(MethodEnum.POST, '/accommodations', {
      title,
      description,
      urlImage
    });
  }

  public update(id: string, title: string, description: string, urlImage: string): Promise<AccommodationModel> {
    return this.axiosService.request(MethodEnum.PUT, `/accommodations/${id}`, {
      title,
      description,
      urlImage
    });
  }

  public delete(id: string): Promise<void> {
    return this.axiosService.request(MethodEnum.DELETE, `/accommodations/${id}`, null);
  }

  public assign(userId: string, accommodationId: string): Promise<UserModel> {
    return this.axiosService.request(MethodEnum.POST, '/accommodations/assign', {
      userId,
      accommodationId
    });
  }

  public removeAssignment(userId: string): Promise<UserModel> {
    return this.axiosService.request(MethodEnum.DELETE, `/accommodations/assign/${userId}`, null);
  }

  public respond(accepted: boolean): Promise<UserModel> {
    return this.axiosService.request(MethodEnum.PUT, '/accommodations/respond', {
      accepted
    });
  }
}
