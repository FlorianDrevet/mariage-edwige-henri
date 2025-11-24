import {NgModule} from '@angular/core';
import {CommonModule, NgOptimizedImage} from '@angular/common';
import {CarouselNavComponent} from "./carousel-nav/carousel-nav.component";
import {
  CarouselComponent,
  CarouselControlComponent,
  CarouselIndicatorsComponent,
  CarouselInnerComponent,
  CarouselItemComponent
} from "@coreui/angular";
import {RouterLink} from "@angular/router";
import {BrowserAnimationsModule} from "@angular/platform-browser/animations";
import {AccueilComponent} from "./accueil.component";
import { ExplanationProfilModalComponent } from './explanation-profil-modal/explanation-profil-modal.component';
import {SharedModule} from "../../shared/shared.module";


@NgModule({
  declarations: [
    AccueilComponent,
    CarouselNavComponent,
    ExplanationProfilModalComponent,
  ],
  exports: [
    CarouselNavComponent
  ],
  imports: [
    CommonModule,
    CarouselComponent,
    CarouselIndicatorsComponent,
    CarouselInnerComponent,
    CarouselControlComponent,
    CarouselItemComponent,
    RouterLink,
    BrowserAnimationsModule,
    NgOptimizedImage,
    SharedModule
  ]
})
export class AcceuilModule { }
