import { Component, computed, inject, signal } from '@angular/core';
import { rxResource } from '@angular/core/rxjs-interop';
import { FormBuilder, Validators } from '@angular/forms';
import { AccommodationApi } from '../../../../shared/apis/accommodation.api';
import { AuthService } from '../../../../shared/services/auth.service';
import { AvailableAccommodationModel, BookingModel } from '../../../../shared/models/accommodation.model';

@Component({
  standalone: false,
  selector: 'app-hebergement',
  templateUrl: './hebergement.component.html',
  styleUrl: './hebergement.component.scss'
})
export class HebergementComponent {
  private readonly fb = inject(FormBuilder);
  private readonly accommodationApi = inject(AccommodationApi);
  protected readonly authService = inject(AuthService);

  // ── State ──
  readonly step = signal<'list' | 'form' | 'rib' | 'done'>('list');
  readonly selectedAccommodation = signal<AvailableAccommodationModel | null>(null);
  readonly ibanCopied = signal(false);

  // ── Resources ──
  readonly availableResource = rxResource({
    stream: () => this.accommodationApi.getAvailableAccommodations(),
  });
  readonly myBookingsResource = rxResource({
    stream: () => this.accommodationApi.getMyBookings(),
  });

  readonly availableAccommodations = computed(() => this.availableResource.value() ?? []);
  readonly myBookings = computed(() => this.myBookingsResource.value()?.bookings ?? []);

  // ── Form ──
  readonly bookingForm = this.fb.nonNullable.group({
    numberOfPersons: [1, [Validators.required, Validators.min(1)]],
    firstName: ['', Validators.required],
    lastName: ['', Validators.required],
    email: [''],
  });

  readonly totalAmount = computed(() => {
    const acc = this.selectedAccommodation();
    if (!acc) return 0;
    const persons = this.bookingForm.get('numberOfPersons')?.value ?? 1;
    return acc.pricePerPersonPerNight * acc.numberOfNights * persons;
  });

  // ── Handlers ──
  onSelectAccommodation(accommodation: AvailableAccommodationModel): void {
    this.selectedAccommodation.set(accommodation);
    this.bookingForm.reset({ numberOfPersons: 1, firstName: '', lastName: '', email: '' });
    this.step.set('form');
  }

  onBackToList(): void {
    this.step.set('list');
    this.selectedAccommodation.set(null);
  }

  onSubmitBooking(): void {
    if (this.bookingForm.invalid) return;
    const acc = this.selectedAccommodation();
    if (!acc) return;

    const { numberOfPersons, firstName, lastName, email } = this.bookingForm.getRawValue();
    this.accommodationApi.bookAccommodation(acc.id, {
      numberOfPersons,
      firstName,
      lastName,
      email: email || undefined,
    }).subscribe(() => {
      this.step.set('rib');
      this.myBookingsResource.reload();
    });
  }

  onConfirmPayment(): void {
    const booking = this.getLatestPendingBooking();
    if (!booking) return;

    this.accommodationApi.confirmBooking(booking.accommodationId, booking.id)
      .subscribe(() => {
        this.step.set('done');
        this.myBookingsResource.reload();
        this.availableResource.reload();
      });
  }

  onCancelBooking(booking: BookingModel): void {
    this.accommodationApi.cancelBooking(booking.accommodationId, booking.id)
      .subscribe(() => {
        this.myBookingsResource.reload();
        this.availableResource.reload();
      });
  }

  copyIban(): void {
    navigator.clipboard.writeText('FR76 2823 3000 0137 4424 8513 031').then(() => {
      this.ibanCopied.set(true);
      setTimeout(() => this.ibanCopied.set(false), 2000);
    });
  }

  getStatusLabel(status: string): string {
    switch (status) {
      case 'Confirmed': return 'Confirmé';
      case 'Cancelled': return 'Annulé';
      default: return 'En attente de paiement';
    }
  }

  private getLatestPendingBooking(): BookingModel | undefined {
    return this.myBookings().find(b => b.status === 'Pending');
  }
}