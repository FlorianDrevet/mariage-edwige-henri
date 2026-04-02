import {AfterViewInit, Component, Inject, PLATFORM_ID} from '@angular/core';
import {isPlatformBrowser} from '@angular/common';
import {Router} from "@angular/router";

@Component({
  standalone: false,
  selector: 'app-explanation-profil-modal',
  templateUrl: './explanation-profil-modal.component.html',
  styleUrl: './explanation-profil-modal.component.scss'
})
export class ExplanationProfilModalComponent implements AfterViewInit{
  showModal = true;

  constructor(private router: Router, @Inject(PLATFORM_ID) private platformId: Object) {}

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

  public goToProfil() {
    this.closeModal()
    this.router.navigate(['profils']);
  }
}
