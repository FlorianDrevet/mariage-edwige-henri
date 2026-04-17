import { NgModule } from '@angular/core';
import { ServerModule } from '@angular/platform-server';
import { provideServerRendering, withRoutes } from '@angular/ssr';
import { serverRoutes } from './app.routes.server';

import { AppModule } from './app.module';
import { AppComponent } from './app.component';
import {SERVER_API_URL} from "./shared/services/server-api-url.token";

@NgModule({
  imports: [
    AppModule,
    ServerModule,
  ],
  providers: [
    provideServerRendering(withRoutes(serverRoutes)),
    {
      provide: SERVER_API_URL,
      // Aspire injecte l'URL via process.env (service discovery).
      // Fallback sur l'URL locale standard si Aspire n'est pas actif.
      useFactory: () =>
        process.env['services__api__https__0'] ??
        process.env['services__api__http__0'] ??
        'http://localhost:5143'
    }
  ],
  bootstrap: [AppComponent],
})
export class AppServerModule {}
