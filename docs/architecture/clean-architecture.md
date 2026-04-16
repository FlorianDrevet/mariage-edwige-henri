# Clean Architecture — Guide

> Comment structurer une application .NET avec Clean Architecture, DDD et CQRS.

---

## 1. Pourquoi Clean Architecture ?

La Clean Architecture sépare les responsabilités en couches concentriques, où les dépendances pointent toujours vers l'intérieur :

```
┌─────────────────────────────────────┐
│         Presentation (API)          │  ← Endpoints, Mapping, Error Handling
├─────────────────────────────────────┤
│         Infrastructure              │  ← EF Core, Blob, JWT, Discord
├─────────────────────────────────────┤
│         Application                 │  ← CQRS Handlers, Validators, Interfaces
├─────────────────────────────────────┤
│         Domain                      │  ← Aggregates, Entities, Value Objects
└─────────────────────────────────────┘
```

**Règle d'or :** Les couches internes ne connaissent pas les couches externes.

---

## 2. Les couches dans ce projet

### 2.1 Domain (`Mariage.Domain`)

Le cœur métier. Aucune dépendance externe.

- **Aggregates** : `User`, `Gift`, `Picture`
- **Entities** : `Guest`, `GiftGiver`
- **Value Objects** : `UserId`, `GiftId`, `PictureId`, `GiftCategory`
- **Errors** : Définitions d'erreurs métier (pattern ErrorOr)

### 2.2 Application (`Mariage.Application`)

Orchestration métier via CQRS (MediatR).

- **Commands** : Actions qui modifient l'état
- **Queries** : Actions qui lisent l'état
- **Validators** : FluentValidation
- **Interfaces** : Contrats que l'Infrastructure doit implémenter

### 2.3 Infrastructure (`Mariage.Infrastructure`)

Implémentations concrètes des interfaces définies dans Application.

- **Persistence** : EF Core DbContext, Repositories
- **Authentication** : JWT Token generation, Password hashing
- **Services** : Blob Storage, Discord webhook

### 2.4 Contracts (`Mariage.Contracts`)

DTOs partagés entre l'API et les clients.

- **Requests** : Données entrantes
- **Responses** : Données sortantes (dont `PaginatedResponse<T>`)

### 2.5 Presentation (`Mariage.Api`)

Point d'entrée HTTP.

- **Endpoints** : Minimal API (static extension methods)
- **Mapping** : Mapster configuration
- **Error Handling** : Middleware global

---

## 3. Pour aller plus loin

<!-- TODO: Détailler les patterns DDD (Aggregate Root, Value Object, Domain Events) -->
<!-- TODO: Expliquer le pattern CQRS avec MediatR -->
<!-- TODO: Documenter le pattern ErrorOr pour le error handling -->
<!-- TODO: Ajouter un diagramme de flux pour une requête complète (Endpoint → Handler → Repository → DB) -->
<!-- TODO: Expliquer les Behaviors (ValidationBehavior) -->

---

## 📚 Ressources

- [Jason Taylor — Clean Architecture Template](https://github.com/jasontaylordev/CleanArchitecture)
- [Microsoft — Clean Architecture with ASP.NET Core](https://learn.microsoft.com/en-us/dotnet/architecture/modern-web-apps-azure/common-web-application-architectures)
- [Domain-Driven Design — Eric Evans](https://www.domainlanguage.com/ddd/)
