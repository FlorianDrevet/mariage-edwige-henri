import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { AccommodationModel, MyAccommodationModel } from '../models/accommodation.model';

@Injectable({ providedIn: 'root' })
export class AccommodationApi {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = environment['API_URL'] as string;

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

  assignUsers(accommodationId: string, userIds: string[]): Observable<AccommodationModel> {
    return this.http.post<AccommodationModel>(
      `${this.baseUrl}/accommodations/${accommodationId}/assignments`,
      { userIds }
    );
  }

  unassignUser(accommodationId: string, userId: string): Observable<AccommodationModel> {
    return this.http.delete<AccommodationModel>(
      `${this.baseUrl}/accommodations/${accommodationId}/assignments/${userId}`
    );
  }

  getMyAccommodation(): Observable<MyAccommodationModel> {
    return this.http.get<MyAccommodationModel>(`${this.baseUrl}/accommodations/my`);
  }

  respondToAccommodation(response: string): Observable<MyAccommodationModel> {
    return this.http.put<MyAccommodationModel>(
      `${this.baseUrl}/accommodations/my/response`,
      { response }
    );
  }
}
