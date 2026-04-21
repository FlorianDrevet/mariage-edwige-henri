import {RenderMode, ServerRoute} from '@angular/ssr';

export const serverRoutes: ServerRoute[] = [
  // Static pages — rendered server-side (SSG prerender yields 0 routes with NgModules, use Server)
  {
    path: 'accueil',
    renderMode: RenderMode.Server,
  },
  {
    path: 'mariage',
    renderMode: RenderMode.Server,
  },
  {
    path: 'mariage/ceremonie-religieuse',
    renderMode: RenderMode.Server,
  },
  {
    path: 'mariage/vin-honneur',
    renderMode: RenderMode.Server,
  },
  {
    path: 'mariage/reception',
    renderMode: RenderMode.Server,
  },
  {
    path: 'mariage/photos',
    renderMode: RenderMode.Server,
  },
  {
    path: 'staff-officiel',
    renderMode: RenderMode.Server,
  },
  {
    path: 'contact',
    renderMode: RenderMode.Server,
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
    path: 'hebergements',
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
