import {AfterViewInit, Component} from '@angular/core';

@Component({
  selector: 'app-explanation-modal',
  templateUrl: './explanation-modal.component.html',
  styleUrl: './explanation-modal.component.scss'
})
export class ExplanationModalComponent implements AfterViewInit {
  showModal = true;

  closeModal() {
    this.showModal = false;
    document.body.classList.remove('modal-open');
  }

  ngAfterViewInit(): void {
    document.body.classList.add('modal-open');
  }
}
