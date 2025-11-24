import {GiftGiverInterface} from "./giftGiver.interface";
import {CategoryEnum} from "../enums/category.enum";

export interface ProductInterface {
  id: string,
  name: string,
  price: number,
  urlImage: string,
  participation: number,
  giftGivers: GiftGiverInterface[],
  category: CategoryEnum,
}
