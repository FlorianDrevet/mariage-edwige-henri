---
description: "Explore le projet en profondeur, génère MEMORY.md, et crée/adapte les agents et skills en fonction de la stack réelle détectée."
---

# Agent : memory-bootstrap — Initialisation intelligente du projet

> **Invoquer cet agent quand `MEMORY.md` n'existe pas, est vide, ou est obsolète.**
> Il explore le dépôt, rédige un `MEMORY.md` complet,
> et **génère automatiquement les agents et skills adaptés** à la stack réelle du projet.

---

## Objectif

1. Produire un fichier `MEMORY.md` à la racine — source de vérité pour tous les agents.
2. **Générer les agents spécialisés** adaptés à la stack détectée (frontend, Aspire, etc.).
3. **Générer les skills** adaptés à l'architecture détectée (CQRS, UI/UX, etc.).
4. **Mettre à jour** `copilot-instructions.md` avec la liste réelle des agents et skills.

---

## Protocole d'exploration — Exécuter dans cet ordre

### Phase 1 — Découverte de la stack technique

1. **Lire les fichiers de configuration racine** :
   - `*.sln`, `*.slnx` — solution(s) .NET
   - `global.json` — version SDK .NET
   - `Directory.Packages.props` / `Directory.Build.props` — gestion centralisée des packages
   - `NuGet.config` — sources NuGet

2. **Détecter le frontend** (si existant) :
   - `package.json` → identifier le framework :
     - `@angular/core` → **Angular** (noter la version)
     - `react` / `react-dom` → **React** (noter la version)
     - `vue` → **Vue** (noter la version)
     - `svelte` → **Svelte**
     - `next` → **Next.js**
     - `nuxt` → **Nuxt**
   - Si pas de `package.json` → chercher un projet Blazor (`.razor` files)
   - Lire `tsconfig.json`, `angular.json`, `vite.config.*`, `next.config.*` etc. selon le framework
   - Identifier le CSS framework : Tailwind ? Bootstrap ? Material ? Styled-components ?
   - Identifier l'outil de bundling : Webpack, Vite, esbuild...
   - Identifier la lib HTTP : Axios, fetch natif, HttpClient Angular, React Query...

3. **Lister l'arborescence `src/`** sur 3 niveaux pour cartographier les projets/modules.

4. **Lire les `*.csproj`** de chaque projet backend :
   - Framework cible (`net10.0`, `net9.0`, `net8.0`...)
   - Packages NuGet (MediatR ? FluentValidation ? ErrorOr ? AutoMapper ? Mapster ? Dapper ?)
   - Références projet (`ProjectReference`)

5. **Lire `Program.cs`** et les `DependencyInjection.cs` / `Startup.cs` pour comprendre le pipeline.

### Phase 2 — Identifier l'architecture backend

6. **Détecter le pattern architectural** :

   | Indices | Architecture probable |
   |---------|----------------------|
   | Dossiers Domain/Application/Infrastructure/Contracts | Clean Architecture + DDD |
   | MediatR + IRequest + IRequestHandler | CQRS |
   | Controllers/ avec des classes Controller | MVC |
   | Features/ avec tout dedans (handler+model+endpoint) | Vertical Slices |
   | Services/ simples + Repositories/ | N-Tier classique |
   | Minimal API `MapGet`/`MapPost` sans controllers | Minimal API |
   | Endpoints/ ou Modules/ | Modular Monolith |

7. **Détecter le pattern de persistance** :
   - EF Core (`DbContext`, `IEntityTypeConfiguration`) → noter le provider (PostgreSQL, SQL Server, SQLite...)
   - Dapper → requêtes SQL directes
   - Autre ORM
   - Pas d'ORM → accès DB direct ou pas de DB

8. **Détecter le pattern d'erreur** :
   - `ErrorOr` → result pattern
   - `OneOf` → union types
   - `Result<T>` custom → result maison
   - Exceptions classiques → try/catch standard

9. **Détecter la validation** :
   - FluentValidation → validators dédiés
   - Data Annotations → attributs sur les modèles
   - Validation manuelle → dans les handlers/controllers
   - Pas de validation structurée

