import {InjectionToken} from '@angular/core';

/**
 * Injection token for the backend API base URL used in SSR context.
 * Provided only in AppServerModule (app.module.server.ts).
 * On the browser side, environment.API_URL is used instead.
 */
export const SERVER_API_URL = new InjectionToken<string>('SERVER_API_URL');
