# Project Memory — Mariage Edwige & Henri

> Shared memory for all Copilot agents. Always read before any task.

---

## 1. Solution Overview

- **Product goal**: Wedding website — gift registry (liste de mariage), photo sharing, guest management, event info
- **Technology stack**:
  - Backend: **.NET 10** (C# with `extension` methods syntax), **ASP.NET Core Minimal API**
  - Frontend: **Angular 21.2** with **SSR** (`@angular/ssr`)
  - Database: **PostgreSQL** via EF Core 10 + Npgsql
  - Orchestration: **.NET Aspire 13.2.1** (AppHost + ServiceDefaults)
  - Blob storage: **Azure Blob Storage** (pictures & gift images)
  - Mapping: **Mapster 7.4**
  - Validation: **FluentValidation 12.1**
  - CQRS: **MediatR 14**
  - Error handling: **ErrorOr 2.0** (result pattern)
  - Auth: **JWT Bearer** (custom implementation)
  - CSS: **Tailwind CSS 3.4** + custom fonts (Viga `font-viga`, GFSDidot `font-gfsdidot`, Montserrat `font-montserrat`)
  - UI libs: **CoreUI Angular 5.6** (standalone) + **Angular Material 21.2**
  - HTTP client (front): **Axios** (wrapped in `AxiosService`)
  - Auth (front): **@auth0/angular-jwt** + **ngx-cookie-service**
  - Notifications: **Discord webhook**
- **Architecture**: Clean Architecture + DDD + CQRS
- **Solution file**: `src/back/Mariage.slnx`

---

## 2. Project Structure

```
src/
├── back/
│   ├── Mariage.slnx                    — Solution file
│   ├── global.json                     — SDK .NET 10
│   ├── Directory.Packages.props        — Central package management
│   ├── Mariage.Api/                    — Presentation layer (Minimal API endpoints, Mapster mapping, error handling)
│   ├── Mariage.AppHost/                — .NET Aspire orchestrator (PostgreSQL + API + Frontend)
│   ├── Mariage.Application/            — Application layer (CQRS handlers, validators, interfaces)
│   ├── Mariage.Contracts/              — DTOs/Requests/Responses (incl. PaginatedResponse<T>)
│   ├── Mariage.Domain/                 — Domain layer (Aggregates, Entities, Value Objects, Errors)
│   ├── Mariage.Infrastructure/         — Infrastructure (EF Core, Repositories, JWT, Blob, Discord)
│   └── Mariage.ServiceDefaults/        — Aspire service defaults (OpenTelemetry, health checks)
├── front/
│   ├── package.json                    — Angular 21.2 app
│   ├── tailwind.config.js              — Tailwind with custom fonts
│   └── src/app/
│       ├── core/                       — Layouts (navigation, footer), login
│       ├── feature/                    — Feature modules (accueil, wedding-list, gift, photos, profil, users, mariage, maries, contact)
│       └── shared/                     — APIs, services, models, interfaces, enums, components
docs/                                   — Learning wiki (pagination, lazy loading, architecture)
```

---

## 3. Backend — Business Model

### 3.1 Aggregates & Entities

| Aggregate | Class | Key Properties |
|-----------|-------|----------------|
| **User** | `UserAggregate/User.cs` | Username, Email?, Password, Salt, Role, PictureIds (favorites), Guests (owned) |
| └ Guest | `UserAggregate/Entities/Guest.cs` | FirstName, LastName, IsComing |
| **Gift** | `GiftAggregate/Gift.cs` | Name, Price, Participation, UrlImage, Category (string), GiftGivers (owned) |
| **GiftCategory** | `GiftAggregate/GiftCategory.cs` | Name (unique) — dynamic categories managed via admin UI |
| └ GiftGiver | `GiftAggregate/Entities/GiftGiver.cs` | FirstName, LastName, Email?, Amount |
| **Picture** | `PictureAggregate/Picture.cs` | UserId, UrlImage, CreatedAt |
| **Accommodation** | `AccommodationAggregate/Accommodation.cs` | Title, Description, UrlImage, AccommodationAssignments (owned) |
| └ AccommodationAssignment | `AccommodationAggregate/Entities/AccommodationAssignment.cs` | UserId, ResponseStatus (Pending/Accepted/Refused) |

### 3.2 Value Objects

| VO | Location | Type |
|----|----------|------|
| `UserId` | `UserAggregate/ValueObjects/` | Guid wrapper |
| `GuestId` | `UserAggregate/ValueObjects/` | Guid wrapper |
| `GiftId` | `GiftAggregate/ValueObjects/` | Guid wrapper |
| `GiftGiverId` | `GiftAggregate/ValueObjects/` | Guid wrapper |
| `GiftCategoryId` | `GiftAggregate/ValueObjects/` | Guid wrapper |
| `PictureId` | `PictureAggregate/ValueObject/` | Guid wrapper |
| `AccommodationId` | `AccommodationAggregate/ValueObjects/` | Guid wrapper |
| `AccommodationAssignmentId` | `AccommodationAggregate/ValueObjects/` | Guid wrapper |

### 3.3 Base Classes

- `AggregateRoot<TId>` → `Entity<TId>` (with `IEquatable`) → in `Domain/Common/Models/`
- `ValueObject` (with `GetEqualityComponents()`) → in `Domain/Common/Models/`

### 3.4 Error Handling (ErrorOr pattern)

| File | Error codes |
|------|-------------|
| `Errors.Authentication.cs` | `Auth.InvalidUsername`, `Auth.InvalidPassword` |
| `Errors.Gift.cs` | `Gift.NotFound` |
| `Errors.User.cs` | `User.DuplicateEmail`, `User.NotFoundUserWithId` |
| `Errors.Pictures.cs` | `Pictures.NotFoundPictureWithId` |
| `Errors.Participation.cs` | `Participation.AmountExceedParticipationLeft` |
| `Errors.Accommodation.cs` | `Accommodation.NotFound`, `Accommodation.UserAlreadyAssigned`, `Accommodation.AlreadyAssignedElsewhere`, `Accommodation.UserNotAssigned` |

All in `Mariage.Domain/Common/Errors/` as `partial class Errors`.

---

## 4. Backend — Operations (CQRS via MediatR)

### 4.1 Authentication
| Type | Class | Location |
|------|-------|----------|
| Command | `RegisterCommand` + Handler + Validator | `Application/Authentication/Commands/Register/` |
| Query | `LoginQuery` + Handler | `Application/Authentication/Queries/Login/` |
| Result | `AuthenticationResult` | `Application/Authentication/Common/` |

### 4.2 Gifts
| Type | Class | Location |
|------|-------|----------|
| Command | `CreateGiftCommand` | `Application/Gifts/Commands/CreateGift/` |
| Command | `CreateGiftParticipationCommand` | `Application/Gifts/Commands/CreateGiftParticipation/` |
| Command | `CreateGiftCategoryCommand` | `Application/Gifts/Commands/CreateGiftCategory/` |
| Command | `DeleteGiftCategoryCommand` | `Application/Gifts/Commands/DeleteGiftCategory/` |
| Query | `GetGiftQuery` | `Application/Gifts/Queries/GetGifts/` |
| Query | `GetGiftByIdQuery` | `Application/Gifts/Queries/GetGiftById/` |
| Query | `GetGiftCategoriesQuery` | `Application/Gifts/Queries/GetGiftCategories/` |

### 4.3 Pictures
| Type | Class | Location |
|------|-------|----------|
| Command | `CreatePictureCommand` | `Application/Pictures/Commands/CreatePicture/` |
| Command | `RemovePictureCommand` | `Application/Pictures/Commands/RemovePicture/` |
| Command | `AddPicturesToFavoritesCommand` | `Application/Pictures/Commands/AddPicturesToFavorites/` |
| Command | `RemovePictureFromFavoritesCommand` | `Application/Pictures/Commands/RemovePictureFromFavorites/` |
| Query | `GetPictureQuery` (paginated) | `Application/Pictures/Queries/GetPictures/` |
| Query | `GetPicturePhotoBoothQuery` | `Application/Pictures/Queries/GetPicturePhotoBooth/` |
| Query | `GetPicturePhotographQuery` | `Application/Pictures/Queries/GetPicturePhotograph/` |
| Query | `GetPicturesTookByUserQuery` | `Application/Pictures/Queries/GetPicturesTookByUser/` |
| Query | `GetFavoritePicturesQuery` | `Application/Pictures/Queries/GetFavoritesPictures/` |

### 4.4 UserInfos
| Type | Class | Location |
|------|-------|----------|
| Command | `ChangeEmailCommand` | `Application/UserInfos/Commands/Email/` |
| Command | `ChangeIsComingCommand` | `Application/UserInfos/Commands/IsComing/` |
| Command | `AddGuestsCommand` | `Application/UserInfos/Commands/AddGuests/` |
| Query | `GetAllUsersInfosQuery` | `Application/UserInfos/Queries/AllUsers/` |
| Query | `GetUserByIdQuery` | `Application/UserInfos/Queries/GetUserById/` |

### 4.5 Accommodations
| Type | Class | Location |
|------|-------|----------|
| Command | `CreateAccommodationCommand` | `Application/Accommodations/Commands/CreateAccommodation/` |
| Command | `UpdateAccommodationCommand` | `Application/Accommodations/Commands/UpdateAccommodation/` |
| Command | `DeleteAccommodationCommand` | `Application/Accommodations/Commands/DeleteAccommodation/` |
| Command | `AssignAccommodationCommand` | `Application/Accommodations/Commands/AssignAccommodation/` |
| Command | `UnassignAccommodationCommand` | `Application/Accommodations/Commands/UnassignAccommodation/` |
| Command | `RespondToAccommodationCommand` | `Application/Accommodations/Commands/RespondToAccommodation/` |
| Query | `GetAccommodationsQuery` | `Application/Accommodations/Queries/GetAccommodations/` |
| Query | `GetMyAccommodationQuery` | `Application/Accommodations/Queries/GetMyAccommodation/` |

### 4.6 Behaviors
- `ValidationBehavior<TRequest, TResponse>` — FluentValidation pipeline via `IPipelineBehavior`

### 4.7 Key Interfaces

| Interface | Location |
|-----------|----------|
| `IUserRepository` | `Application/Common/Interfaces/Persistence/` |
| `IGiftRepository` | `Application/Common/Interfaces/Persistence/` |
| `IPictureRepository` | `Application/Common/Interfaces/Persistence/` |
| `IJwtGenerator` | `Application/Common/Interfaces/Authentication/` |
| `IHashPassword` | `Application/Common/Interfaces/Authentication/` |
| `IAccommodationRepository` | `Application/Common/Interfaces/Persistence/` |
| `IBlobService` | `Application/Common/Interfaces/Services/` |
| `IDateTimeProvider` | `Application/Common/Interfaces/Services/` |
| `IDiscordWebhook` | `Application/Common/Interfaces/Services/` |

---

## 5. Contracts / DTOs

### 5.1 Authentication
- `RegisterRequest(Username, Password)` → `AuthenticationResponse(Id, Username, Token)`
- `LoginRequest(Username, Password)` → `AuthenticationResponse(Id, Username, Token)`

### 5.2 Gifts
- `CreateGiftRequest(Name, Price, ImageFile, Category)` → `GiftResponse(Id, Name, Price, Participation, UrlImage, Category, GiftGivers[])`
- `CreateGiftParticipationRequest(FirstName, LastName, Email, Amount)` → `GiftResponse`
- `GiftGiverResponse(Id, FirstName, LastName, Email, Amount)`

### 5.3 Pictures
- `CreatePictureRequest(ImageFile)` → `PictureResponse(Id, IsFavorite, UrlImage, Username)`
- `AddFavoritePictureRequest`, `RemovePictureFromFavoriteRequest`
- `GetPicturesPaginated`, `GetFavoritePicturesPaginated`

### 5.4 UserInfos
- `ChangeEmailRequest(Email?)` → `UserInfosResponse(Id, Username, Email, Guests[])`
- `ChangeIsComingRequest(GuestId, IsComing)` → `UserInfosResponse`
- `AddGuestsRequest(UserId, Guests[])` → `UserInfosResponse`
- `GuestResponse(Id, FirstName, LastName, IsComing)`

### 5.5 Accommodations
- `CreateAccommodationRequest(Title, Description, ImageFile)` → `AccommodationResponse(Id, Title, Description, UrlImage, Assignments[])`
- `UpdateAccommodationRequest(Title, Description, ImageFile?)` → `AccommodationResponse`
- `AssignAccommodationRequest(UserIds[])`, `RespondToAccommodationRequest(Response)`
- `AccommodationAssignmentResponse(UserId, Username, ResponseStatus)`
- `MyAccommodationResponse(Id, Title, Description, UrlImage, ResponseStatus)`

---

## 6. API Endpoints

| Route | Method | Handler | Auth |
|-------|--------|---------|------|
| `/healthz` | POST | HealthCheck | None |
| `/auth/register` | POST | RegisterCommand | Admin |
| `/auth/login` | POST | LoginQuery | None (rate limited) |
| `/wedding-list` | GET | GetGiftQuery | None |
| `/wedding-list/gift/{giftId}` | GET | GetGiftByIdQuery | None |
| `/wedding-list` | POST | CreateGiftCommand | Admin |
| `/wedding-list/{giftId}/participation` | POST | CreateGiftParticipationCommand | None |
| `/pictures` | POST | CreatePictureCommand | Auth |
| `/pictures/{id}` | DELETE | RemovePictureCommand | Auth |
| `/pictures` | GET | GetPictureQuery (paginated) | Auth |
| `/pictures-photo-booth` | GET | GetPicturePhotoBoothQuery | Auth |
| `/pictures-photograph` | GET | GetPicturePhotographQuery | Auth |
| `/pictures/took-by-user` | GET | GetPicturesTookByUserQuery | Auth |
| `/pictures/favorites` | GET | GetFavoritePicturesQuery | Auth |
| `/pictures/{pictureId}/favorites` | POST | AddPicturesToFavoritesCommand | Auth |
| `/pictures/{pictureId}/favorites` | DELETE | RemovePictureFromFavoritesCommand | Auth |
| `/user-infos/email` | PUT | ChangeEmailCommand | Auth |
| `/user-infos/is-coming` | PUT | ChangeIsComingCommand | Auth |
| `/user-infos` | GET | GetAllUsersInfosQuery | Admin |
| `/user-infos/profils` | GET | GetUserByIdQuery | Auth |
| `/user-infos/guests` | POST | AddGuestsCommand | Admin |
| `/accommodations` | GET | GetAccommodationsQuery | Admin |
| `/accommodations` | POST | CreateAccommodationCommand | Admin |
| `/accommodations/{id}` | PUT | UpdateAccommodationCommand | Admin |
| `/accommodations/{id}` | DELETE | DeleteAccommodationCommand | Admin |
| `/accommodations/{id}/assignments` | POST | AssignAccommodationCommand | Admin |
| `/accommodations/{id}/assignments/{userId}` | DELETE | UnassignAccommodationCommand | Admin |
| `/accommodations/my` | GET | GetMyAccommodationQuery | Auth |
| `/accommodations/my/response` | PUT | RespondToAccommodationCommand | Auth |

### Routing conventions
- Minimal API via static `Use{Feature}Controller()` extension methods on `IApplicationBuilder`
- `UseEndpoints()` pattern with `MapGet`/`MapPost`/`MapPut`/`MapDelete`
- Mapster mapping in endpoint lambdas
- No `/api` prefix

---

## 7. Persistence

### 7.1 ORM
- **EF Core 10** with **Npgsql** (PostgreSQL)

### 7.2 DbContext
- `MariageDbContext` in `Infrastructure/Persistence/`:
  - `DbSet<Gift> Gifts`
  - `DbSet<User> Users`
  - `DbSet<Picture> Pictures`
  - `DbSet<Accommodation> Accommodations`

### 7.3 Configurations (Fluent API)
- `UserConfiguration.cs` — UserId conversion, PictureIds as comma-separated string, Guests owned collection
- `GiftConfiguration.cs` — GiftId conversion, GiftGivers owned collection, Category as ComplexProperty
- `PictureConfiguration.cs` — PictureId/UserId conversions

### 7.4 Repositories
- `UserRepository`, `GiftRepository`, `PictureRepository`, `AccommodationRepository` in `Infrastructure/Persistence/Repositories/`

### 7.5 External Services
- `BlobService` — Azure Blob Storage (upload, delete, list photo booth/photograph)
- `DiscordWebhook` — Discord notifications
- `DateTimeProvider` — `IDateTimeProvider` implementation

### 7.6 Migrations
- `20251125124802_Initial` — single migration
- Auto-migration via `MigrateDbContextExtensions` (runs on startup as `IHostedService`)

### 7.7 Known Persistence Pitfalls
- **PictureIds stored as comma-separated string** in Users table — fragile, no FK constraint
- **Namespace mismatch**: `InfraFlowSculptor.Infrastructure.Extensions` in `MigrateDbContextExtensions.cs`

---

## 8. Authentication & Authorization

- **Provider**: Custom JWT Bearer (not ASP.NET Identity)
- **JWT generation**: `IJwtGenerator` / `JwtGenerator` in Infrastructure
- **Password hashing**: `IHashPassword` / `HashPassword` in Infrastructure
- **JWT settings**: `JwtSettings` (Issuer, Audience, Secret) from `appsettings.json`
- **Policies**: `IsAdmin` (requires `Admin` role)
- **Rate limiting**: `/auth/login` → Fixed window (3 req / 10 sec)
- **Frontend**: Token stored in cookie (`auth_token`), decoded with `@auth0/angular-jwt`
- **Claims**: `ClaimTypes.NameIdentifier` = UserId, `role` = User/Admin/Moderator

---

## 9. Frontend

### 9.1 Framework & version
- **Angular 21.2** with SSR (`@angular/ssr/node`, `server.ts`)
- **Module-based** (NgModules, not standalone components)
- **Node.js 24 LTS** required (Angular 21 requires Node >= 20.19 or 22.12 or >=24)
- All components have explicit `standalone: false` (required for Angular 19+ where default changed to `true`)

### 9.1b SSR Configuration (Angular 21)
- **Hydration**: `provideClientHydration(withIncrementalHydration())` in `AppModule` — enables incremental hydration + event replay + **HTTP Transfer Cache** (activé par défaut en Angular 19+, désactiver avec `withNoHttpTransferCache()` si besoin)
- **Server engine**: `AngularNodeAppEngine` in `server.ts` (modern pattern, replaces `CommonEngine`)
- **Server routing**: `app.routes.server.ts` with `provideServerRendering(withRoutes(serverRoutes))` in `AppServerModule`
  - **Prerender (SSG)**: `/accueil`, `/mariage/*`, `/staff-officiel`, `/contact` (static content)
  - **CSR**: `/login`, `/profils`, `/utilisateurs`, `/photos` (auth-dependent pages)
  - **SSR**: `/liste-de-mariage`, `/liste-de-mariage/cadeau/:id` (dynamic data)
  - Catch-all `**` → SSR
- **Incremental hydration**: `@defer (on viewport; hydrate on viewport)` used in `wedding-list.component.html` for below-fold category sections
- **Platform guards**: `isPlatformBrowser` checks on `AuthService`, `AxiosService`, `AuthGuardService`, `LoginComponent`, `MariageComponent`, `ExplanationModalComponent`, `ExplanationProfilModalComponent`, `PhotoListComponent`
- **DOCUMENT DI token**: Used in `PhotoListComponent`, `MariageComponent`, `ExplanationModalComponent`, `ExplanationProfilModalComponent` instead of global `document`
- **Pitfall**: `BrowserModule` must ONLY be imported in `AppModule`, never in feature modules (was removed from `GiftModule`)
- **Transfer State (SSR)**: `GiftStateService` uses `HttpClient` + `WritableSignal` for SSR pages. `provideHttpClient(withFetch())` activé dans `AppModule`. `SERVER_API_URL` token injectable pour l'URL absolue backend côté serveur (lu depuis `process.env` Aspire dans `AppServerModule`). `withHttpTransferCache()` (intégré à `provideClientHydration`) gère le cache HTTP serveur→client automatiquement.
- **Signals (AuthService)**: `isAuthenticated`, `isAdmin`, `isModerator` sont des `WritableSignal<boolean>` — remplacent les anciens `BehaviorSubject`. Templates : `authService.isAuthenticated()` (pas de pipe `async`).

### 9.2 Structure
```
src/app/
├── core/                  — layouts (navigation, footer), login
├── feature/               — accueil, wedding-list, gift, photos, profil, users, mariage, maries, contact
└── shared/
    ├── apis/              — gift.api.ts, pictures.api.ts, profil.api.ts, users.api.ts
    ├── services/          — auth, axios, auth-guard, discord-notification, screen
    ├── models/            — user, guest, picture, photoBooth
    ├── interfaces/        — product, giftGiver
    ├── enums/             — category, role, method, pictureFilter, errors
    ├── components/        — button, input, need-to-be-connected, photo-list, skeleton-photo-card, product, title-wedding
    ├── directives/
    └── pipe/
```

### 9.3 Pages / Routes

| Route | Component | Description |
|-------|-----------|-------------|
| `/accueil` | AccueilComponent | Home page |
| `/liste-de-mariage` | WeddingListComponent | Gift registry |
| `/liste-de-mariage/cadeau/:id` | GiftComponent | Gift detail |
| `/login` | LoginComponent | Login |
| `/utilisateurs` | UsersComponent | Admin user management |
| `/mariage` | MariageComponent | Wedding event info |
| `/mariage/ceremonie-religieuse` | CeremonieComponent | Religious ceremony |
| `/mariage/vin-honneur` | VinHonneurComponent | Cocktail hour |
| `/mariage/reception` | ReceptionComponent | Reception |
| `/mariage/hebergement` | HebergementComponent | Accommodation info |
| `/mariage/photos` | PhotosComponent | Photo info |
| `/maries` | MariesComponent | About the couple |
| `/contact` | ContactComponent | Contact page |
| `/photos` | PhotosMariageComponent | Photo gallery |
| `/profil` | ProfilComponent | User profile |
| `/hebergements` | AccommodationsComponent | Admin accommodation management |

### 9.4 API Services
- `GiftApi` — Gift CRUD & participation
- `PicturesApi` — Picture CRUD, favorites, photo booth/photograph
- `ProfilApi` — Guest isComing update
- `UsersApi` — User list, profile, add guests

### 9.5 HTTP & Auth
- **Axios** wrapped in `AxiosService` (not Angular HttpClient) — utilisé pour toutes les mutations et appels authentifiés
- **HttpClient** (`provideHttpClient(withFetch())`) — utilisé par `GiftStateService` pour les GET publics SSR avec TransferCache automatique
- `AuthService` — **WritableSignals** `isAuthenticated`, `isAdmin`, `isModerator` (plus de BehaviorSubject). Méthode `refreshAuth()` (anciennement `isAuthenticated()`). Dans les templates : `authService.isAuthenticated()` (appel du signal getter).
- Token stocké en cookie avec `ngx-cookie-service`
- `GiftStateService` — service SSR-aware : HttpClient pour les GET publics, signals pour l'état partagé, `SERVER_API_URL` token pour URL absolue côté serveur

### 9.6 Design system
- **Tailwind CSS 3.4** + SCSS + custom fonts (Viga `font-viga` → accents, GFSDidot `font-gfsdidot` → titres élégants, Montserrat `font-montserrat` → corps/nav)
- Polices déclarées dans `src/scss/_typography.scss` · Tailwind classes dans `tailwind.config.js`
- **CoreUI Angular** (modals, avatars, buttons)
- **Angular Material** (CDK)

#### Palette couleurs — Thème Noël dark mode (appliquée via Tailwind + CSS custom properties)
| Token | Hex | Usage |
|-------|-----|-------|
| `primary` / `--color-primary` | `#6f0523` | **Fond de page** (`background-color: body`), couleur principale |
| `primary-light` / `--color-primary-light` | `#8a0a2e` | Variante claire du primaire |
| `primary-dark` / `--color-primary-dark` | `#520418` | Fond cartes produit, inputs, modals, sidenav |
| `gold` / `--color-gold` | `#dabb7f` | **Texte principal** (titres, labels, liens), dégradés dorés |
| `gold-dark` / `--color-gold-dark` | `#b8954f` | Point médian des dégradés dorés, placeholders |
| `gold-light` / `--color-gold-light` | `#e8d4a8` | Texte paragraphes, texte secondaire, liens hover |
| `secondary` / `--color-secondary` | `#1a3c34` | **Boutons** (gradient vert sapin), soulignements actifs, en-têtes tableau, accents |
| `secondary-light` / `--color-secondary-light` | `#2d5a3f` | Bouton hover, liens hover |
- Variables SCSS dans `src/scss/_vars.scss` · Variables CSS dans `:root` (dispo dans tous les composants)
- Gradient doré standard : `linear-gradient(to right, #dabb7f 0%, #b8954f 63%, #dabb7f 100%)`
- Gradient texte (navigation/photos) : `linear-gradient(to right, #b8954f, #dabb7f, #e8d4a8, #dabb7f, #b8954f)` (shimmer doré)
- Boutons : gradient vert sapin `linear-gradient(to right, #1a3c34, #2d5a3f, #1a3c34)` avec texte doré
- Cartes/inputs/modals : fond `#520418` avec bordure gradient gold-vert sapin
- Texte body : `#dabb7f` (doré), paragraphes `#e8d4a8` (doré clair)
- CoreUI overrides globaux dans `styles.scss` (modals, selects, close buttons)

### 9.7 Environments
- Dev: `''` (empty string = relative URL) — Angular dev-server proxy in `proxy.conf.js` forwards API calls to Aspire-injected backend URL
- Prod: `https://mariage-backend-on8u.onrender.com`

### 9.8 Aspire Integration (dev)
- `proxy.conf.js` reads `services__api__https__0` / `services__api__http__0` env vars injected by Aspire
- Proxied routes: `/auth`, `/wedding-list`, `/pictures`, `/pictures-photo-booth`, `/pictures-photograph`, `/user-infos`, `/healthz`, `/accommodations`
- Angular dev server runs on port 4200 (hardcoded in AppHost)
- `moduleResolution: "bundler"` in tsconfig.json (required for subpath exports)

---

## 10. Orchestration / Infrastructure

### .NET Aspire (AppHost)
- **Version**: 13.2.1
- **ACR**: `wedding-acr` (Azure Container Registry)
- **ACA**: `aca-wedding-env` (Azure Container Apps Environment)
- **PostgreSQL** with DbGate UI, persistent data volume → database `postgresdb`
- **API project** references `postgresdb`
- **Frontend**: `AddJavaScriptApp("frontend", "./../../front", "dev")` with `WithHttpEndpoint(port: 4200)` and `WithExternalHttpEndpoints()`
- Aspire injects `services__api__https__0` / `services__api__http__0` into the npm process

### Deployment
- **Render.com** (production frontend)
- **Azure Container Apps** (backend via Aspire `azure.yaml`)

### Secrets Management
- **Azure Key Vault** `kv-weh` (RBAC-based, francecentral)
- ACA `aca-backend-weh` uses **System Assigned Managed Identity** (principalId: `6d76d2ed-c8fd-4c3e-b822-b533c8ac9854`)
- Identity has **Key Vault Secrets User** role on `kv-weh`
- All 6 env vars use ACA `secretRef` → Key Vault references (no plain-text secrets)
- Secrets in KV: `ConnectionStrings--postgresdb`, `JwtSettings--Secret`, `BlobSettings--ConnectionString`, `BlobSettings--ConnectionStringPictures`, `AppInsights--ConnectionString`, `DiscordWebhookSettings--WebhookUrl`
- To update a secret: `az keyvault secret set --vault-name kv-weh --name <secret-name> --value <new-value>` then restart the container app

---

## 11. Build & Run Commands

```bash
# Backend — Build
cd src/back && dotnet build Mariage.slnx

# Backend — Run API only
cd src/back/Mariage.Api && dotnet run

# Frontend — Install
cd src/front && npm install

# Frontend — Dev
cd src/front && npm run dev

# Frontend — Build
cd src/front && npm run build

# Full stack — Aspire (ask user to run)
cd src/back/Mariage.AppHost && dotnet run

# Migrations — Add
cd src/back && dotnet ef migrations add <Name> --project Mariage.Infrastructure --startup-project Mariage.Api

# Migrations — Update
cd src/back && dotnet ef database update --project Mariage.Infrastructure --startup-project Mariage.Api
```

---

## 12. Conventions & Patterns — Où implémenter quoi

| Ce que tu veux faire | Où chercher / créer |
|---------------------|---------------------|
| Nouvel agrégat | `Mariage.Domain/{Name}Aggregate/` — `{Name}.cs`, `Entities/`, `ValueObjects/` |
| Nouvelle commande/query | `Mariage.Application/{Feature}/Commands/{Name}/` ou `Queries/{Name}/` — Command.cs + Handler.cs + Validator.cs |
| Validation | `Mariage.Application/{Feature}/Commands/{Name}/{Name}Validator.cs` (FluentValidation) |
| Erreurs domaine | `Mariage.Domain/Common/Errors/Errors.{Feature}.cs` (partial class `Errors`) |
| Contrat (DTO) | `Mariage.Contracts/{Feature}/` |
| Endpoint API | `Mariage.Api/Controllers/{Feature}Controller.cs` — static `Use{Feature}Controller()` |
| Mapping Mapster | `Mariage.Api/Common/Mapping/{Feature}MappingConfig.cs` |
| Interface repository | `Mariage.Application/Common/Interfaces/Persistence/I{Name}Repository.cs` |
| Implémentation repository | `Mariage.Infrastructure/Persistence/Repositories/{Name}Repository.cs` |
| Config EF Core | `Mariage.Infrastructure/Persistence/Configurations/{Name}Configuration.cs` |
| Interface service | `Mariage.Application/Common/Interfaces/Services/I{Name}.cs` |
| Implémentation service | `Mariage.Infrastructure/Services/{Name}/` |
| DI présentation | `Mariage.Api/DependencyInjection.cs` → `AddPresentation()` |
| DI application | `Mariage.Application/DependencyInjection.cs` → `AddApplication()` |
| DI infrastructure | `Mariage.Infrastructure/DependencyInjection.cs` → `AddInfrastructure()` |
| Page frontend | `src/front/src/app/feature/{name}/` + route dans `app-routing.module.ts` |
| API service frontend | `src/front/src/app/shared/apis/{name}.api.ts` |
| Modèle frontend | `src/front/src/app/shared/models/{name}.model.ts` |
| Enum frontend | `src/front/src/app/shared/enums/{name}.enum.ts` |
| Composant partagé | `src/front/src/app/shared/components/{name}/` |

---

## 13. Available Agents & Skills

| Agent/Skill | Description | File |
|-------------|-------------|------|
| `dev` | Orchestrateur principal | `.github/agents/dev.agent.md` |
| `dotnet-dev` | Expert C# .NET 10 | `.github/agents/dotnet-dev.agent.md` |
| `architect` | Architecte senior — analyse, challenge, plan d'implémentation | `.github/agents/architect.agent.md` |
| `aspire-debug` | Diagnostic runtime .NET Aspire | `.github/agents/aspire-debug.agent.md` |
| `memory-bootstrap` | Explore et génère MEMORY.md | `.github/agents/memory-bootstrap.agent.md` |
| `merge-main` | Merge main avec résolution conflits | `.github/agents/merge-main.agent.md` |
| `pr-manager` | Conventions de Pull Request | `.github/agents/pr-manager.agent.md` |
| `cqrs-feature` (skill) | Génération de features CQRS | `.github/skills/cqrs-feature/SKILL.md` |

---

## 14. Known Pitfalls & Lessons Learned

- [2026-03-26] **PictureIds as comma-separated string** in Users table — fragile, no FK constraint, manual parsing
- [2026-03-26] **`extension` keyword** in `Infrastructure/DependencyInjection.cs` — C# 13/.NET 10 feature, not all tooling supports it
- [2026-03-26] **Namespace mismatch**: `InfraFlowSculptor.Infrastructure.Extensions` in `MigrateDbContextExtensions.cs` (copy-pasted)
- [2026-03-26] **No upload validation** beyond `Length == 0` check — no content-type or size limit
- [2026-03-26] **UserId extraction** from JWT claims repeated in every endpoint — no shared middleware
- [2026-03-26] **Custom password hashing** — not standard ASP.NET Identity
- [2026-03-26] **Hardcoded CORS origins** in `Program.cs`
- [2026-03-26] **Angular Module-based** (not standalone) — requires explicit `standalone: false` on ALL components/directives/pipes since Angular 19+ changed the default to `true`
- [2026-04-02] **Angular 21 + CoreUI 5.x migration**: CoreUI 5.x is standalone-only. Use standalone components in NgModule `imports[]` (not `declarations[]`). `@angular/ssr` CommonEngine moved to `@angular/ssr/node`. `moduleResolution` must be `"bundler"` (not `"node"`) to resolve subpath exports.
- [2026-03-26] **Axios** instead of Angular `HttpClient` — non-standard for Angular. **SSR pitfall**: AxiosService must check `isPlatformBrowser` and return `new Promise(() => {})` on server to prevent failed XHR requests and hydration issues. Promise resolution must be wrapped in `ngZone.run()` to guarantee change detection triggers.
- [2026-04-20] **`@angular/build:application` obligatoire pour SSR** — `@angular-devkit/build-angular:application` ne génère PAS `angular-app-engine-manifest.mjs`. Sans ce fichier, `AngularNodeAppEngine` crash au démarrage avec "Angular app engine manifest is not set". Fixer dans `angular.json` → `build.builder` → `"@angular/build:application"`. Installer avec `npm install @angular/build --legacy-peer-deps`.
- [2026-04-20] **Angular SSR SSRF protection : variable `NG_ALLOWED_HOSTS`** — La variable d'env correcte est `NG_ALLOWED_HOSTS` (pas `ANGULAR_APP_ALLOWED_ORIGINS`). Format : noms de domaine séparés par virgule, sans `https://` (ex: `mariage-edwige-henri.fr,aca-frontend-weh.whiteglacier-bc202834.francecentral.azurecontainerapps.io`). Peut aussi être configurée en code avec `new AngularNodeAppEngine({ allowedHosts })`.
- [2026-04-20] **`az acr build` crash colorama/cp1252 sur Windows** — Erreur `UnicodeEncodeError: 'charmap' codec can't encode character '\u276f'`. Fix : `$env:PYTHONUTF8 = "1"` avant la commande `az acr build`. La build ACR elle-même peut réussir malgré le crash du log streamer — vérifier avec `az acr task list-runs`.
- [2026-04-20] **ACA ne crée pas de nouvelle révision si le tag image est identique** — `az containerapp update` avec le même tag ne redéploie pas. Solution : utiliser `:latest` (si la build a écrasé le tag `:latest`) ou un tag différent pour forcer une nouvelle révision.
- [2026-04-20] **EF Core `NullReferenceException` dans MigrationsModelDiffer** — Causé par une propriété `List<T>` avec `ValueConverter` mais sans `ValueComparer`. Fix : ajouter `.Metadata.SetValueComparer(new ValueComparer<List<T>>(...))` dans la config EF.
- [2026-04-20] **Migration SQL Server incompatible avec Npgsql** — Le snapshot d'une migration créée avec SQL Server (`nvarchar(max)`, etc.) cause un crash lors de l'application avec Npgsql. Fix : supprimer toutes les migrations et recréer `InitialCreate` avec Npgsql activé.
- [2026-03-26] **No test projects** in the solution
- [2026-03-26] **`/healthz` POST** endpoint exists to wake up Render.com free plan instances

---

## 15. Changelog

| Date | Description |
|------|-------------|
| 2026-03-26 | Initial memory bootstrap — .NET 10 Clean Architecture + CQRS (MediatR, ErrorOr, Mapster, FluentValidation) + Angular 17 (Axios, Tailwind, CoreUI) + Aspire + PostgreSQL |
| 2026-04-02 | Re-bootstrap agents — rewrote angular-front (Angular 19→17, standalone→NgModules, inject→constructor, signals→classic), fixed all InfraFlowSculptor→Mariage refs across agents, removed Azure DevOps section from pr-manager, added architect agent, updated copilot-instructions.md |
| 2026-04-02 | **Migration Angular 17→21 + Aspire 13.1.2→13.2.1** — Node.js 24 LTS install, Angular 21.2.7, CoreUI 5.6.21, Angular Material 21.2, zone.js 0.16.1, TypeScript 5.9.3, moduleResolution→bundler, standalone:false on 42 files, @angular/ssr/node, proxy.conf.js Aspire integration, Microsoft packages 10.0.3→10.0.5 |
| 2026-04-02 | **Adaptation site mariage sœur** — palette couleurs (vert sapin #2D5016, bordeaux #7D2633, doré #C9A84C), date mariage 19/12/2026, carousel→1 photo fixe accueil, onglet "Les mariés"→"Staff officiel" (route /staff-officiel, fiançailles 25/05/2025), sections Le Mariage : Sanctuaire Saint-Joseph (cérémonie), Volupté (photographes), Domaine de Sarson (cocktail + dîner), hébergement sur place |
| 2026-04-02 | **Refonte palette couleurs** — primaire `#6f0523` (bordeaux profond), doré `#dabb7f` + `#b8954f`, secondaire `#0a4b52` (sarcelle) remplacent l'ancienne palette (#7D2633, #C9A84C, #2D5016). CSS custom properties dans `_vars.scss`, couleurs Tailwind dans `tailwind.config.js`. Tous les composants SCSS mis à jour (boutons, inputs, navigation, timeline, profil, etc.). |
| 2026-04-02 | **Thème Noël dark mode** — fond de page `#6f0523` (burgundy), textes en `#dabb7f` (or) et `#e8d4a8` (or clair), accents `#0a4b52` (sarcelle) pour boutons/liens. Tous les composants adaptés : cartes produit fond `#520418`, inputs/modals/toggles fond sombre, gradient texte nav `#dabb7f→#0a4b52→#dabb7f`, boutons gradient teal, hamburger gold, tables admin teal/gold. CoreUI overrides globaux (modals, selects). |
| 2026-04-02 | **Remplacement polices** — AlexBrush (Wedding)→Viga (`font-viga`), WindSong→GFSDidot (`font-gfsdidot`), LibreBaskerville→Montserrat (`font-montserrat`) dans `_typography.scss` + `tailwind.config.js` + tous les templates. Anciens dossiers AlexBrush-Regular, Libre_Baskerville, WindSong supprimés. |
| 2026-04-02 | **Vert sapin + dégradé titre doré** — secondaire `#0a4b52`→`#1a3c34` (vert sapin foncé), `#0d6370`→`#2d5a3f` (vert sapin clair). Gradient titre "Edwige & Henri" revu : `#b8954f→#dabb7f→#e8d4a8→#dabb7f→#b8954f` (shimmer doré). 12 fichiers SCSS/Tailwind mis à jour. |
| 2026-04-16 | **Refactoring pagination** — Generic `PaginatedList<T>` (Application), `PaginatedResponse<T>` (Contracts), `QueryableExtensions.ToPaginatedListAsync` (Infrastructure). All picture endpoints now return paginated responses with totalCount/hasNextPage metadata. 1-based pageNumber with defaults. FluentValidation for pagination params. Frontend updated: `PaginatedResponse<T>` model, `hasNextPage` tracking, `loading="lazy"` on images, `trackBy` on ngFor. Documentation wiki initialized in `docs/` (backend pagination, frontend lazy loading, clean architecture). |
| 2026-04-16 | **Skeleton loading photos** — Remplacement `mat-spinner` par `SkeletonPhotoCardComponent` avec shimmer doré personnalisé sur fond bordeaux. Animation CSS custom (gold gradient sweep). FadeIn transition sur photo-items. Composant réutilisable avec `@Input() count`. Documentation complète dans `docs/frontend/skeleton-loading.md` (comparatif 4 approches : ngx-skeleton-loader, @defer, animate-pulse, shimmer maison). `MatProgressSpinner` supprimé du SharedModule. |
| 2026-04-16 | **Key Vault secrets** — Créé `kv-weh` (Azure Key Vault, RBAC, francecentral). 6 secrets migrés depuis plain-text env vars. System Assigned Managed Identity activée sur `aca-backend-weh` avec rôle Key Vault Secrets User. Toutes les env vars utilisent désormais `secretRef` → KV references. Plus aucun secret en clair dans la config ACA. |
| 2026-04-17 | **Fix SSR + Axios hydration** — AxiosService: ajout `isPlatformBrowser` guard (skip requêtes côté serveur via `new Promise(() => {})`), wrapping `resolve`/`reject` dans `ngZone.run()` pour garantir la change detection. AuthGuardService: bypass SSR pour éviter redirection `/login` côté serveur. MariageComponent: guard cookies dans `ngOnInit`. |
| 2026-04-17 | **Audit SSR complet Angular 21** — `provideClientHydration(withIncrementalHydration())` (hydration incrémentale + event replay). `server.ts` modernisé (`AngularNodeAppEngine` + `createNodeRequestHandler`). Server routes (`app.routes.server.ts`) avec `RenderMode.Prerender` (pages statiques), `RenderMode.Client` (pages auth), `RenderMode.Server` (pages dynamiques). `@defer (on viewport; hydrate on viewport)` sur 7 catégories cadeaux dans wedding-list. Guards `isPlatformBrowser` + `DOCUMENT` DI token sur 6 composants (photo-list, mariage, modals). `ViewChild` remplace `document.querySelector` sur 2 composants (model-create-gift, gift). `BrowserModule` + `HttpClientModule` retirés de GiftModule. AuthService: guard SSR sur constructor. |
| 2026-04-17 | **Angular Signals + SSR Transfer State** — `AuthService` migré BehaviorSubject→WritableSignal (`isAuthenticated`, `isAdmin`, `isModerator`), méthode `isAuthenticated()` renommée `refreshAuth()`. `GiftStateService` créé (HttpClient + signals, `loadProducts`, `loadProductById`, `refreshGiftById`). `SERVER_API_URL` InjectionToken fourni dans `AppServerModule` (service discovery Aspire). `WeddingListComponent` migré AfterViewInit→OnInit+GiftStateService. `GiftComponent` migré vers GiftStateService (GET) + GiftApi (mutations). `provideHttpClient(withFetch())` ajouté à AppModule. HttpClient TransferCache activé par défaut (Angular 19+). 5 templates migrés de `| async` vers appels signal `()`. |
| 2026-04-20 | **Refactor profil/users → Angular 21 best practices** — Fix bug "page vide à la 1ère navigation" (cause : axios+Promise hors zone après hydration). Pattern cible : `HttpClient` + `Observable` + `rxResource({ stream })` + `inject()` + `signals` + `computed()` + `ChangeDetectionStrategy.OnPush`. Créé `auth.interceptor.ts` (functional `HttpInterceptorFn`, JWT Bearer, SSR-safe). `UsersApi` + `ProfilApi` réécrits HttpClient/Observable. `ProfilComponent` : `rxResource` + `computed` (profil, isLoading) + `effect` (sync FormGroup + Discord notif one-shot). `UsersComponent` : `rxResource` + tout l'état modal en signals (`selectedUserId`, `deleteUserName`…). `ToggleButtonComponent` : `input.required<>()` signal-based + `.subscribe()` ajouté (était manquant, requête ne partait pas). `ModalAddUserComponent` : AxiosService→HttpClient direct. AppModule : `withInterceptors([authInterceptor])` ajouté. AxiosService conservé pour code legacy non-refacto (login, gift.api, pictures.api, model-create-gift). |
| 2026-04-20 | **Fix "Cannot GET /accueil" en production** — `AngularNodeAppEngine.handle()` retourne `null` pour les routes CSR ou quand SSRF bloque le hostname ; l'ancien code appelait `next()` sans middleware suivant → "Cannot GET". Fix : fallback `res.sendFile('index.html', { root: browserDistFolder })` dans le handler Express + `readFile(join(browserDistFolder, 'index.html'))` dans `createNodeRequestHandler` (raw Node `ServerResponse`, pas d'Express `sendFile`). Toutes les routes `RenderMode.Prerender` → `RenderMode.Server` (Prerender non supporté avec NgModules). Piège ACA : si on redéploie avec le même image tag et qu'un `az containerapp update` sans `--revision-suffix` retourne une révision existante dont le container a été créé AVANT le nouveau push → la vieille image tourne encore. Solution : forcer une nouvelle révision avec `--revision-suffix`. Build ACR `ddd`, révision `fix-cannotget` déployée. Toutes les routes testées 200 OK. |
| 2026-04-20 | **Feature hébergements (Accommodation)** — Nouvel agrégat DDD `Accommodation` + `AccommodationAssignment` (owned). 6 commands + 2 queries CQRS (create, update, delete, assign, unassign, respond, getAll, getMy). 8 endpoints API REST (`/accommodations/*`). EF Core config + migration `AddAccommodation`. Frontend : page admin `/hebergements` (CRUD chambres + assign/unassign utilisateurs), section profil utilisateur (voir chambre assignée + accepter/refuser). AccommodationsModule + routing + navigation admin. |
| 2026-04-21 | **Dynamic gift categories + remove Honeymoon** — `GiftCategory` converted from enum VO to `AggregateRoot<GiftCategoryId>` (Name, unique index). `Gift.Category` changed from `GiftCategory` VO to `string`. 3 new CQRS operations: `GetGiftCategories`, `CreateGiftCategory`, `DeleteGiftCategory` (blocked if in use). 3 new API endpoints (`GET/POST/DELETE /wedding-list/categories`). `IGiftCategoryRepository` + EF config + migration `DynamicGiftCategories`. Frontend: `CategoryEnum` removed, replaced by dynamic categories from API. Admin UI panel to add/delete categories. All Honeymoon special-case code removed (product, gift detail, category-gift components). `FormsModule` added to `WeddingListModule`. |
| 2026-04-20 | **Fix Angular SSR manifest + Aspire/Azure infrastructure** — (1) `angular.json` builder changé de `@angular-devkit/build-angular:application` vers `@angular/build:application` (nécessaire pour générer `angular-app-engine-manifest.mjs` et résoudre "Angular app engine manifest is not set"). (2) `server.ts` réécrit : lazy manifest init via IIFE Promise, `app.set('trust proxy', true)`, `allowedHosts` depuis `NG_ALLOWED_HOSTS` env var. (3) `@angular/build` installé avec `--legacy-peer-deps` ; `Dockerfile` mis à jour (`npm ci --legacy-peer-deps`) ; `package-lock.json` régénéré. (4) `Program.cs` : `UseSqlServer` → `UseNpgsql`. (5) `UserConfiguration.cs` : `ValueComparer<List<PictureId>>` ajouté (corrige NullReferenceException dans MigrationsModelDiffer). (6) Migration `InitialSqlServer` supprimée, `InitialCreate` recréée avec types PostgreSQL. (7) `AppHost.cs` + `package.json` : port frontend 4200→4010. (8) Azure ACA `aca-frontend-weh` : `NG_ALLOWED_HOSTS` env var configurée, image déployée (révision 0000012), serveur opérationnel sans erreur. |
| 2026-04-23 | **Timeline mariage : étape hébergement dédiée** — L'information d'hébergement n'est plus affichée sous la réception. Une étape finale `/mariage/hebergement` a été ajoutée dans la frise desktop et mobile avec l'icône existante `assets/icons/lit-double.png`, et la frise desktop conserve l'alternance haut/bas des étapes. |
