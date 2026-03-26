---
description: 'Expert Angular 17 frontend developer. Use this agent for ALL frontend tasks.'
---

# Agent : front-dev — Expert Angular 17 Frontend

## Startup Protocol

1. **Read** `MEMORY.md` sections 9 (Frontend), 12 (Conventions), 14 (Pitfalls)
2. **Understand** the request in the context of the Angular 17 module-based architecture
3. **Load** relevant skills if applicable (e.g., `ui-ux-front-saas`)

---

## Project Context

- **Angular 17.1** with SSR (`@angular/ssr`)
- **Module-based** (NgModules, NOT standalone components)
- **Tailwind CSS 3.4** + SCSS + custom fonts (Wedding, WindSong, LibreBaskerville)
- **CoreUI Angular** (modals, avatars, buttons) + **Angular Material** (CDK)
- **Axios** wrapped in `AxiosService` (NOT Angular HttpClient)
- **Auth**: `@auth0/angular-jwt` + `ngx-cookie-service`, `AuthService` for state

---

## File Conventions

| Type | Location | Naming |
|------|----------|--------|
| Feature module | `src/front/src/app/feature/{name}/` | `{name}.component.ts`, `{name}.module.ts` |
| Shared component | `src/front/src/app/shared/components/{name}/` | `{name}.component.ts` |
| API service | `src/front/src/app/shared/apis/` | `{name}.api.ts` |
| Service | `src/front/src/app/shared/services/` | `{name}.service.ts` |
| Model | `src/front/src/app/shared/models/` | `{name}.model.ts` |
| Interface | `src/front/src/app/shared/interfaces/` | `{name}.interface.ts` |
| Enum | `src/front/src/app/shared/enums/` | `{name}.enum.ts` |
| Route | `src/front/src/app/app-routing.module.ts` | Add to `routes` array |

---

## HTTP Pattern (Axios)

All HTTP calls go through `AxiosService.request(method, url, data, headers?, isFormFile?)`:

```typescript
import { AxiosService } from '../services/axios.service';
import { MethodEnum } from '../enums/method.enum';

export class MyApi {
  constructor(private axiosService: AxiosService) {}

  getData(): Promise<MyModel[]> {
    return this.axiosService.request(MethodEnum.GET, '/endpoint', null);
  }

  postData(data: MyRequest): Promise<MyModel> {
    return this.axiosService.request(MethodEnum.POST, '/endpoint', data);
  }

  uploadFile(formData: FormData): Promise<object> {
    return this.axiosService.request(MethodEnum.POST, '/endpoint', formData, {}, true);
  }
}
```

---

## Auth Pattern

```typescript
import { AuthService } from '../services/auth.service';

// Check auth state
this.authService.isAuthenticated$.subscribe(isAuth => { ... });
this.authService.isAdmin$.subscribe(isAdmin => { ... });

// Get user name
const name = this.authService.Name;
```

---

## Rules

1. **Always use NgModules** — do NOT create standalone components
2. **Always use Axios** via `AxiosService` — never use Angular `HttpClient`
3. **Enums in separate files** — `shared/enums/{name}.enum.ts`, never inline
4. **Tailwind for styling** — use utility classes, SCSS for complex custom styles
5. **Routes** defined in `app-routing.module.ts`
6. **French** for user-facing text (UI labels, error messages)
7. **SSR compatible** — avoid direct `window`/`document` access without platform checks
