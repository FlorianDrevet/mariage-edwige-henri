import {Injectable} from '@angular/core';
import {AxiosService} from "../services/axios.service";
import {MethodEnum} from "../enums/method.enum";
import {UserModel} from "../models/user.model";
import {GuestModel, PostGuestModel} from "../models/guest.model";
import {PictureModel} from "../models/picture.model";
import {HttpHeaders} from "@angular/common/http";
import {PhotoBoothModel} from "../models/phoroBooth.model";

@Injectable({
  providedIn: 'root'
})
export class PicturesApi {

  private static pageSize = 10;

  constructor(private axiosService: AxiosService) {
  }

  public getPictures(pageIndex: number, filter: string): Promise<PictureModel[]> {
    return this.axiosService.request(MethodEnum.GET, '/pictures/' + filter, {
      "page": pageIndex,
      "pageSize": PicturesApi.pageSize
    });
  }

  public getPicturesPhotoBooth(): Promise<PictureModel[]> {
    return this.axiosService.request(MethodEnum.GET, '/pictures-photo-booth', {});
  }

  public getPicturesPhotograph(): Promise<PictureModel[]> {
    return this.axiosService.request(MethodEnum.GET, '/pictures-photograph', {});
  }

  public deletePicture(id: string): Promise<void> {
    return this.axiosService.request(MethodEnum.DELETE, '/pictures/' + id, {});
  }

  public uploadPicture(formData: FormData): Promise<object> {
    return this.axiosService.request(MethodEnum.POST, '/pictures', formData, {},  true);
  }

  public addFavoritePicture(id: string): Promise<object> {
    return this.axiosService.request(MethodEnum.POST, '/pictures/' + id + '/favorites', {});
  }

  public removeFavoritePicture(id: string): Promise<object> {
    return this.axiosService.request(MethodEnum.DELETE, '/pictures/' + id + '/favorites', {});
  }
}
