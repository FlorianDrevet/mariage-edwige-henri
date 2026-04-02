import { Pipe, PipeTransform } from '@angular/core';
import {ProductInterface} from "../interfaces/product.interface";
import {CategoryEnum} from "../enums/category.enum";

@Pipe({
  standalone: false,
  name: 'giftCategory'
})
export class GiftCategoryPipe implements PipeTransform {

  transform(value: ProductInterface[], filter: CategoryEnum): ProductInterface[] {
    return value.filter(gift => gift.category === filter);
  }

}