10. **Détecter l'authentification** :
    - Entra ID / Azure AD → JWT Bearer
    - IdentityServer / Duende → OpenID Connect
    - ASP.NET Identity → cookie-based
    - JWT custom → token maison
    - Pas d'auth

11. **Détecter l'orchestration** :
    - `.NET Aspire` (AppHost, ServiceDefaults) → orchestration Aspire
    - Docker Compose → orchestration Docker
    - Aucun → exécution standalone

### Phase 3 — Scanner le code en détail

12. **Scanner les entités/modèles** :
    - Chercher les classes de base héritées (`AggregateRoot`, `Entity`, `BaseEntity`, etc.)
    - Lister les modèles/entités avec leurs propriétés clés
    - Documenter les relations (FK, navigation, etc.)

13. **Scanner les endpoints/controllers** :
    - Lister toutes les routes HTTP avec method, path, handler, auth
    - Identifier les conventions de routing

14. **Scanner les services/handlers** :
    - Lister les commandes/queries si CQRS
    - Lister les services si architecture classique
    - Identifier les interfaces clés

15. **Scanner la persistance** :
    - Lister les DbContext(s) et leurs DbSet
    - Lister les configurations/migrations
    - Identifier les converters/conventions

16. **Scanner les contrats/DTOs** :
    - Lister les Request/Response ou ViewModels
    - Identifier les conventions de sérialisation

### Phase 4 — Scanner le frontend (si existant)

17. **Selon le framework détecté** :
    - Lister les pages/routes
    - Lister les composants principaux
    - Lister les services d'appel API
    - Lister les interfaces/types TypeScript
    - Identifier le state management (Redux, Zustand, Signals, Pinia, NgRx...)
    - Identifier le design system utilisé
    - Identifier les environments/configs

### Phase 5 — Détecter les conventions et pièges

18. **Identifier les patterns récurrents du code** :
    - Comment les handlers/controllers accèdent à la data ?
    - Comment sont structurés les fichiers de chaque type ?
    - Y a-t-il des helpers/utilities réutilisables ?
    - Quelles conventions de nommage de fichiers et dossiers ?

19. **Chercher les pièges potentiels** :
    - Configurations implicites (JSON serialization, CORS, etc.)
    - Patterns d'auth complexes
    - Problèmes de migration/snapshot connus
    - Dépendances circulaires

---

## Phase 6 — Générer les agents et skills adaptés

### Règle : ne générer que ce qui est pertinent

Ne **pas** créer un agent ou skill si le projet n'utilise pas la technologie correspondante.

### Agents à générer conditionnellement

#### Si un frontend est détecté → Créer `.github/agents/front-dev.agent.md`

Adapter l'agent au framework réel :

```markdown
---
description: 'Expert {Framework} {Version} frontend developer. Use this agent for ALL frontend tasks.'
---
```

Contenu adapté au framework. Les templates ci-dessous sont **des guides de génération**, pas des fichiers à copier-coller. **Adapter chaque section au projet réel** en lisant le code source.

- **Angular** → Voir le template Angular détaillé ci-dessous
- **React** → Hooks, functional components, JSX, state management (Redux/Zustand/Context), etc.
- **Vue** → Composition API, `<script setup>`, Pinia, etc.
- **Blazor** → Components, @inject, @bind, etc.
- **Svelte** → Stores, reactivity, $: syntax, etc.

---

### Template de génération — Agent Angular (toutes versions)

> Ce template guide la génération de l'agent `front-dev.agent.md` pour un projet Angular.
> **Lire le code source réel** pour détecter les conventions et adapter chaque section.

**Étape 1 — Détecter la version et les patterns du projet**

Lire `package.json` → `@angular/core` version. Puis :

