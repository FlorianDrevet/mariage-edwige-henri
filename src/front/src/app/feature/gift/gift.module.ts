import { NgModule } from '@angular/core';
import {CommonModule, NgOptimizedImage} from '@angular/common';
import {GiftComponent} from "./gift.component";
import {SharedModule} from "../../shared/shared.module";
import {WeddingListModule} from "../wedding-list/wedding-list.module";
import {FormsModule, ReactiveFormsModule} from "@angular/forms";
import {BrowserModule} from "@angular/platform-browser";
import {HttpClientModule} from "@angular/common/http";
import { ModalCreateGiftGiverComponent } from './components/modal-create-gift-giver/modal-create-gift-giver.component';
import {
  ModalBodyComponent,
  ModalComponent,
  ModalFooterComponent,
  ModalHeaderComponent,
  ModalTitleDirective, ModalToggleDirective
} from "@coreui/angular";



@NgModule({
  declarations: [
    GiftComponent,
    ModalCreateGiftGiverComponent,
  ],
  imports: [
    HttpClientModule,
    BrowserModule,
    CommonModule,
    NgOptimizedImage,
    SharedModule,
    WeddingListModule,
    FormsModule,
    ReactiveFormsModule,
    ModalBodyComponent,
    ModalComponent,
    ModalFooterComponent,
    ModalHeaderComponent,
    ModalTitleDirective,
    ModalToggleDirective,
  ]
})
export class GiftModule { }
