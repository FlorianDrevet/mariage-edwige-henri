import {Component, Input} from '@angular/core';
import {ProductInterface} from "../../../../shared/interfaces/product.interface";
import { CategoryEnum } from '../../../../shared/enums/category.enum';

@Component({
  standalone: false,
  selector: 'app-category-gift',
  templateUrl: './category-gift.component.html',
  styleUrl: './category-gift.component.scss'
})
export class CategoryGiftComponent {
    @Input() public category!: CategoryEnum;
    @Input() public title!: string;
    @Input() public products!: ProductInterface[];

    protected readonly CategoryEnum = CategoryEnum;
}
