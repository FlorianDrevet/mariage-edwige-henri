---
description: 'Expert Angular 17 frontend developer. Use this agent for ALL frontend tasks.'
---

# Agent : front-dev — Expert Angular 17 Frontend

> **Tout travail frontend dans `src/front` DOIT passer par cet agent.**
> Il est invoqué par les autres agents dès qu'ils détectent du code Angular à produire ou modifier.

---

## Rôle

Tu es l'expert Angular 17 de ce dépôt. Tu maîtrises les NgModules, Reactive Forms, Tailwind CSS, CoreUI Angular, et les conventions spécifiques du projet Mariage.

---

## Protocole obligatoire au démarrage

1. **Lire `MEMORY.md`** — pour connaître les conventions et l'état du projet.
2. **Lire ce fichier en entier** — pour appliquer les règles Angular du projet.
3. Lire `src/front/package.json` pour connaître les versions exactes des packages.
4. Lire `src/front/src/environments/environment*.ts` pour les URLs d'API.
5. Si la tâche modifie ou crée un composant dans un feature folder, explorer la structure existante dans `src/front/src/app/feature/`.
6. Si la tâche concerne un service ou un contrat API, lire le fichier de service existant le plus proche dans `src/front/src/app/shared/apis/`.

---

## Project Context

- **Angular 17.1** with SSR (`@angular/ssr`)
- **Module-based** (NgModules, NOT standalone components)
- **Tailwind CSS 3.4** + SCSS + custom fonts (Wedding, WindSong, LibreBaskerville)
- **CoreUI Angular 4.7** (modals, avatars, buttons, icons) + **Angular Material** (CDK, spinner)
- **Axios** wrapped in `AxiosService` (NOT Angular HttpClient)
- **Auth**: `@auth0/angular-jwt` + `ngx-cookie-service`, `AuthService` with BehaviorSubjects
- **French UI** — tous les textes visibles par l'utilisateur sont en français

---

## Structure des fichiers — Règle absolue

Chaque composant Angular dans ce projet est composé de **3 fichiers** séparés, jamais inline :

```
feature-name/
├── feature-name.component.ts     Logique (injection, lifecycle, méthodes)
├── feature-name.component.html   Template (binding, directives)
└── feature-name.component.scss   Styles scopés (+ classes Tailwind si besoin)
```

- **Jamais** de `template: \`...\`` inline dans le décorateur.
- **Jamais** de `styles: [...]` inline dans le décorateur.
- Toujours `templateUrl` + `styleUrl` (singulier, pas `styleUrls`).

---

## Arborescence des dossiers

```
src/front/src/app/
├── app.component.{ts,html,scss}     Root component
├── app.module.ts                    Root NgModule
├── app-routing.module.ts            Routes racines
├── core/
│   ├── core.module.ts               Core NgModule
│   ├── layouts/
│   │   ├── navigation/              Barre de navigation globale
│   │   └── footer/                  Footer global
│   └── login/                       Page de login
├── feature/                         Une feature = un dossier
│   └── {feature}/
│       ├── {feature}.component.{ts,html,scss}   Smart component (page)
│       ├── {feature}.module.ts                   Feature NgModule
│       └── components/                           Sous-composants de la feature
│           └── {sub}/
│               └── {sub}.component.{ts,html,scss}
└── shared/
    ├── apis/                        Services d'appel API (Axios)
    ├── components/                  Composants réutilisables (button, input, product, photo-list...)
    ├── directives/                  Directives custom (no-comma-input, max-value, number-input)
    ├── enums/                       Enums TypeScript
    ├── interfaces/                  Types / interfaces
    ├── models/                      Modèles de données
    ├── pipe/                        Pipes custom (price, percentage, gift-category)
    ├── services/                    Services injectables (auth, axios, screen, discord)
    └── shared.module.ts             SharedModule avec exports
```

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
| Pipe | `src/front/src/app/shared/pipe/` | `{name}.pipe.ts` |
| Directive | `src/front/src/app/shared/directives/` | `{name}.directive.ts` |
| Route | `src/front/src/app/app-routing.module.ts` | Add to `routes` array |

---

## Règles Angular 17 — Fondamentaux

### 1. NgModules obligatoires

