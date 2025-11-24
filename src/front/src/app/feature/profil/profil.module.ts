import {NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';
import {ProfilComponent} from "./profil.component";
import {SharedModule} from "../../shared/shared.module";
import {ReactiveFormsModule} from "@angular/forms";
import { ToggleButtonComponent } from './components/toggle-button/toggle-button.component';
import {
  ButtonDirective,
  ProgressBarComponent,
  ProgressComponent,
  ToastBodyComponent, ToastComponent, ToasterComponent,
  ToastHeaderComponent
} from "@coreui/angular";
import {ToastMariageComponent} from "./components/toast/toast.component";


@NgModule({
  declarations: [
    ProfilComponent,
    ToggleButtonComponent,
    ToastMariageComponent,
  ],
  imports: [
    CommonModule,
    SharedModule,
    ReactiveFormsModule,
    ProgressBarComponent,
    ProgressComponent,
    ToastBodyComponent,
    ToastHeaderComponent,
    ToastComponent,
    ToasterComponent,
    ButtonDirective,
  ]
})
export class ProfilModule {
}