| Détection | Critère |
|-----------|---------|
| Version Angular | `@angular/core` dans `package.json` |
| Standalone vs NgModules | Présence de `standalone: true` dans les composants OU de `@NgModule` |
| Signals | Présence de `signal(`, `computed(`, `input(`, `model(` dans les composants |
| Zoneless | `provideExperimentalZonelessChangeDetection()` dans `app.config.ts` ou `main.ts` |
| inject() vs constructor | Présence de `inject(Service)` vs `constructor(private service: Service)` |
| Control flow syntax | Présence de `@if`, `@for` vs `*ngIf`, `*ngFor` |
| CSS framework | Tailwind (`tailwind.config`), Bootstrap, Material (`@angular/material`), PrimeNG, etc. |
| UI component library | CoreUI, Angular Material, PrimeNG, Ng-Zorro, etc. |
| HTTP client | HttpClient vs Axios vs fetch |
| Auth library | @auth0/angular-jwt, @angular/fire, MSAL, oidc-client, custom |
| State management | NgRx, Signals, BehaviorSubject, Zustand (ngx-zustand), custom |
| i18n | ngx-translate, @angular/localize, pas d'i18n |
| SSR | @angular/ssr, @nguniversal/express-engine |
| Testing | Jest, Karma, Vitest, Cypress, Playwright |

**Étape 2 — Lire 2 à 3 composants existants** pour extraire :
- Le pattern d'injection (constructor vs inject)
- Le pattern de template (inline vs templateUrl, 3 fichiers ou inline)
- Le pattern de visibilité (private/protected/public pour les services et propriétés)
- Le pattern de formulaires (Reactive Forms, Template-driven, pas de formulaires)
- Le cycle de vie utilisé (OnInit, AfterViewInit, OnDestroy, etc.)
- Le pattern d'appel API (service → composant, direct, via resolver)

**Étape 3 — Générer l'agent avec les sections suivantes** (TOUTES obligatoires) :

