import { ChangeDetectionStrategy, Component, inject, input, signal } from '@angular/core';
import { GuestModel } from '../../../../shared/models/guest.model';
import { ProfilApi } from '../../../../shared/apis/profil.api';

@Component({
  standalone: false,
  selector: 'app-toggle-button',
  templateUrl: './toggle-button.component.html',
  styleUrl: './toggle-button.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ToggleButtonComponent {
  private readonly profilApi = inject(ProfilApi);

  readonly guest = input.required<GuestModel>();
  readonly disabled = input<boolean>(false);

  /** Local signal mirrors the server state — initialised from the input on first read. */
  readonly isComing = signal<boolean | null>(null);

  yesClicked(): void {
    this.update(true);
  }

  noClicked(): void {
    this.update(false);
  }

  private update(value: boolean): void {
    const guest = this.guest();
    this.isComing.set(value);
    guest.isComing = value;
    this.profilApi.putGuestIsComing(guest.id, value).subscribe();
  }
}
