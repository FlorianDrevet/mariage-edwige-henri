import {GiftGiverInterface} from "./giftGiver.interface";

export interface ProductInterface {
  id: string,
  name: string,
  price: number,
  urlImage: string,
  participation: number,
  giftGivers: GiftGiverInterface[],
  category: string,
}
