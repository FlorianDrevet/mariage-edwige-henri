import {Component, OnInit} from '@angular/core';
import {AuthService} from "../../shared/services/auth.service";
import {GiftStateService} from "../../shared/services/gift-state.service";
import {GiftApi} from "../../shared/apis/gift.api";

@Component({
  standalone: false,
  selector: 'app-wedding-list',
  templateUrl: './wedding-list.component.html',
  styleUrl: './wedding-list.component.scss'
})
export class WeddingListComponent implements OnInit {

  newCategoryName = '';

  constructor(protected giftState: GiftStateService,
              protected authService: AuthService,
              private giftApi: GiftApi) {}

  ngOnInit(): void {
    this.giftState.loadProducts();
    this.giftState.loadCategories();
  }

  onAddCategory(): void {
    const name = this.newCategoryName.trim();
    if (!name) return;
    this.giftApi.createCategory(name).then(() => {
      this.newCategoryName = '';
      this.giftState.refreshCategories();
    });
  }

  onDeleteCategory(id: string): void {
    this.giftApi.deleteCategory(id).then(() => {
      this.giftState.refreshCategories();
    });
  }
}
