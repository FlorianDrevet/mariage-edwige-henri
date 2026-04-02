import {AfterViewInit, Component} from '@angular/core';
import {Router} from "@angular/router";

@Component({
  standalone: false,
  selector: 'app-explanation-profil-modal',
  templateUrl: './explanation-profil-modal.component.html',
  styleUrl: './explanation-profil-modal.component.scss'
})
export class ExplanationProfilModalComponent implements AfterViewInit{
  showModal = true;

  constructor(private router: Router) {
  }

  closeModal() {
    this.showModal = false;
    document.body.classList.remove('modal-open');
  }

  ngAfterViewInit(): void {
    document.body.classList.add('modal-open');
  }

  public goToProfil() {
    this.closeModal()
    this.router.navigate(['profils']);
  }
}
