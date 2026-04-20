import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { UserModel } from '../models/user.model';

@Injectable({ providedIn: 'root' })
export class ProfilApi {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = environment['API_URL'] as string;

  putGuestIsComing(guestId: string, isComing: boolean): Observable<UserModel[]> {
    return this.http.put<UserModel[]>(`${this.baseUrl}/user-infos/is-coming`, {
      guestId,
      isComing,
    });
  }
}