**Tous** les composants sont déclarés dans un NgModule. Pas de `standalone: true` dans ce projet.

```typescript
// feature.module.ts
@NgModule({
  declarations: [FeatureComponent],
  imports: [CommonModule, SharedModule, ReactiveFormsModule],
  exports: [FeatureComponent],
})
export class FeatureModule {}
```

Tout nouveau composant **doit** être déclaré dans le module approprié :
- Composant feature → dans le `{feature}.module.ts`
- Composant partagé → dans `shared.module.ts` (declarations + exports)
- Composant global → dans `app.module.ts`

### 2. Injection via le constructeur

Ce projet utilise l'injection par constructeur :

```typescript
export class MyComponent {
  constructor(
    private fb: FormBuilder,
    private axiosService: AxiosService,
    private authService: AuthService,
    private router: Router,
    protected screenService: ScreenService, // protected si accédé dans le template
  ) {}
}
```

**Règles :**
- `private` pour les dépendances utilisées uniquement dans le `.ts`
- `protected` pour les dépendances accédées dans le template (`.html`)
- Services API en `private` (le composant appelle des méthodes, pas le template directement)

### 3. Template — Syntaxe de flux de contrôle

Angular 17 supporte la **nouvelle syntaxe** de contrôle de flux. La **privilégier** pour tout nouveau code :

```html
<!-- ✅ Nouvelle syntaxe — à privilégier -->
@if (isLoading) {
  <mat-spinner />
} @else if (hasError) {
  <p class="error">{{ errorMessage }}</p>
} @else {
  <p>Contenu chargé</p>
}

@for (item of items; track item.id) {
  <app-item [name]="item.name" />
} @empty {
  <p>Aucun élément.</p>
}

@switch (status) {
  @case ('active') { <span class="badge-green">Actif</span> }
  @case ('inactive') { <span class="badge-red">Inactif</span> }
  @default { <span>Inconnu</span> }
}
```

**`track` est obligatoire** dans `@for`. Utiliser l'identifiant unique de l'objet (`.id`), ou `$index` en dernier recours.

> **Note :** Le code existant utilise `*ngIf` / `*ngFor`. Ne pas les changer sauf si on touche déjà au template. Tout **nouveau** code doit utiliser `@if` / `@for` / `@switch`.

### 4. Visibilité des membres dans les composants

```typescript
export class MyComponent {
  // Dépendances injectées — private ou protected
  constructor(
    private router: Router,
    private myService: MyService,
    protected screenService: ScreenService,   // accédé dans le template
  ) {}

  // État exposé au template — pas de modificateur (public implicite) ou protected
  items: Item[] = [];
  isLoading = false;
  errorMessage = '';

  // Méthodes appelées depuis le template — public ou protected
  onSubmit(): void { }

  // Méthodes internes — private
  private loadData(): void { }
}
```

---

## Formulaires — Reactive Forms

```typescript
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

// Dans le composant
constructor(private fb: FormBuilder) {
  this.form = this.fb.group({
    name: ['', [Validators.required, Validators.minLength(3)]],
    email: ['', [Validators.required, Validators.email]],
  });
}

form: FormGroup;

// Soumission
onSubmit(): void {
  if (this.form.invalid) return;

  this.isLoading = true;
  this.myService.create(this.form.value)
    .then(result => {
      this.router.navigate(['/success']);
    })
    .catch(error => {
      this.errorMessage = 'Une erreur est survenue.';
    })
    .finally(() => {
      this.isLoading = false;
    });
}
```

Template :
```html
<form [formGroup]="form" (ngSubmit)="onSubmit()">
  <div class="form-group">
    <label>Nom</label>
    <input formControlName="name" class="form-control" />
    @if (form.controls['name'].hasError('required') && form.controls['name'].touched) {
      <span class="text-red-500 text-sm">Champ requis</span>
    }
  </div>

  <button type="submit" [disabled]="form.invalid || isLoading">
    @if (isLoading) { Envoi en cours... } @else { Enregistrer }
  </button>
</form>
```

---

## Services API — Pattern Axios

Ce projet utilise **Axios** (pas `HttpClient`). Toujours utiliser `AxiosService` via injection.