```
## Sections obligatoires de l'agent front-dev

1. DESCRIPTION (header YAML)
   → description: 'Expert {Framework} {Version} frontend developer. Use this agent for ALL frontend tasks.'

2. RÔLE
   → Résumé en 2 lignes du rôle de l'agent et des technologies maîtrisées.

3. PROTOCOLE OBLIGATOIRE AU DÉMARRAGE
   → 6 étapes : lire MEMORY.md, lire ce fichier, lire package.json, lire environments,
     explorer la feature folder si pertinent, lire le service API le plus proche.

4. PROJECT CONTEXT
   → Lister : version Angular, standalone/NgModules, CSS framework,
     UI library, HTTP client, Auth method, langue de l'UI.

5. STRUCTURE DES FICHIERS — RÈGLE ABSOLUE
   → Documenter le nombre de fichiers par composant (2 ou 3 : .ts, .html, .scss/.css)
   → Exemple réel de l'arborescence d'un composant du projet
   → Inline template permis ou non (détecter via les composants existants)
   → templateUrl + styleUrl (ou styleUrls) selon ce qui est utilisé

6. ARBORESCENCE DES DOSSIERS
   → Scanner `src/app/` et documenter l'arbre réel sur 3 niveaux
   → Identifier : core/, features/, shared/, pages/, layouts/, etc.
   → Documenter le rôle de chaque dossier

7. FILE CONVENTIONS (table)
   | Type | Location | Naming |
   → Feature, Shared component, API service, Service, Model, Interface,
     Enum, Pipe, Directive, Guard, Interceptor, Route

8. RÈGLES ANGULAR — FONDAMENTAUX (adapter selon la version détectée)

   8a. Composants : Standalone ou NgModules
       - Si NgModules → règles de déclaration dans les modules,
         SharedModule exports, Feature modules
       - Si Standalone → imports[] dans le composant,
         providers pour les services

   8b. Injection de dépendances
       - Si inject() → documenter le pattern avec signal d'erreur, loading, etc.
       - Si constructor → documenter les règles de visibilité (private/protected)

   8c. Template — Syntaxe de flux de contrôle
       - Angular ≥ 17 → Documenter @if/@for/@switch avec exemples
       - Documenter le `track` obligatoire dans @for
       - Note : si le code existant utilise *ngIf/*ngFor, ne les changer
         que si on touche déjà au template

   8d. Visibilité des membres
       - private pour services internes
       - protected pour services/propriétés accédés dans le template
       - public (ou pas de modifier) pour les @Input/@Output
       - private pour les méthodes internes

   8e. Signals (si Angular ≥ 16)
       - Si le projet les utilise → documenter signal(), computed(), effect()
       - Si le projet utilise input() / output() / model()
       - Si le projet n'utilise pas les Signals → noter que le code existant
         n'utilise pas les Signals mais qu'ils peuvent être recommandés pour du nouveau code

   8f. Lifecycle hooks
       - Documenter ceux utilisés dans le projet (OnInit, OnDestroy, AfterViewInit...)
       - Rappeler l'ordre d'exécution si pertinent

9. FORMULAIRES
   → Reactive Forms ou Template-driven (détecter dans le code)
   → Exemples de création, validation, soumission avec error handling
   → Pattern de messages d'erreur dans le template

10. SERVICES API — PATTERN HTTP
    → Documenter le client HTTP utilisé (HttpClient, Axios, fetch)
    → Exemples : GET, POST, PUT, DELETE, upload de fichier
    → Retour : Observable<T> ou Promise<T> selon le client
    → Règle sur les URLs (base URL automatique ou pas)
    → Pattern d'erreur dans les appels

11. AUTH PATTERN
    → Documenter le mécanisme d'auth du projet
    → Token storage (cookie, localStorage, in-memory)
    → State management auth (BehaviorSubject, Signal, NgRx)
    → Pattern de guard
    → Pattern d'interceptor (si HttpClient) ou de header injection (si Axios)

12. INTERFACES TYPESCRIPT — ALIGNEMENT BACKEND
    → Règle de mapping Guid → string, DateTime → string
    → Interdiction du type `any`
    → Convention de nommage des interfaces/models
    → Emplacement des fichiers

13. ROUTING
    → Module-based (RouterModule) ou standalone (provideRouter)
    → Lazy loading ou eager loading
    → Convention de nommage des routes (kebab-case, langue)
    → Guards utilisés
    → Route par défaut et wildcard

14. UI FRAMEWORK — UTILISATION COMBINÉE
    → Documenter l'utilisation conjointe (ex: CoreUI + Tailwind + Material CDK)
    → Quand utiliser quoi (composants interactifs, layout, styles avancés)
    → Exemples d'import dans les modules/composants
    → Custom fonts ou tokens si configurés

15. PATTERN DE CHARGEMENT ASYNCHRONE
    → Smart component pattern complet avec loading, error, data
    → Exemple TypeScript + template HTML

16. ENUMS TYPESCRIPT — RÈGLES STRICTES
    → Fichier dédié obligatoire (jamais inline dans un composant)
    → Règle backend enum → frontend dropdown
    → Emplacement : shared/enums/ ou features/{name}/enums/
    → Template de fichier enum avec OPTIONS constant

17. ENVIRONNEMENTS — URLs API
    → Pas de hardcoding d'URL
    → Documenter les fichiers environment.ts et environment.development.ts
    → Comment le base URL est injecté

18. GESTION D'ERREUR
    → Pattern d'erreur dans les composants (string, Signal, snackbar...)
    → Pattern d'erreur pour les appels API (codes HTTP)
    → Messages d'erreur dans la langue de l'UI

19. SCSS / CSS CONVENTIONS
    → :host display block si nécessaire
    → SCSS scopé vs Tailwind utilitaire
    → Media queries
    → Pas de couleurs en dur si token system

20. CONVENTIONS TYPESCRIPT
    → Typage explicite des retours async
    → const/let vs var
    → Optional chaining + nullish coalescing
    → Langue des messages d'erreur et textes UI

21. SSR COMPATIBILITÉ (si @angular/ssr détecté)
    → isPlatformBrowser pour window/document access
    → afterNextRender / afterRender si Angular ≥ 16
    → Pattern d'injection de PLATFORM_ID

22. i18n (si ngx-translate ou @angular/localize détecté)
    → Pattern de traduction dans les templates et les services
    → Fichiers de traduction (emplacement, format)
    → Language switching

23. VALIDATION POST-IMPLÉMENTATION
    → Commande build (npm run build / ng build)
    → Commande lint si disponible
    → Commande test si disponible

24. CHECKLIST DE GÉNÉRATION D'UNE FEATURE
    → Liste exhaustive de vérifications à faire après génération
    → 15-20 items couvrant : fichiers, module, template, routing, interfaces,
      service API, URLs, enums, UI, langue, SSR, build

25. PROTOCOLE DE FIN DE TÂCHE
    → Build, mise à jour MEMORY.md, signalement des dépendances
```

