import {NgModule} from '@angular/core';
import {CommonModule, NgOptimizedImage} from '@angular/common';
import {FooterComponent} from "./layouts/footer/footer.component";
import {NavigationComponent} from "./layouts/navigation/navigation.component";
import {RouterLink, RouterLinkActive} from "@angular/router";
import {SharedModule} from "../shared/shared.module";
import {LoginComponent} from './login/login.component';
import {ReactiveFormsModule} from "@angular/forms";
import {
  ModalBodyComponent,
  ModalComponent,
  ModalFooterComponent,
  ModalHeaderComponent,
  ModalToggleDirective
} from "@coreui/angular";
import {WeddingListModule} from "../feature/wedding-list/wedding-list.module";
import { HamburgerNavigationComponent } from './layouts/navigation/hamburger-navigation/hamburger-navigation.component';

@NgModule({
  declarations: [
    FooterComponent,
    NavigationComponent,
    LoginComponent,
    HamburgerNavigationComponent,
  ],
  imports: [
    CommonModule,
    RouterLink,
    RouterLinkActive,
    NgOptimizedImage,
    SharedModule,
    ReactiveFormsModule,
    ModalToggleDirective,
    WeddingListModule,
    ModalComponent,
    ModalHeaderComponent,
    ModalBodyComponent,
    ModalFooterComponent
  ],
  exports: [
    FooterComponent,
    NavigationComponent
  ]
})
export class CoreModule { }