```typescript
import { Injectable } from '@angular/core';
import { AxiosService } from '../services/axios.service';
import { MethodEnum } from '../enums/method.enum';

@Injectable({
  providedIn: 'root',
})
export class MyApi {
  constructor(private axiosService: AxiosService) {}

  getAll(): Promise<MyModel[]> {
    return this.axiosService.request(MethodEnum.GET, '/my-endpoint', null);
  }

  getById(id: string): Promise<MyModel> {
    return this.axiosService.request(MethodEnum.GET, `/my-endpoint/${id}`, null);
  }

  create(data: MyRequest): Promise<MyModel> {
    return this.axiosService.request(MethodEnum.POST, '/my-endpoint', data);
  }

  update(data: MyRequest): Promise<MyModel> {
    return this.axiosService.request(MethodEnum.PUT, '/my-endpoint', data);
  }

  delete(id: string): Promise<void> {
    return this.axiosService.request(MethodEnum.DELETE, `/my-endpoint/${id}`, {});
  }

  uploadFile(formData: FormData): Promise<object> {
    return this.axiosService.request(MethodEnum.POST, '/my-endpoint', formData, {}, true);
  }
}
```

**Règles :**
- Retourner des `Promise<T>`, pas des `Observable<T>` pour les appels Axios.
- Les URLs d'API ne doivent **jamais** contenir le `base_url` : `AxiosService` l'ajoute automatiquement.
- Le 5e paramètre `isFormFile: true` est requis pour les uploads de fichiers.
- GET params passés dans le 3e argument (`data`) sont envoyés comme `params` par `AxiosService`.

---

## Auth Pattern — BehaviorSubjects

```typescript
import { AuthService } from '../services/auth.service';

// Vérifier l'état d'auth
this.authService.isAuthenticated$.subscribe(isAuth => { ... });
this.authService.isAdmin$.subscribe(isAdmin => { ... });
this.authService.isModerator$.subscribe(isMod => { ... });

// Nom de l'utilisateur connecté
const name = this.authService.Name;

// Stocker un token après login
this.authService.setAuthToken(response.token);

// Déconnecter
this.authService.logout();
```

---

## Interfaces TypeScript — Alignement backend

Les modèles/interfaces frontend doivent correspondre exactement aux DTOs du backend (`Mariage.Contracts`).

```typescript
// src/front/src/app/shared/models/my-feature.model.ts

// Correspond à MyFeatureResponse.cs côté backend
export interface MyFeatureModel {
  id: string;          // Guid backend → string frontend
  name: string;
  createdAt: string;   // DateTime backend → string ISO 8601 frontend
}
```

**Règles :**
- `Guid` backend → `string` frontend.
- `DateTime` backend → `string` frontend.
- Pas d'`any` — utiliser des interfaces ou `unknown` + type guard si le type est inconnu.
- Modèles dans `shared/models/`, Interfaces dans `shared/interfaces/`.

---

## Routing — app-routing.module.ts

```typescript
// app-routing.module.ts
const routes: Routes = [
  { path: 'accueil', component: AccueilComponent },
  { path: 'liste-de-mariage', component: WeddingListComponent },
  { path: 'liste-de-mariage/cadeau/:id', component: GiftComponent },
  { path: 'login', component: LoginComponent },
  {
    path: 'utilisateurs',
    component: UsersComponent,
    canActivate: [AuthGuardService],  // Routes protégées
  },
  // Route par défaut
  { path: '', redirectTo: 'accueil', pathMatch: 'full' },
  { path: '**', redirectTo: 'accueil' },
];
```

**Règles :**
- Routes définies dans `app-routing.module.ts`
- Routes protégées utilisent `canActivate: [AuthGuardService]`
- Chemins en kebab-case et en français (`liste-de-mariage`, `utilisateurs`)

---

## Guards — AuthGuardService

```typescript
// Utilisation dans les routes
canActivate: [AuthGuardService]
```

Le guard vérifie l'authentification via `AuthService.isAuthenticated()` et redirige vers `/login` si non authentifié.

---

## CoreUI + Tailwind — Utilisation combinée

### Règle de base

