import { APP_INITIALIZER, NgModule, TransferState } from '@angular/core';
import { ServerModule } from '@angular/platform-server';
import { provideServerRendering, withRoutes } from '@angular/ssr';
import { serverRoutes } from './app.routes.server';

import { AppModule } from './app.module';
import { AppComponent } from './app.component';
import {SERVER_API_URL} from "./shared/services/server-api-url.token";
import {APP_INSIGHTS_CONNECTION_STRING_KEY} from "./shared/services/app-insights.state-key";

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
    },
    {
      // Passe la connection string App Insights (ACA, process.env) au bundle
      // navigateur via l'état d'hydratation, puisque celui-ci est buildé une
      // seule fois en CI sans connaître cette variable d'environnement.
      provide: APP_INITIALIZER,
      useFactory: (transferState: TransferState) => () => {
        transferState.set(
          APP_INSIGHTS_CONNECTION_STRING_KEY,
          process.env['APPLICATIONINSIGHTS_CONNECTION_STRING'] ?? null
        );
      },
      deps: [TransferState],
      multi: true,
    }
  ],
  bootstrap: [AppComponent],
})
export class AppServerModule {}
