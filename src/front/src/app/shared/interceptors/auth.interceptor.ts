import { HttpInterceptorFn } from '@angular/common/http';
import { inject, PLATFORM_ID } from '@angular/core';
import { isPlatformBrowser } from '@angular/common';
import { JwtHelperService } from '@auth0/angular-jwt';
import { AuthService } from '../services/auth.service';
import { environment } from '../../../environments/environment';

/**
 * Adds `Authorization: Bearer <jwt>` to outbound requests targeting the API
 * when a non-expired token is available in the cookie store.
 *
 * No-op on the server (no cookies, no token).
 */
export const authInterceptor: HttpInterceptorFn = (req, next) => {
  const platformId = inject(PLATFORM_ID);
  if (!isPlatformBrowser(platformId)) {
    return next(req);
  }

  const apiUrl = environment['API_URL'] as string;
  if (!req.url.startsWith(apiUrl) && !req.url.startsWith('/')) {
    return next(req);
  }

  const auth = inject(AuthService);
  const jwtHelper = inject(JwtHelperService);
  const token = auth.getAuthToken();

  if (!token || jwtHelper.isTokenExpired(token)) {
    return next(req);
  }

  return next(
    req.clone({ setHeaders: { Authorization: `Bearer ${token}` } })
  );
};