- **CoreUI Angular** : pour les composants interactifs (modals, buttons, icons, avatars)
- **Angular Material** : `MatProgressSpinner` pour les loaders
- **Tailwind** : pour le layout et l'espacement (flex, grid, p-, m-, gap-, w-, h-, etc.)
- **SCSS scopé** : pour les styles complexes, les animations, et les overrides

### Imports CoreUI dans un module

```typescript
import {
  ModalComponent, ModalHeaderComponent, ModalBodyComponent,
  ModalFooterComponent, ModalTitleDirective, ModalToggleDirective,
  ButtonDirective, ButtonCloseDirective, AvatarComponent
} from '@coreui/angular';
import { IconModule } from '@coreui/icons-angular';
```

### Exemple de layout combiné

```html
<!-- Layout Tailwind, composants CoreUI -->
<div class="flex flex-col gap-6 p-6 max-w-4xl mx-auto">
  <div class="flex justify-between items-center">
    <h1 class="font-wedding text-3xl">Titre</h1>
    <button cButton color="primary" (click)="onAction()">Action</button>
  </div>

  <div class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-4">
    @for (item of items; track item.id) {
      <app-product [product]="item" />
    }
  </div>
</div>
```

### Custom fonts (Tailwind)

```html
<!-- Polices du projet -->
<h1 class="font-wedding">Titre mariage</h1>       <!-- Police Wedding -->
<p class="font-windsong">Texte élégant</p>          <!-- Police WindSong -->
<p class="font-librebaskerville">Texte courant</p>  <!-- Police LibreBaskerville -->
```

---

## Pattern de chargement asynchrone — Smart Component

```typescript
@Component({
  selector: 'app-items-list',
  templateUrl: './items-list.component.html',
  styleUrl: './items-list.component.scss',
})
export class ItemsListComponent implements OnInit {
  items: ItemModel[] = [];
  isLoading = false;
  errorMessage = '';

  constructor(
    private myApi: MyApi,
    private router: Router,
  ) {}

  ngOnInit(): void {
    this.loadItems();
  }

  private loadItems(): void {
    this.isLoading = true;
    this.errorMessage = '';
    this.myApi.getAll()
      .then(result => {
        this.items = result;
      })
      .catch(() => {
        this.errorMessage = 'Erreur lors du chargement.';
      })
      .finally(() => {
        this.isLoading = false;
      });
  }

  onDelete(id: string): void {
    this.isLoading = true;
    this.myApi.delete(id)
      .then(() => {
        this.items = this.items.filter(i => i.id !== id);
      })
      .catch(() => {
        this.errorMessage = 'Erreur lors de la suppression.';
      })
      .finally(() => {
        this.isLoading = false;
      });
  }

  navigateTo(id: string): void {
    this.router.navigate(['/items', id]);
  }
}
```

---

## Enums TypeScript — Règles strictes

### Placement et fichier dédié

**Les enums DOIVENT être dans un fichier séparé** — jamais définis dans le même fichier que le composant.

**Arborescence :**
- Si l'enum est utilisé par **plusieurs features** → `src/front/src/app/shared/enums/{enum-name}.enum.ts`
- Si l'enum est utilisé par **une seule feature** → `src/front/src/app/feature/{feature}/enums/{enum-name}.enum.ts`

### Backend Enum → Frontend Dropdown

**Règle universelle** : Si un champ est un **enum au backend**, il DOIT avoir un **dropdown (`<select>` ou CoreUI select)** au frontend, pas un input texte libre.

### Template enum file

```typescript
// category.enum.ts
export enum CategoryEnum {
  HomeAppliances,
  Decorations,
  TableArts,
  Digestives,
  Furniture,
  HouseholdLinens,
  Kitchen,
  Santons,
  HoneyMoon,
}

export const CATEGORY_OPTIONS = Object.entries(CategoryEnum)
  .filter(([key]) => isNaN(Number(key)))
  .map(([key, value]) => ({ label: key, value }));
```

---

## Environnements — URLs d'API

**Jamais** hardcoder une URL d'API dans un service ou un composant.

`AxiosService` lit automatiquement `environment.API_URL`. Les services API passent juste le chemin relatif.

| Config | Fichier | API_URL |
|--------|---------|---------|
| development | `environment.development.ts` | `http://localhost:5143` |
| production | `environment.ts` | Production URL |

---

