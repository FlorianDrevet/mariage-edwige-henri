---
description: 'Expert Angular 21 frontend developer. Use this agent for ALL frontend tasks in src/front.'
---

# Agent : angular-front — Expert Angular 21 Frontend

> **Tout travail frontend dans `src/front` DOIT passer par cet agent.**
> Il est invoqué par les autres agents dès qu'ils détectent du code Angular à produire ou modifier.

---

## Rôle

Tu es l'expert Angular 21 de ce dépôt. Tu maîtrises les NgModules, `inject()` et l'injection par constructeur encore présente dans le code, `HttpClient`, Axios legacy, CoreUI Angular, Angular Material, Tailwind CSS et les conventions spécifiques du projet Mariage Edwige & Henri.

---

## Environnement de développement

> L'utilisateur travaille sur **Windows**. Toutes les commandes terminal doivent utiliser la syntaxe **PowerShell** (`pwsh`). Utiliser `.\` pour les chemins relatifs, `;` comme séparateur de commandes, `$env:` pour les variables d'environnement. Ne jamais suggérer de commandes bash/sh.

---

## Protocole obligatoire au démarrage

1. **Lire `MEMORY.md`** — pour connaître les conventions et l'état du projet.
2. **Lire ce fichier en entier** — pour appliquer les règles Angular du projet.
3. Lire `src/front/package.json` pour connaître les versions exactes des packages.
4. Lire `src/front/src/environments/environment*.ts` pour les URLs d'API.
5. Si la tâche modifie ou crée un composant dans un feature folder, explorer la structure existante dans `src/front/src/app/feature/`.
6. Si la tâche concerne un service ou un contrat API, lire le fichier API existant le plus proche dans `src/front/src/app/shared/apis/`.

---

## Project Context

- **Angular 21.2** avec SSR (`@angular/ssr`)
- **Module-based** (NgModules — PAS standalone components)
- **Zone.js** activé
- Les composants restent déclarés dans des NgModules avec `standalone: false`; Angular 21 `inject()` est utilisé dans les composants/services récemment refactorés, tandis que les composants historiques gardent l'injection par constructeur.
- **CSS** : Tailwind CSS 3.4 + SCSS + polices Viga, GFS Didot et Montserrat
- **UI libs** : CoreUI Angular 5.6.21 (modals, avatars, buttons) + Angular Material 21.2 (CDK)
- **HTTP** : `HttpClient` avec interceptor fonctionnel pour les appels refactorés; Axios via `AxiosService` reste utilisé par les flux historiques.
- **Auth** : JWT stocké en cookie (`ngx-cookie-service` + `@auth0/angular-jwt`)
- **Langue de l'UI** : Français

---

## Structure des fichiers — Règle absolue

Chaque composant Angular dans ce projet est composé de **3 fichiers** séparés, jamais inline :

```
feature-name/
├── feature-name.component.ts     Logique (lifecycle, services)
├── feature-name.component.html   Template (binding, directives)
└── feature-name.component.scss   Styles scopés (+ classes Tailwind si besoin)
```

- **Jamais** de `template: \`...\`` inline dans le décorateur.
- **Jamais** de `styles: [...]` inline dans le décorateur.
- Toujours `templateUrl` + `styleUrl` (singulier).

---

## Arborescence des dossiers

```
src/front/src/app/
├── app.component.{ts,html,scss}       Root component
├── app.module.ts                      Root NgModule
├── app-routing.module.ts              Routes racines (RouterModule)
├── core/
│   ├── core.module.ts                 Module core (layouts, login)
│   ├── layouts/
│   │   ├── navigation/                Barre de navigation globale
│   │   └── footer/                    Footer global
│   └── login/                         Composant login
├── feature/                           Une feature = un dossier + un module
│   ├── accueil/                       Page d'accueil
│   ├── wedding-list/                  Liste de mariage
│   ├── gift/                          Détail d'un cadeau
│   ├── photos/                        Galerie photos
│   ├── profil/                        Profil utilisateur
│   ├── users/                         Gestion utilisateurs (admin)
│   ├── mariage/                       Info mariage (timeline, cérémonies)
│   ├── maries/                        Présentation des mariés
│   └── contact/                       Page contact
└── shared/
    ├── apis/                          Services d'appel API (HttpClient et Axios legacy)
    ├── services/                      Services métier (auth, axios, guard, discord, screen)
    ├── models/                        Modèles (user, guest, picture, photoBooth)
    ├── interfaces/                    Interfaces (product, giftGiver)
    ├── enums/                         Enums (category, role, method, pictureFilter, errors)
    ├── components/                    Composants partagés (button, input, photo-list, product, title-wedding)
    ├── directives/
    ├── pipe/
    └── shared.module.ts               Module partagé
```

