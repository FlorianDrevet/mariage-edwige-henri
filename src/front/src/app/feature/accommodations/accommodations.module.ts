import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import {
  ModalBodyComponent,
  ModalComponent,
  ModalFooterComponent,
  ModalHeaderComponent,
  ModalTitleDirective,
  ModalToggleDirective
} from '@coreui/angular';
import { SharedModule } from '../../shared/shared.module';
import { ReactiveFormsModule } from '@angular/forms';

@NgModule({
  declarations: [],
  exports: [],
  imports: [
    CommonModule,
    ModalToggleDirective,
    SharedModule,
    ModalComponent,
    ModalHeaderComponent,
    ModalBodyComponent,
    ReactiveFormsModule,
    ModalFooterComponent,
    ModalTitleDirective
  ]
})
export class AccommodationsModule {}