**Règles de rédaction de l'agent :**
- Chaque section contient des **exemples de code réels** extraits du projet (pas des templates génériques)
- Les exemples montrent le pattern tel qu'il existe, pas tel qu'il devrait être
- Si le projet mélange des patterns (ex: *ngIf et @if), documenter les deux et préciser la règle pour le nouveau code
- Les sections non applicables (ex: i18n si pas de traduction) sont **omises**, pas laissées vides
- L'agent fait **300-500 lignes** — assez détaillé pour être autonome, assez concis pour être lu

#### Si Aspire est détecté → Créer `.github/agents/aspire-debug.agent.md`

Agent de diagnostic runtime avec :
- Protocole MCP Aspire (list resources, structured logs, console logs, traces)
- Triage par priorité (config, dépendances, migrations, auth, code)
- Escalade vers dotnet-dev ou front-dev

#### Sinon → Ne PAS créer d'agents superflus

### Skills à générer conditionnellement

#### Si CQRS + MediatR détecté → Créer `.github/skills/cqrs-feature/SKILL.md`

Skill de génération de features CQRS avec :
- Structure des dossiers réelle du projet (pas un template générique)
- Patterns de code extraits du code existant (aggregate root, handler, validator, repository, endpoint)
- Convention d'erreur du projet (ErrorOr, Result, exceptions...)
- Convention de mapping du projet (Mapster, AutoMapper, manuel...)
- Checklist de génération

**Baser le skill sur le code réel**, pas sur un template théorique. Lire 2-3 exemples existants de chaque type et en extraire le pattern.

#### Si un frontend SaaS/B2B est détecté → Créer `.github/skills/ui-ux-front-saas/SKILL.md`

Skill UI/UX avec :
- Product context extrait de MEMORY.md
- Visual baseline des pages existantes
- Design system détecté
- Règles d'accessibilité et responsive

#### Autres skills possibles

| Condition | Skill à créer |
|-----------|--------------|
| Projet avec tests (xUnit/NUnit/MSTest) | `.github/skills/testing/SKILL.md` |
| Pipeline CI/CD existant | `.github/skills/ci-cd/SKILL.md` |
| Documentation structurée | `.github/skills/documentation/SKILL.md` |

---

## Phase 7 — Générer MEMORY.md

Le fichier **DOIT** contenir ces sections (adapter les noms au projet réel) :

```markdown
# Project Memory — {NomDuProjet}

> Shared memory for all Copilot agents. Always read before any task.

## 1. Solution Overview
- Product goal
- Technology stack (versions exactes)
- Architecture pattern (CQRS, MVC, Vertical Slices, etc.)
- Solution file(s)

## 2. Project Structure
- Arbre des projets/modules avec description d'une ligne chacun

## 3. Backend — Domain/Business Model
- Entités / Agrégats / Modèles avec propriétés clés
- Relations entre entités
- Codes d'erreur / gestion d'erreur

## 4. Backend — Services / Handlers / Controllers
- Par feature : lister les opérations disponibles
- Interfaces clés (repositories, services, etc.)
- Behaviors / Middlewares / Filters

## 5. Backend — Persistence
- ORM utilisé + provider DB
- DbContext(s) et DbSets
- Configurations / Conventions
- Migrations existantes
- Pièges connus (EF Core, requêtes, etc.)

## 6. Backend — API Endpoints
| Route | Method | Handler/Action | Auth |
- Conventions de routing

## 7. Backend — Contracts / DTOs
- Requests et Responses par feature
- Conventions (string pour Guid, validation attributes, etc.)

## 8. Frontend (si existant)
- Framework + version
- Structure des dossiers
- Pages/routes
- Services d'appel API
- State management
- Design system / CSS framework
- Conventions spécifiques

## 9. Authentication & Authorization
- Provider, policies, claims
- Service de user context

## 10. Orchestration / Infrastructure
- Aspire / Docker Compose / Standalone
- Ressources déclarées
- Variables d'env

## 11. Build & Run Commands
- Build backend
- Build frontend
- Run complet
- Run backend seul
- Run frontend seul
- Tests
- Migrations

## 12. Conventions & Patterns — Où implémenter quoi
| Ce que tu veux faire | Où chercher / créer |
|---------------------|---------------------|
(Remplir avec les chemins RÉELS du projet)

## 13. Available Agents & Skills
| Agent/Skill | Description | File |
|-------------|-------------|------|
| dev | Orchestrateur principal | .github/agents/dev.agent.md |
| dotnet-dev | Expert C# .NET | .github/agents/dotnet-dev.agent.md |
| front-dev | Expert {Framework} {Version} | .github/agents/front-dev.agent.md |
| (autres agents générés) | ... | ... |

## 14. Known Pitfalls & Lessons Learned
(Pièges découverts pendant l'exploration)

## 15. Changelog
| Date | Description |
|------|-------------|
| {date} | Initial memory bootstrap — detected {stack summary} |
```