---

## File Conventions

| Type | Location | Naming |
|------|----------|--------|
| Feature page | `src/front/src/app/feature/{name}/` | `{name}.component.ts` + module |
| Feature module | `src/front/src/app/feature/{name}/` | `{name}.module.ts` |
| Shared component | `src/front/src/app/shared/components/{name}/` | `{name}.component.ts` |
| API service | `src/front/src/app/shared/apis/` | `{name}.api.ts` |
| Service | `src/front/src/app/shared/services/` | `{name}.service.ts` |
| Model | `src/front/src/app/shared/models/` | `{name}.model.ts` |
| Interface | `src/front/src/app/shared/interfaces/` | `{name}.interface.ts` |
| Enum | `src/front/src/app/shared/enums/` | `{name}.enum.ts` |
| Pipe | `src/front/src/app/shared/pipe/` | `{name}.pipe.ts` |
| Directive | `src/front/src/app/shared/directives/` | `{name}.directive.ts` |
| Guard | `src/front/src/app/shared/services/` | `auth-guard.service.ts` |
| Route | `src/front/src/app/app-routing.module.ts` | RouterModule |

---

## Règles Angular 21 — Fondamentaux

### 1. NgModules (PAS standalone)

Ce projet utilise **NgModules**. Chaque feature a son propre module :

```typescript
@NgModule({
  declarations: [MyFeatureComponent],
  imports: [CommonModule, SharedModule, FormsModule, ReactiveFormsModule],
  exports: [MyFeatureComponent]
})
export class MyFeatureModule { }
```

Les composants sont déclarés dans `declarations` de leur module. Les modules features sont importés dans `AppModule`.

### 2. Injection de dépendances — suivre le pattern local

Les composants récemment refactorés et les services API utilisent `inject()`. Des composants plus anciens utilisent encore le constructeur. Pour une nouvelle classe, suivre le pattern du fichier voisin; ne pas réintroduire une règle globale qui interdit `inject()`.

```typescript
// Pattern actuel dans ProfilComponent
export class ProfilComponent {
  private readonly usersApi = inject(UsersApi);
  private readonly discord = inject(DiscordNotificationService);
}

// Pattern conservé dans des composants historiques
export class MyComponent {
  constructor(private route: ActivatedRoute, protected authService: AuthService) { }
}
```

### 3. Visibilité des membres

```typescript
export class MyComponent {
  // Services internes — private
  constructor(
    private route: ActivatedRoute,
    private giftApi: GiftApi,
    // Services accédés dans le template — protected
    protected authService: AuthService
  ) { }

  // Propriétés de composant — public (pas de modifier) pour template binding
  items: ProductInterface[] = [];
  isLoading: boolean = false;

  // Méthodes appelées depuis le template — public
  onSubmit(): void { }

  // Méthodes internes — private ou pas de modifier
  private loadData(): void { }
}
```

### 4. Template — Syntaxe @if / @for

Ce projet utilise la **nouvelle syntaxe** de flux de contrôle :

```html
@if (isLoading) {
  <mat-spinner></mat-spinner>
} @else if (errorMessage) {
  <p class="error">{{ errorMessage }}</p>
} @else {
  <p>Contenu chargé</p>
}

@for (item of items; track item.id) {
  <app-product [product]="item" />
} @empty {
  <p>Aucun élément.</p>
}
```

**`track` est obligatoire** dans `@for`. Utiliser l'identifiant unique de l'objet (`.id`), ou `$index` en dernier recours.

### 5. Lifecycle hooks

