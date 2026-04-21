import {Component, Input, OnInit} from '@angular/core';
import {ProductInterface} from "../../interfaces/product.interface";

@Component({
  standalone: false,
  selector: 'app-product',
  templateUrl: './product.component.html',
  styleUrl: './product.component.scss'
})
export class ProductComponent implements OnInit{
  @Input() product!: ProductInterface;
  ngOnInit(): void {
    console.log(this.product);
  }
}
