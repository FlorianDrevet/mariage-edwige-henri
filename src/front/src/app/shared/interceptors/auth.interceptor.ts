import { HttpErrorResponse, HttpInterceptorFn } from '@angular/common/http';
import { inject, PLATFORM_ID } from '@angular/core';
import { isPlatformBrowser } from '@angular/common';
import { JwtHelperService } from '@auth0/angular-jwt';
import { AuthService } from '../services/auth.service';
import { environment } from '../../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { catchError, switchMap, throwError } from 'rxjs';

let isRefreshing = false;

/**
 * Adds `Authorization: Bearer <jwt>` to outbound requests targeting the API.
 * If the server responds with 401, attempts a token refresh via /auth/refresh.
 * On refresh failure, redirects to /login.
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

  // Don't add auth header to the refresh endpoint itself
  if (req.url.includes('/auth/refresh') || req.url.includes('/auth/login')) {
    return next(req);
  }

  const auth = inject(AuthService);
  const jwtHelper = inject(JwtHelperService);
  const http = inject(HttpClient);
  const router = inject(Router);
  const token = auth.getAuthToken();

  // Attach token if available (even if expired — server will respond 401 and we'll refresh)
  const clonedReq = token
    ? req.clone({ setHeaders: { Authorization: `Bearer ${token}` } })
    : req;

  return next(clonedReq).pipe(
    catchError((error: HttpErrorResponse) => {
      if (error.status === 401 && !isRefreshing) {
        isRefreshing = true;
        const currentToken = auth.getAuthToken();
        const refreshToken = auth.getRefreshToken();

        if (!currentToken || !refreshToken) {
          isRefreshing = false;
          auth.logout();
          router.navigate(['/login']);
          return throwError(() => error);
        }

        return http.post<{ token: string; refreshToken: string }>(
          `${apiUrl || ''}/auth/refresh`,
          { token: currentToken, refreshToken }
        ).pipe(
          switchMap((response) => {
            isRefreshing = false;
            auth.setTokens(response.token, response.refreshToken);
            // Retry the original request with the new token
            const retryReq = req.clone({
              setHeaders: { Authorization: `Bearer ${response.token}` }
            });
            return next(retryReq);
          }),
          catchError((refreshError) => {
            isRefreshing = false;
            auth.logout();
            router.navigate(['/login']);
            return throwError(() => refreshError);
          })
        );
      }
      return throwError(() => error);
    })
  );
};