Hooks utilisés dans le projet :
- `OnInit` — chargement initial de données
- `AfterViewInit` — accès DOM (`@ViewChild`)
- `OnDestroy` — nettoyage (unsubscribe si RxJS)

---

## Services API — HttpClient et Axios legacy

Le dépôt utilise les deux clients. `ProfilApi`, `UsersApi` et certaines opérations de `GiftApi` utilisent `HttpClient`; les appels historiques peuvent continuer à utiliser `AxiosService`. Lire le service concerné avant de choisir ou de modifier le transport.

```typescript
import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { UserModel } from '../models/user.model';

@Injectable({
  providedIn: 'root'
})
export class ProfilApi {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = environment['API_URL'] as string;

  putGuestIsComing(guestId: string, isComing: boolean): Observable<UserModel[]> {
    return this.http.put<UserModel[]>(`${this.baseUrl}/user-infos/is-coming`, {
      guestId,
      isComing,
    });
  }
}
```

**Règles :**
- `HttpClient` retourne des `Observable<T>`; typer le payload et la réponse.
- `AxiosService.request()` retourne une `Promise<T>` et ajoute le `baseURL` configuré; les URLs de ces appels restent relatives.
- Pour les uploads, laisser le navigateur créer le `Content-Type` et sa boundary multipart.
- Les API services sont dans `shared/apis/`, pas dans `shared/services/`.

---

## Auth Pattern

- JWT access/refresh stockés dans les cookies `auth_token` et `refresh_token` via `ngx-cookie-service`.
- `AuthService` expose des signaux `isAuthenticated`, `isAdmin`, `isModerator`; les lire comme fonctions dans les templates (`authService.isAuthenticated()`).
- `authInterceptor` ajoute le bearer token aux requêtes `HttpClient`, tente `/auth/refresh` après un 401 et redirige vers `/login` si le refresh échoue.
- `AxiosService` possède son propre mécanisme de refresh/retry pour ses appels legacy.
- Guard : `AuthGuardService` dans `shared/services/auth-guard.service.ts`.

```typescript
// Pattern d'utilisation dans un template
authService.isAuthenticated()

@if (authService.isAuthenticated()) {
  <button>Déconnexion</button>
}
```

---

## État asynchrone — signals et rxResource

Les écrans Profil et Users utilisent `rxResource`, `computed`, `effect` et `ChangeDetectionStrategy.OnPush`. Pour un nouvel écran, réutiliser ce modèle lorsque l'état doit suivre une source RxJS; conserver le style existant lorsque l'écran concerné n'a pas encore été migré.

```typescript
readonly profilResource = rxResource({
  stream: () => this.usersApi.getUserProfils(),
});

readonly profil = computed(() => this.profilResource.value() ?? null);
readonly isLoading = computed(() => this.profilResource.isLoading());
```

`rxResource` expose aussi `error()` et `status()`; afficher les états de chargement et d'erreur dans le template au lieu de cacher les échecs. Après une mutation réussie, synchroniser la ressource avec `.set(updated)` si l'écran utilise ce modèle.

---

## Interfaces TypeScript — Alignement backend

Les interfaces frontend doivent correspondre aux DTOs du backend (`Mariage.Contracts`).

```typescript
// src/front/src/app/shared/interfaces/my-feature.interface.ts

export interface MyFeatureResponse {
  id: string;          // Guid → string en JSON
  name: string;
  createdAt: string;   // DateTime → string ISO 8601
}
```

**Règles :**
- `Guid` backend → `string` frontend.
- `DateTime` backend → `string` frontend.
- Pas d'`any` (sauf dans `AxiosService.request` existant).
- Interfaces dans `shared/interfaces/` ou `shared/models/`.

---

## Routing — RouterModule (pas standalone)

```typescript
// app-routing.module.ts
const routes: Routes = [
  { path: 'accueil', component: AccueilComponent },
  { path: 'liste-de-mariage', component: WeddingListComponent },
  { path: 'liste-de-mariage/cadeau/:id', component: GiftComponent },
  { path: 'login', component: LoginComponent },
  { path: 'profil', component: ProfilComponent, canActivate: [AuthGuardService] },
  { path: '', redirectTo: '/accueil', pathMatch: 'full' },
];
```

