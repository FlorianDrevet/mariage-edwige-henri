import {NgModule} from '@angular/core';
import {CommonModule, NgOptimizedImage} from '@angular/common';
import {ScreenService} from "./services/screen.service";
import {ButtonComponent} from './components/button/button.component';
import {BrowserAnimationsModule} from "@angular/platform-browser/animations";
import {AuthService} from "./services/auth.service";
import {AxiosService} from "./services/axios.service";
import {CookieService} from "ngx-cookie-service";
import {PricePipe} from './pipe/price.pipe';
import {PercentagePipe} from './pipe/percentage.pipe';
import {RouterLink} from "@angular/router";
import {InputComponent} from './components/input/input.component';
import {IconModule, IconSetService} from "@coreui/icons-angular";
import {FormsModule, ReactiveFormsModule} from "@angular/forms";
import {TitleWeddingComponent} from './components/title-wedding/title-wedding.component';
import { NoCommaInputDirective } from './directives/no-comma-input.directive';
import { MaxValueInputDirective } from './directives/max-value-input.directive';
import { NumberInputDirective } from './directives/number-input.directive';
import { GiftCategoryPipe } from './pipe/gift-category.pipe';
import { PhotoListComponent } from './components/photo-list/photo-list.component';
import { SkeletonPhotoCardComponent } from './components/skeleton-photo-card/skeleton-photo-card.component';

@NgModule({
  declarations: [
    ButtonComponent,
    PricePipe,
    PercentagePipe,
    InputComponent,
    TitleWeddingComponent,
    NoCommaInputDirective,
    MaxValueInputDirective,
    NumberInputDirective,
    GiftCategoryPipe,
    PhotoListComponent,
    SkeletonPhotoCardComponent,
  ],
  imports: [
    CommonModule,
    NgOptimizedImage,
    BrowserAnimationsModule,
    RouterLink,
    IconModule,
    FormsModule,
    ReactiveFormsModule,
  ],
  providers: [
    ScreenService,
    AuthService,
    AxiosService,
    CookieService,
    IconSetService,
  ],
  exports: [
    ButtonComponent,
    PricePipe,
    InputComponent,
    TitleWeddingComponent,
    PercentagePipe,
    NoCommaInputDirective,
    NumberInputDirective,
    GiftCategoryPipe,
    PhotoListComponent,
  ]
})
export class SharedModule { }
