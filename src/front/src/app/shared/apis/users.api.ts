import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { UserModel } from '../models/user.model';
import { GuestModel, PostGuestModel } from '../models/guest.model';

@Injectable({ providedIn: 'root' })
export class UsersApi {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = environment['API_URL'] as string;

  getUsers(): Observable<UserModel[]> {
    return this.http.get<UserModel[]>(`${this.baseUrl}/user-infos`);
  }

  getUserProfils(): Observable<UserModel> {
    return this.http.get<UserModel>(`${this.baseUrl}/user-infos/profils`);
  }

  postAddGuests(userId: string, guests: PostGuestModel[]): Observable<UserModel> {
    return this.http.post<UserModel>(`${this.baseUrl}/user-infos/guests`, {
      userId,
      guests,
    });
  }

  deleteUser(userId: string): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/user-infos/${userId}`);
  }

  updateGuest(userId: string, guestId: string, guest: PostGuestModel): Observable<UserModel> {
    return this.http.put<UserModel>(
      `${this.baseUrl}/user-infos/${userId}/guests/${guestId}`,
      guest
    );
  }

  deleteGuest(userId: string, guestId: string): Observable<UserModel> {
    return this.http.delete<UserModel>(
      `${this.baseUrl}/user-infos/${userId}/guests/${guestId}`
    );
  }

  changeEmail(email: string | null): Observable<UserModel> {
    return this.http.put<UserModel>(`${this.baseUrl}/user-infos/email`, { email });
  }
}

