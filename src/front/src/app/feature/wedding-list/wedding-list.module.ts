import {NgModule} from '@angular/core';
import {CommonModule, NgOptimizedImage} from '@angular/common';
import {JWT_OPTIONS, JwtHelperService} from "@auth0/angular-jwt";
import {WeddingListComponent} from "./wedding-list.component";
import {ProductComponent} from "../../shared/components/product/product.component";
import {
    ButtonCloseDirective,
    ButtonDirective, FormSelectDirective,
    ModalBodyComponent,
    ModalComponent,
    ModalFooterComponent,
    ModalHeaderComponent,
    ModalTitleDirective,
    ModalToggleDirective,
    ProgressBarComponent,
    ProgressComponent
} from "@coreui/angular";
import {SharedModule} from "../../shared/shared.module";
import {ModelCreateGiftComponent} from './components/model-create-gift/model-create-gift.component';
import {ReactiveFormsModule, FormsModule} from "@angular/forms";
import {RouterLink} from "@angular/router";
import { CategoryGiftComponent } from './components/category-gift/category-gift.component';

@NgModule({
  declarations: [
    WeddingListComponent,
    ProductComponent,
    ModelCreateGiftComponent,
    CategoryGiftComponent
  ],
    imports: [
        CommonModule,
        ProgressBarComponent,
        ProgressComponent,
        SharedModule,
        NgOptimizedImage,
        ModalComponent,
        ModalHeaderComponent,
        ModalToggleDirective,
        ModalBodyComponent,
        ModalFooterComponent,
        ButtonDirective,
        ButtonCloseDirective,
        ModalTitleDirective,
        ReactiveFormsModule,
        FormsModule,
        RouterLink,
        FormSelectDirective,
    ],
  exports: [
    ModelCreateGiftComponent,
    ProductComponent
  ],
  providers: [
    {
      provide: JWT_OPTIONS,
      useValue: JWT_OPTIONS
    },
    JwtHelperService
  ]
})
export class WeddingListModule { }
