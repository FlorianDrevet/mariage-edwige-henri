import {RenderMode, ServerRoute} from '@angular/ssr';

export const serverRoutes: ServerRoute[] = [
  // Static pages — prerender at build time (SSG)
  {
    path: 'accueil',
    renderMode: RenderMode.Prerender,
  },
  {
    path: 'mariage',
    renderMode: RenderMode.Prerender,
  },
  {
    path: 'mariage/ceremonie-religieuse',
    renderMode: RenderMode.Prerender,
  },
  {
    path: 'mariage/vin-honneur',
    renderMode: RenderMode.Prerender,
  },
  {
    path: 'mariage/reception',
    renderMode: RenderMode.Prerender,
  },
  {
    path: 'mariage/photos',
    renderMode: RenderMode.Prerender,
  },
  {
    path: 'staff-officiel',
    renderMode: RenderMode.Prerender,
  },
  {
    path: 'contact',
    renderMode: RenderMode.Prerender,
  },

  // Auth-required pages — client-side rendering (no SSR needed)
  {
    path: 'login',
    renderMode: RenderMode.Client,
  },
  {
    path: 'profils',
    renderMode: RenderMode.Client,
  },
  {
    path: 'utilisateurs',
    renderMode: RenderMode.Client,
  },
  {
    path: 'photos',
    renderMode: RenderMode.Client,
  },

  // Dynamic pages — server-side rendering
  {
    path: 'liste-de-mariage',
    renderMode: RenderMode.Server,
  },
  {
    path: 'liste-de-mariage/cadeau/:id',
    renderMode: RenderMode.Server,
  },

  // Catch-all — SSR
  {
    path: '**',
    renderMode: RenderMode.Server,
  },
];
