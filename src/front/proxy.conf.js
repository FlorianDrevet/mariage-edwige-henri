/**
 * Angular dev-server proxy configuration for Aspire integration.
 *
 * When running via Aspire, the API endpoint is injected as environment variables:
 *   services__api__https__0  (HTTPS — preferred)
 *   services__api__http__0   (HTTP — fallback)
 *
 * When running standalone (ng serve without Aspire), falls back to localhost:5143.
 *
 * All API routes are forwarded to the backend.
 * The Angular dev-server handles the remaining routes (Angular SPA).
 */

const httpsTarget = process.env['services__api__https__0'];
const httpTarget  = process.env['services__api__http__0'];
const target      = httpsTarget || httpTarget || 'http://localhost:5143';
const secure      = !!httpsTarget; // only verify TLS for HTTPS targets

const apiRoutes = [
  '/auth',
  '/wedding-list',
  '/pictures',
  '/pictures-photo-booth',
  '/pictures-photograph',
  '/user-infos',
  '/healthz',
];

module.exports = [
  {
    context: apiRoutes,
    target,
    secure,
    changeOrigin: true,
  },
];