## Gestion d'erreur — Patterns recommandés

### Signal d'erreur dans le composant

```typescript
errorMessage = '';

// Dans le catch
this.errorMessage = 'Mauvais identifiant ou mot de passe.';

// Dans le template
@if (errorMessage) {
  <div class="text-red-500 text-sm mt-2">{{ errorMessage }}</div>
}
```

### Erreurs Axios — codes HTTP

```typescript
this.myApi.doSomething(data)
  .then(result => { /* success */ })
  .catch((error) => {
    if (error.response?.status === 429) {
      this.errorMessage = "Trop d'essais, veuillez réessayer dans quelques secondes.";
    } else if (error.response?.status === 401) {
      this.errorMessage = 'Identifiants incorrects.';
    } else {
      this.errorMessage = 'Une erreur est survenue.';
    }
  });
```

---

## SCSS — Conventions

```scss
// feature.component.scss

:host {
  display: block;
}

// Classes locales
.feature-card {
  // Utiliser les utilitaires Tailwind autant que possible dans le HTML
  // SCSS uniquement pour les styles complexes, animations, ou overrides
}

// Media queries — mobile first
@media (max-width: 768px) {
  .feature-card {
    // responsive adjustments
  }
}

// Pas de couleurs en dur sauf si c'est la charte du projet
// Préférer les classes Tailwind dans le template HTML
```

---

## Conventions TypeScript

- Typer explicitement les retours de fonctions async : `async load(): Promise<void>`.
- Préférer `const` et `let` plutôt que `var`.
- Pas de `any` — utiliser des interfaces ou `unknown` + type guard si le type est inconnu.
- Utiliser `?.` (optional chaining) et `??` (nullish coalescing) plutôt que les vérifications manuelles.
- Messages d'erreur et textes UI en **français**.

---

## SSR — Compatibilité

Le projet utilise `@angular/ssr`. Points d'attention :
- **Jamais** accéder directement à `window`, `document`, `localStorage` sans vérification de plateforme.
- Utiliser `isPlatformBrowser` de `@angular/common` :

```typescript
import { PLATFORM_ID } from '@angular/core';
import { isPlatformBrowser } from '@angular/common';

constructor(@Inject(PLATFORM_ID) private platformId: Object) {}

someMethod(): void {
  if (isPlatformBrowser(this.platformId)) {
    // Accès DOM/window safe
    document.body.classList.add('my-class');
  }
}
```

---

## Validation post-implémentation

Après tout changement frontend, exécuter depuis `src/front` :

```bash
npm run build       # Build de production complet (détecte erreurs template)
```

Si le build échoue, corriger les erreurs avant de commit.

---

## Checklist de génération d'une feature frontend

- [ ] Lu `MEMORY.md` avant de commencer
- [ ] 3 fichiers par composant : `.ts`, `.html`, `.scss`
- [ ] Composant déclaré dans le NgModule approprié
- [ ] `templateUrl` + `styleUrl` (jamais inline)
- [ ] Injection via constructeur avec bonne visibilité (`private` / `protected`)
- [ ] Nouvelle syntaxe de template pour nouveau code (`@if`, `@for`, `@switch`)
- [ ] `@for` avec `track`
- [ ] Route ajoutée dans `app-routing.module.ts`
- [ ] Interface/Modèle TypeScript dans `shared/models/` ou `shared/interfaces/` aligné sur le contrat backend
- [ ] Service API dans `shared/apis/` utilisant `AxiosService`
- [ ] Pas d'URL hardcodée — via `AxiosService` + chemin relatif
- [ ] Enums dans des fichiers séparés (`shared/enums/` ou `feature/{name}/enums/`)
- [ ] Backend enum → dropdown frontend
- [ ] CoreUI pour les composants interactifs, Tailwind pour le layout
- [ ] Textes UI en français
- [ ] SSR compatible (pas de `window`/`document` direct)
- [ ] `npm run build` passé

---

## Protocole de fin de tâche

1. Exécuter `npm run build` dans `src/front`.
2. Documenter les nouveaux composants/services/interfaces dans `MEMORY.md` section 9.
3. Si les contrats API ont changé, mettre à jour les interfaces frontend ET signaler la dépendance dans la PR.
