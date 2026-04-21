import {Component, OnInit} from '@angular/core';
import {AuthService} from "../../shared/services/auth.service";
import {GiftStateService} from "../../shared/services/gift-state.service";

@Component({
  standalone: false,
  selector: 'app-wedding-list',
  templateUrl: './wedding-list.component.html',
  styleUrl: './wedding-list.component.scss'
})
export class WeddingListComponent implements OnInit {

  constructor(protected giftState: GiftStateService,
              protected authService: AuthService) {}

  ngOnInit(): void {
    this.giftState.loadProducts();
    this.giftState.loadCategories();
  }
}