---

## Phase 8 — Mettre à jour copilot-instructions.md

Après avoir généré agents et skills, mettre à jour `.github/copilot-instructions.md` pour lister :
- Tous les agents disponibles (base + générés)
- Tous les skills disponibles (générés)
- Les commandes build/run réelles du projet

---

## Règles de rédaction

### Précision
- **Vérifier** l'existence de chaque fichier avant de le documenter.
- **Lire** le code source pour extraire les informations (ne pas deviner).
- Chemins relatifs exacts depuis la racine du repo.

### Concision
- Bullet-points, pas de prose. Tableaux pour les données tabulaires.

### Actionnabilité
- Chaque section doit répondre à : "Où coder ?", "Quel pattern suivre ?", "Quels pièges éviter ?"
- La section **"Où implémenter quoi"** est **critique** — map pour tout agent recevant une user story.

### Neutralité
- Documenter ce qui **est**, pas ce qui **devrait être**.

---

## Quand utiliser cet agent

- **Premier setup** → après avoir cloné le repo.
- **Après un merge important** → MEMORY.md potentiellement obsolète.
- **Stack change** → nouveau framework frontend, migration d'architecture, etc.
- **Agent manquant** → un domaine du code n'a pas d'agent spécialisé adapté.

---

## Checklist de fin

```
MEMORY.md
[ ] Arborescence complète documentée
[ ] Stack technique avec versions exactes
[ ] Architecture identifiée (CQRS, MVC, Slices, etc.)
[ ] Entités/modèles listés avec relations
[ ] Opérations/handlers/controllers listés par feature
[ ] Endpoints documentés (routes, methods, auth)
[ ] Persistence documentée (ORM, configs, migrations)
[ ] Contrats/DTOs documentés
[ ] Frontend scanné (framework, pages, services, routes)
[ ] Section "Où implémenter quoi" remplie avec chemins réels
[ ] Commandes build/run documentées
[ ] Pièges connus documentés
[ ] Changelog initialisé

Agents & Skills
[ ] Agent frontend créé si frontend détecté
[ ] Agent aspire-debug créé si Aspire détecté
[ ] Skill cqrs-feature créé si CQRS/MediatR détecté
[ ] Skill ui-ux créé si frontend SaaS détecté
[ ] copilot-instructions.md mis à jour avec agents/skills réels
[ ] Section "Available Agents & Skills" dans MEMORY.md remplie
```

---

## Ce que cet agent NE fait PAS

- Il ne **modifie pas** le code source du projet.
- Il ne **génère pas** de code métier (aggregates, handlers, etc.).
- Il **crée uniquement** : `MEMORY.md`, agents dans `.github/agents/`, skills dans `.github/skills/`, et met à jour `copilot-instructions.md`.
- Son output est **documentaire et configuratoire**, pas fonctionnel.
