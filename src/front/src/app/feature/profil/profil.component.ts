import { ChangeDetectionStrategy, Component, computed, effect, inject, signal } from '@angular/core';
import { rxResource } from '@angular/core/rxjs-interop';
import { FormBuilder, Validators } from '@angular/forms';
import { cilEnvelopeClosed } from '@coreui/icons';
import { UsersApi } from '../../shared/apis/users.api';
import { DiscordNotificationService } from '../../shared/services/discord-notification.service';
import { AccommodationApi } from '../../shared/apis/accommodation.api';
import { catchError, of } from 'rxjs';
import { BookingModel } from '../../shared/models/accommodation.model';

@Component({
  standalone: false,
  selector: 'app-profil',
  templateUrl: './profil.component.html',
  styleUrl: './profil.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ProfilComponent {
  private readonly usersApi = inject(UsersApi);
  private readonly discord = inject(DiscordNotificationService);
  private readonly accommodationApi = inject(AccommodationApi);
  private readonly fb = inject(FormBuilder);

  readonly icon = { cilEnvelopeClosed };

  /** Signal-based async resource — auto-loads on creation, exposes value/error/status. */
  readonly profilResource = rxResource({
    stream: () => this.usersApi.getUserProfils(),
  });

  readonly bookingsResource = rxResource({
    stream: () => this.accommodationApi.getMyBookings().pipe(
      catchError(() => of({ bookings: [] }))
    ),
  });

  readonly profil = computed(() => this.profilResource.value() ?? null);
  readonly isLoading = computed(() => this.profilResource.isLoading());
  readonly myBookings = computed(() => this.bookingsResource.value()?.bookings ?? []);

  readonly profilForm = this.fb.nonNullable.group({
    email: ['', Validators.required],
  });

  constructor() {
    // When data lands, sync the form + send a one-shot Discord notification.
    let notified = false;
    effect(() => {
      const user = this.profil();
      if (!user) return;
      this.profilForm.setValue({ email: user.email ?? '' });
      if (!notified) {
        notified = true;
        this.discord.sendNotification(`${user.username} clicked on profil page`).subscribe();
      }
    });
  }

  onUpdateClick(): void {
    const current = this.profil();
    if (!current) return;

    const newEmail = this.profilForm.controls.email.value || null;
    if (newEmail === current.email) return;

    this.usersApi.changeEmail(newEmail).subscribe(updated => {
      this.profilResource.set(updated);
    });
  }

  onCancelBooking(booking: BookingModel): void {
    this.accommodationApi.cancelBooking(booking.accommodationId, booking.id)
      .subscribe(() => this.bookingsResource.reload());
  }

  getBookingStatusLabel(status: string): string {
    switch (status) {
      case 'Confirmed': return 'Confirmé';
      case 'Cancelled': return 'Annulé';
      default: return 'En attente de paiement';
    }
  }
}

