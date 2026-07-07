import {Inject, Injectable, PLATFORM_ID, TransferState} from '@angular/core';
import {isPlatformBrowser} from '@angular/common';
import {ApplicationInsights} from '@microsoft/applicationinsights-web';
import {APP_INSIGHTS_CONNECTION_STRING_KEY} from './app-insights.state-key';

@Injectable({providedIn: 'root'})
export class AppInsightsService {
  constructor(
    @Inject(PLATFORM_ID) platformId: object,
    transferState: TransferState
  ) {
    if (!isPlatformBrowser(platformId)) {
      return;
    }

    const connectionString = transferState.get(APP_INSIGHTS_CONNECTION_STRING_KEY, null);
    if (!connectionString) {
      return;
    }

    const appInsights = new ApplicationInsights({
      config: {
        connectionString,
        enableAutoRouteTracking: true,
        enableCorsCorrelation: true,
      },
    });

    appInsights.loadAppInsights();
    appInsights.trackPageView();
  }
}
