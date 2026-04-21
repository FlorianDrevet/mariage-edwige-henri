import {Component, Input} from '@angular/core';
import {ProductInterface} from "../../../../shared/interfaces/product.interface";

@Component({
  standalone: false,
  selector: 'app-category-gift',
  templateUrl: './category-gift.component.html',
  styleUrl: './category-gift.component.scss'
})
export class CategoryGiftComponent {
    @Input() public categoryName!: string;
    @Input() public title!: string;
    @Input() public products!: ProductInterface[];
}
