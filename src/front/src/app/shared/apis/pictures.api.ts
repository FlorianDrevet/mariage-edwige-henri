import {Injectable} from '@angular/core';
import {AxiosService} from "../services/axios.service";
import {MethodEnum} from "../enums/method.enum";
import {PictureModel} from "../models/picture.model";
import {PaginatedResponse} from "../models/paginated-response.model";
import {PhotoBoothModel} from "../models/phoroBooth.model";

@Injectable({
  providedIn: 'root'
})
export class PicturesApi {

  private static pageSize = 10;

  constructor(private axiosService: AxiosService) {
  }

  public getPictures(pageNumber: number, filter: string): Promise<PaginatedResponse<PictureModel>> {
    return this.axiosService.request(MethodEnum.GET, '/pictures/' + filter, {
      "pageNumber": pageNumber,
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
