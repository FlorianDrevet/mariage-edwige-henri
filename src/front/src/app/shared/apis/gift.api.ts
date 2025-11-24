import {Injectable} from '@angular/core';
import {AxiosService} from "../services/axios.service";
import {MethodEnum} from "../enums/method.enum";
import {ProductInterface} from "../interfaces/product.interface";
import {GiftGiverInterface} from "../interfaces/giftGiver.interface";

@Injectable({
  providedIn: 'root'
})
export class GiftApi {

  constructor(private axiosService: AxiosService) { }

  public getProducts(): Promise<ProductInterface[]> {
    return this.axiosService.request(MethodEnum.GET, '/wedding-list', null);
  }

  public getProductById(id: string): Promise<ProductInterface> {
    return this.axiosService.request(MethodEnum.GET, `/wedding-list/gift/${id}`, null);
  }

  public postGiftGiver(id: string, giftGiver: GiftGiverInterface): Promise<ProductInterface> {
    return this.axiosService.request(MethodEnum.POST, `/wedding-list/${id}/participation`, giftGiver);
  }
}
