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
│   ├── Mariage.Contracts/              — DTOs/Requests/Responses
│   ├── Mariage.Domain/                 — Domain layer (Aggregates, Entities, Value Objects, Errors)
│   ├── Mariage.Infrastructure/         — Infrastructure (EF Core, Repositories, JWT, Blob, Discord)
│   └── Mariage.ServiceDefaults/        — Aspire service defaults (OpenTelemetry, health checks)
├── front/
│   ├── package.json                    — Angular 17.1 app
│   ├── tailwind.config.js              — Tailwind with custom fonts
│   └── src/app/
│       ├── core/                       — Layouts (navigation, footer), login
│       ├── feature/                    — Feature modules (accueil, wedding-list, gift, photos, profil, users, mariage, maries, contact)
│       └── shared/                     — APIs, services, models, interfaces, enums, components
```

---

## 3. Backend — Business Model

### 3.1 Aggregates & Entities

| Aggregate | Class | Key Properties |
|-----------|-------|----------------|
| **User** | `UserAggregate/User.cs` | Username, Email?, Password, Salt, Role, PictureIds (favorites), Guests (owned) |
| └ Guest | `UserAggregate/Entities/Guest.cs` | FirstName, LastName, IsComing |
| **Gift** | `GiftAggregate/Gift.cs` | Name, Price, Participation, UrlImage, Category (VO), GiftGivers (owned) |
| └ GiftGiver | `GiftAggregate/Entities/GiftGiver.cs` | FirstName, LastName, Email?, Amount |
| **Picture** | `PictureAggregate/Picture.cs` | UserId, UrlImage, CreatedAt |

### 3.2 Value Objects

| VO | Location | Type |
|----|----------|------|
| `UserId` | `UserAggregate/ValueObjects/` | Guid wrapper |
| `GuestId` | `UserAggregate/ValueObjects/` | Guid wrapper |
| `GiftId` | `GiftAggregate/ValueObjects/` | Guid wrapper |
| `GiftGiverId` | `GiftAggregate/ValueObjects/` | Guid wrapper |
| `GiftCategory` | `GiftAggregate/ValueObjects/` | Enum VO (HomeAppliances, Decorations, TableArts, Digestives, Furniture, HouseholdLinens, Kitchenware, Santons, Honeymoon) |
| `PictureId` | `PictureAggregate/ValueObject/` | Guid wrapper |

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
| Query | `GetGiftQuery` | `Application/Gifts/Queries/GetGifts/` |
| Query | `GetGiftByIdQuery` | `Application/Gifts/Queries/GetGiftById/` |

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

### 4.5 Behaviors
- `ValidationBehavior<TRequest, TResponse>` — FluentValidation pipeline via `IPipelineBehavior`

### 4.6 Key Interfaces

| Interface | Location |
|-----------|----------|
| `IUserRepository` | `Application/Common/Interfaces/Persistence/` |
| `IGiftRepository` | `Application/Common/Interfaces/Persistence/` |
| `IPictureRepository` | `Application/Common/Interfaces/Persistence/` |
| `IJwtGenerator` | `Application/Common/Interfaces/Authentication/` |
| `IHashPassword` | `Application/Common/Interfaces/Authentication/` |
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

### 7.3 Configurations (Fluent API)
- `UserConfiguration.cs` — UserId conversion, PictureIds as comma-separated string, Guests owned collection
- `GiftConfiguration.cs` — GiftId conversion, GiftGivers owned collection, Category as ComplexProperty
- `PictureConfiguration.cs` — PictureId/UserId conversions

### 7.4 Repositories
- `UserRepository`, `GiftRepository`, `PictureRepository` in `Infrastructure/Persistence/Repositories/`

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
    ├── components/        — button, input, need-to-be-connected, photo-list, product, title-wedding
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
| `/mariage/photos` | PhotosComponent | Photo info |
| `/maries` | MariesComponent | About the couple |
| `/contact` | ContactComponent | Contact page |
| `/photos` | PhotosMariageComponent | Photo gallery |
| `/profil` | ProfilComponent | User profile |

### 9.4 API Services
- `GiftApi` — Gift CRUD & participation
- `PicturesApi` — Picture CRUD, favorites, photo booth/photograph
- `ProfilApi` — Guest isComing update
- `UsersApi` — User list, profile, add guests

### 9.5 HTTP & Auth
- **Axios** wrapped in `AxiosService` (not Angular HttpClient)
- `AuthService` — BehaviorSubjects for `isAuthenticated$`, `isAdmin$`, `isModerator$`
- Token stored in cookie with `ngx-cookie-service`

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
- Proxied routes: `/auth`, `/wedding-list`, `/pictures`, `/pictures-photo-booth`, `/pictures-photograph`, `/user-infos`, `/healthz`
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
- **Render.com** (production)
- **Azure Container Apps** (via Aspire `azure.yaml`)

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
- [2026-03-26] **Axios** instead of Angular `HttpClient` — non-standard for Angular
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
