import {AfterViewInit, Component, ElementRef, OnInit, ViewChild} from '@angular/core';
import {ProductInterface} from "../../shared/interfaces/product.interface";
import {ActivatedRoute} from "@angular/router";
import {GiftApi} from "../../shared/apis/gift.api";
import {AuthService} from "../../shared/services/auth.service";
import {DiscordNotificationService} from "../../shared/services/discord-notification.service";
import {CategoryEnum} from "../../shared/enums/category.enum";

@Component({
  standalone: false,
  selector: 'app-gift',
  templateUrl: './gift.component.html',
  styleUrl: './gift.component.scss'
})
export class GiftComponent implements OnInit {
  gift: ProductInterface | null = null;
  value: number = 0;
  choosingAmount: boolean = true;
  clickedLydia: boolean = false;


  constructor(private route: ActivatedRoute,
              private discord: DiscordNotificationService,
              private giftApi: GiftApi,
              protected authService: AuthService) { }

  ngOnInit(): void {
    this.getProduct()
  }

  onClickedLydia() {
    if (this.authService.isAuthenticated$) {
      this.discord.sendNotification(this.authService.Name + " clicked on Lydia button for " + this.gift?.name + " with " + this.value + "€").subscribe();
    }
    else {
      this.discord.sendNotification("Someone clicked on Lydia button for " + this.gift?.name + " with " + this.value + "€").subscribe();
    }
    this.clickedLydia = true;
  }

  getProduct() {
    this.route.paramMap.subscribe(params => {
      if (params.get('id') !== null)
      {
        this.giftApi.getProductById(params.get('id')!).then(response => {
          this.gift = response;
        })
      }
    })
  }

  public onUpClick() {
    if (this.value >= 0) {
      this.value += 1;
    }
  }

  public onDownClick() {
    if (this.value > 0)
      this.value -= 1;
  }

  public onParticipateClick() {
    if (this.gift !== null)
    {
      if (this.value <= this.gift.price - this.gift.participation && this.value > 0)
      {
        this.choosingAmount = false;
      }
    }
  }

  onCloseModal() {
    this.getProduct()
    this.choosingAmount = true;
    this.value = 0;
  }

  protected readonly CategoryEnum = CategoryEnum;
}
