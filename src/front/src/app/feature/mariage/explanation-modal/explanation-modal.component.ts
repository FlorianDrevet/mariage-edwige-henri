import {AfterViewInit, Component, Inject, PLATFORM_ID} from '@angular/core';
import {isPlatformBrowser} from '@angular/common';

@Component({
  standalone: false,
  selector: 'app-explanation-modal',
  templateUrl: './explanation-modal.component.html',
  styleUrl: './explanation-modal.component.scss'
})
export class ExplanationModalComponent implements AfterViewInit {
  showModal = true;

  constructor(@Inject(PLATFORM_ID) private platformId: Object) {}

  closeModal() {
    this.showModal = false;
    if (isPlatformBrowser(this.platformId)) {
      document.body.classList.remove('modal-open');
    }
  }

  ngAfterViewInit(): void {
    if (isPlatformBrowser(this.platformId)) {
      document.body.classList.add('modal-open');
    }
  }
}
