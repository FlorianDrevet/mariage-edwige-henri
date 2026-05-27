import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import {
  AccommodationModel,
  AvailableAccommodationModel,
  BookAccommodationRequest,
  BookingModel,
  MyBookingsModel
} from '../models/accommodation.model';

@Injectable({ providedIn: 'root' })
export class AccommodationApi {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = environment['API_URL'] as string;

  // === Admin ===

  getAccommodations(): Observable<AccommodationModel[]> {
    return this.http.get<AccommodationModel[]>(`${this.baseUrl}/accommodations`);
  }

  createAccommodation(formData: FormData): Observable<AccommodationModel> {
    return this.http.post<AccommodationModel>(`${this.baseUrl}/accommodations`, formData);
  }

  updateAccommodation(id: string, formData: FormData): Observable<AccommodationModel> {
    return this.http.put<AccommodationModel>(`${this.baseUrl}/accommodations/${id}`, formData);
  }

  deleteAccommodation(id: string): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/accommodations/${id}`);
  }

  // === Guest ===

  getAvailableAccommodations(): Observable<AvailableAccommodationModel[]> {
    return this.http.get<AvailableAccommodationModel[]>(`${this.baseUrl}/accommodations/available`);
  }

  bookAccommodation(accommodationId: string, request: BookAccommodationRequest): Observable<AccommodationModel> {
    return this.http.post<AccommodationModel>(
      `${this.baseUrl}/accommodations/${accommodationId}/bookings`,
      request
    );
  }

  confirmBooking(accommodationId: string, bookingId: string): Observable<AccommodationModel> {
    return this.http.put<AccommodationModel>(
      `${this.baseUrl}/accommodations/${accommodationId}/bookings/${bookingId}/confirm`,
      {}
    );
  }

  cancelBooking(accommodationId: string, bookingId: string): Observable<void> {
    return this.http.delete<void>(
      `${this.baseUrl}/accommodations/${accommodationId}/bookings/${bookingId}`
    );
  }

  getMyBookings(): Observable<MyBookingsModel> {
    return this.http.get<MyBookingsModel>(`${this.baseUrl}/accommodations/my-bookings`);
  }
}
