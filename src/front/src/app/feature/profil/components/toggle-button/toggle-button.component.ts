import { ChangeDetectionStrategy, Component, computed, inject, input, output, signal } from '@angular/core';
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

  readonly guest = input<GuestModel | null>(null);
  readonly disabled = input<boolean>(false);
  readonly value = input<boolean | null>(null);
  readonly yesLabel = input<string>('Oui');
  readonly noLabel = input<string>('Non');
  readonly changed = output<boolean>();

  /** Unique ID so multiple toggles on the same page don't share radio groups. */
  readonly _id = Math.random().toString(36).substring(2, 9);

  readonly checkedYes = computed(() => {
    const g = this.guest();
    if (g) return g.isComing;
    return this.value() === true;
  });

  readonly checkedNo = computed(() => {
    const g = this.guest();
    if (g) return !g.isComing;
    return this.value() === false;
  });

  yesClicked(): void {
    this.update(true);
  }

  noClicked(): void {
    this.update(false);
  }

  private update(value: boolean): void {
    const guest = this.guest();
    if (guest) {
      guest.isComing = value;
      this.profilApi.putGuestIsComing(guest.id, value).subscribe();
    } else {
      this.changed.emit(value);
    }
  }
}
