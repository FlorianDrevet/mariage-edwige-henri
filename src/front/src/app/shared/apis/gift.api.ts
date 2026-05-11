import {inject, Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {Observable} from 'rxjs';
import {AxiosService} from "../services/axios.service";
import {MethodEnum} from "../enums/method.enum";
import {ProductInterface} from "../interfaces/product.interface";
import {GiftGiverInterface} from "../interfaces/giftGiver.interface";
import {GiftCategoryInterface} from "../enums/category.enum";
import {environment} from "../../../environments/environment";

@Injectable({
  providedIn: 'root'
})
export class GiftApi {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = environment['API_URL'];

  constructor(private readonly axiosService: AxiosService) { }

  public getProducts(): Promise<ProductInterface[]> {
    return this.axiosService.request(MethodEnum.GET, '/wedding-list', null);
  }

  public getProductById(id: string): Promise<ProductInterface> {
    return this.axiosService.request(MethodEnum.GET, `/wedding-list/gift/${id}`, null);
  }

  public postGiftGiver(id: string, giftGiver: GiftGiverInterface): Promise<ProductInterface> {
    return this.axiosService.request(MethodEnum.POST, `/wedding-list/${id}/participation`, giftGiver);
  }

  public updateGift(id: string, formData: FormData): Observable<ProductInterface> {
    return this.http.put<ProductInterface>(`${this.baseUrl}/wedding-list/${id}`, formData);
  }

  public deleteGift(id: string): Promise<void> {
    return this.axiosService.request(MethodEnum.DELETE, `/wedding-list/${id}`, null);
  }

  public getCategories(): Promise<GiftCategoryInterface[]> {
    return this.axiosService.request(MethodEnum.GET, '/wedding-list/categories', null);
  }

  public createCategory(name: string): Promise<GiftCategoryInterface> {
    return this.axiosService.request(MethodEnum.POST, '/wedding-list/categories', {name});
  }

  public deleteCategory(id: string): Promise<void> {
    return this.axiosService.request(MethodEnum.DELETE, `/wedding-list/categories/${id}`, null);
  }
}
