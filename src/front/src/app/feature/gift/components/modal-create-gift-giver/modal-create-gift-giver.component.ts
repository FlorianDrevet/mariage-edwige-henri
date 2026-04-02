import {Component, EventEmitter, Input, Output} from '@angular/core';
import {cilGift, cilMoney} from "@coreui/icons";
import {FormBuilder, FormGroup, Validators} from "@angular/forms";
import {GiftApi} from "../../../../shared/apis/gift.api";
import {Router} from "@angular/router";
import { Location } from '@angular/common';

@Component({
  standalone: false,
  selector: 'app-modal-create-gift-giver',
  templateUrl: './modal-create-gift-giver.component.html',
  styleUrl: './modal-create-gift-giver.component.scss'
})
export class ModalCreateGiftGiverComponent {
  icon = {cilGift, cilMoney};
  createGiftGiverForm: FormGroup;

  @Input() clickedLydia: boolean = false;
  @Input() amount: number = 0;
  @Input() id: string = '';
  @Input() disabled: boolean = false;
  @Output() close: EventEmitter<boolean> = new EventEmitter();

  constructor(private fb: FormBuilder,
              private giftApi: GiftApi,
              private router: Router,
              private location: Location) {
    this.createGiftGiverForm = this.fb.group({
      firstName: ['', Validators.required],
      lastName: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
    });
  }
  onCreateGiftGiverClick() {
    if (this.createGiftGiverForm.invalid) {
      return;
    }

    let giftGiver = this.createGiftGiverForm.value
    giftGiver.amount = this.amount
    this.giftApi.postGiftGiver(this.id, giftGiver).then(res => {
      this.close.emit(true)
    })
  }
}
