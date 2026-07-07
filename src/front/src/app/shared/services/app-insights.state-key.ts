import {makeStateKey} from '@angular/core';

/**
 * Transfers the App Insights connection string from the SSR server
 * (process.env, set at runtime by ACA) to the browser via hydration state,
 * since the browser bundle is built once in CI without access to that env var.
 */
export const APP_INSIGHTS_CONNECTION_STRING_KEY = makeStateKey<string | null>('appInsightsConnectionString');
