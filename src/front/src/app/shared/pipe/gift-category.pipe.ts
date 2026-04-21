import { Pipe, PipeTransform } from '@angular/core';
import {ProductInterface} from "../interfaces/product.interface";

@Pipe({
  standalone: false,
  name: 'giftCategory'
})
export class GiftCategoryPipe implements PipeTransform {

  transform(value: ProductInterface[], filter: string): ProductInterface[] {
    return value.filter(gift => gift.category === filter);
  }

}
