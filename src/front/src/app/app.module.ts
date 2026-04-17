import {NgModule} from '@angular/core';
import {BrowserModule, provideClientHydration, withIncrementalHydration} from '@angular/platform-browser';

import {AppRoutingModule} from './app-routing.module';
import {AppComponent} from './app.component';
import {CoreModule} from "./core/core.module";
import {provideAnimationsAsync} from '@angular/platform-browser/animations/async';
import {NgOptimizedImage} from "@angular/common";
import {SharedModule} from "./shared/shared.module";
import {AcceuilModule} from "./feature/accueil/acceuil.module";
import {BrowserAnimationsModule} from "@angular/platform-browser/animations";
import {
  AvatarComponent, ButtonCloseDirective, ButtonDirective,
  ModalBodyComponent,
  ModalComponent,
  ModalFooterComponent,
  ModalHeaderComponent,
  ModalTitleDirective,
  ModalToggleDirective
} from "@coreui/angular";
import {WeddingListModule} from "./feature/wedding-list/wedding-list.module";
import {UsersComponent} from './feature/users/users.component';
import {ModalAddUserComponent} from './feature/users/components/modal-add-user/modal-add-user.component';
import {FormsModule, ReactiveFormsModule} from "@angular/forms";
import {UsersModule} from "./feature/users/users.module";
import {ProfilModule} from "./feature/profil/profil.module";
import {GiftModule} from "./feature/gift/gift.module";
import { MariageComponent } from './feature/mariage/mariage.component';
import { CeremonieComponent } from './feature/mariage/timeline/ceremonie/ceremonie.component';
import { VinHonneurComponent } from './feature/mariage/timeline/vin-honneur/vin-honneur.component';
import { ReceptionComponent } from './feature/mariage/timeline/reception/reception.component';
import { NeedToBeConnectedComponent } from './shared/components/need-to-be-connected/need-to-be-connected.component';
import { PhotosComponent } from './feature/mariage/timeline/photos/photos.component';
import { MariesComponent } from './feature/maries/maries.component';
import { ContactComponent } from './feature/contact/contact.component';
import {PhotosMariageComponent} from "./feature/photos/photos.component";
import { TimelineMariageComponent } from './feature/mariage/timeline-mariage/timeline-mariage.component';
import { TimelineMariageMobileComponent } from './feature/mariage/timeline-mariage-mobile/timeline-mariage-mobile.component';
import { ExplanationModalComponent } from './feature/mariage/explanation-modal/explanation-modal.component';
import {provideHttpClient, withFetch} from "@angular/common/http";

@NgModule({
  declarations: [
    AppComponent,
    UsersComponent,
    ModalAddUserComponent,
    MariageComponent,
    CeremonieComponent,
    VinHonneurComponent,
    ReceptionComponent,
    NeedToBeConnectedComponent,
    PhotosComponent,
    MariesComponent,
    ContactComponent,
    PhotosMariageComponent,
    TimelineMariageComponent,
    TimelineMariageMobileComponent,
    ExplanationModalComponent
  ],
  imports: [
    AcceuilModule,
    AppRoutingModule,
    AvatarComponent,
    BrowserAnimationsModule,
    BrowserModule,
    CoreModule,
    FormsModule,
    ModalBodyComponent,
    ModalComponent,
    ModalFooterComponent,
    ModalHeaderComponent,
    ModalTitleDirective,
    ModalToggleDirective,
    NgOptimizedImage,
    ProfilModule,
    ReactiveFormsModule,
    SharedModule,
    UsersModule,
    WeddingListModule,
    GiftModule,
    ButtonCloseDirective,
    ButtonDirective,
  ],
  providers: [
    provideClientHydration(withIncrementalHydration()),
    provideAnimationsAsync(),
    provideHttpClient(withFetch()),
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
