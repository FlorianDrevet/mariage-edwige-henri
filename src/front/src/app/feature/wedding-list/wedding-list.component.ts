import {AfterViewInit, Component} from '@angular/core';
import {GiftApi} from "../../shared/apis/gift.api";
import {ProductInterface} from "../../shared/interfaces/product.interface";
import {AuthService} from "../../shared/services/auth.service";
import {CategoryEnum} from "../../shared/enums/category.enum";

@Component({
  standalone: false,
  selector: 'app-wedding-list',
  templateUrl: './wedding-list.component.html',
  styleUrl: './wedding-list.component.scss'
})
export class WeddingListComponent implements AfterViewInit{
  allProducts: ProductInterface[] = [];
  isLoading: boolean = true;

  constructor(private productApi: GiftApi,
              protected authService: AuthService, ) {
  }

  ngAfterViewInit(): void {
    this.productApi.getProducts().then(products => {
      this.allProducts = products;
      console.log(this.allProducts)
      this.isLoading = false;
    });
  }

  protected readonly CategoryEnum = CategoryEnum;
}