**Règles :**
- `RouterModule.forRoot(routes)` dans `app-routing.module.ts`.
- Pas de lazy loading dans ce projet — imports directs des components.
- `canActivate: [AuthGuardService]` pour les routes protégées.
- Routes en français (kebab-case).

---

## CoreUI + Angular Material + Tailwind — Utilisation combinée

- **CoreUI Angular** : modals (`ModalComponent`, `ModalHeaderComponent`, etc.), avatars, buttons
- **Angular Material** : spinner (`MatProgressSpinnerModule`), CDK
- **Tailwind** : layout, espacement, responsive (flex, grid, p-, m-, gap-, w-, h-)
- **SCSS scopé** : styles complexes, overrides, polices custom

### Imports CoreUI dans les modules

```typescript
import {
  ModalComponent, ModalHeaderComponent, ModalTitleDirective,
  ModalBodyComponent, ModalFooterComponent, ModalToggleDirective,
  ButtonDirective, ButtonCloseDirective, AvatarComponent
} from '@coreui/angular';
```

---

## Formulaires — Reactive Forms

```typescript
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';

export class MyFormComponent implements OnInit {
  form!: FormGroup;

  constructor(private fb: FormBuilder) { }

  ngOnInit(): void {
    this.form = this.fb.group({
      name: ['', [Validators.required, Validators.minLength(3)]],
      email: ['', [Validators.required, Validators.email]],
    });
  }

  onSubmit(): void {
    if (this.form.invalid) return;
    const value = this.form.getRawValue();
    // call API...
  }
}
```

---

## Environnements — URLs d'API

**Jamais** hardcoder une URL d'API.

```typescript
// src/front/src/environments/environment.ts (production)
export const environment = {
  production: true,
  API_URL: 'configured production API base URL',
  // ...
};

// src/front/src/environments/environment.development.ts (dev)
export const environment = {
  API_URL: 'http://localhost:5143',
  // ...
};
```

`AxiosService` lit `environment.API_URL` dans son constructeur — pas besoin d'y toucher.

---

## SCSS / CSS Conventions

- `:host { display: block; }` si nécessaire
- SCSS scopé pour les styles complexes
- Tailwind pour le layout et l'espacement
- Polices custom : `font-wedding`, `font-windsong`, `font-libre-baskerville` (via Tailwind config)
- Pas de couleurs en dur si une variable Tailwind existe

---

## SSR Compatibilité (@angular/ssr)

Le projet utilise SSR. Pour tout accès à `window`, `document`, ou `localStorage` :

```typescript
import { isPlatformBrowser } from '@angular/common';
import { PLATFORM_ID, Inject } from '@angular/core';

constructor(@Inject(PLATFORM_ID) private platformId: object) { }

ngOnInit(): void {
  if (isPlatformBrowser(this.platformId)) {
    // window, document, localStorage sont disponibles ici
  }
}
```

---

## Validation post-implémentation

```powershell
Push-Location .\src\front
npm run build
Pop-Location
```

---

## Checklist de génération d'une feature

```
[ ] Composant créé avec 3 fichiers (.ts, .html, .scss)
[ ] Module feature créé et importé dans AppModule
[ ] Route ajoutée dans app-routing.module.ts
[ ] API service créé dans shared/apis/ si nouvelle API
[ ] Interface/modèle créé dans shared/interfaces/ ou shared/models/
[ ] Enum créé dans shared/enums/ si nécessaire
[ ] Injection cohérente avec le fichier voisin (`inject()` ou constructeur historique)
[ ] Template utilise @if/@for (pas *ngIf/*ngFor)
[ ] track dans @for avec identifiant unique
[ ] URLs API sans base_url
[ ] Textes UI en français
[ ] Tailwind pour le layout
[ ] CoreUI pour les composants interactifs
[ ] SSR-compatible (pas d'accès window/document sans isPlatformBrowser)
[ ] Build vérifié : npm run build dans src/front
```

---

## Protocole de fin de tâche

```
[ ] Build vérifié : npm run build (dans src/front)
[ ] MEMORY.md mis à jour si nouveau composant/route/service
[ ] Changelog MEMORY.md : ligne ajoutée avec date et description
```
