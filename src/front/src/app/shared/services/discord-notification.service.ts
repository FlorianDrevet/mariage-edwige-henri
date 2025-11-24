import { Injectable } from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {environment} from "../../../environments/environment";

@Injectable({
  providedIn: 'root'
})
export class DiscordNotificationService {

  constructor(private http: HttpClient) { }

  sendNotification(message: string) {
    const webhookUrl = environment['discord_webhook'];

    const payload = {
      content: message
    };

    return this.http.post(webhookUrl, payload);
  }
}
