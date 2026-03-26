# Project Memory — {NomDuProjet}

> This file is the shared memory for all GitHub Copilot agents working on this repository.
> **Always read this file before starting any task.** Update it whenever you learn something new about the project.
> Keep entries concise, factual, and actionable. Add the date in `[YYYY-MM-DD]` when updating a section.

---

## 1. Solution Overview

**Product goal:** <!-- Décrire l'objectif produit -->

**Technology stack:**
<!-- Lister le stack technique avec versions exactes.
     Exemples de stacks possibles :
     - .NET 10, ASP.NET Core Minimal APIs, MediatR, EF Core + PostgreSQL
     - .NET 9, ASP.NET Core MVC, Dapper + SQL Server
     - .NET 8, Vertical Slices, AutoMapper, EF Core + SQLite
     - Frontend: Angular 19 / React 18 / Vue 3 / Blazor / Aucun
     - Orchestration : Aspire / Docker Compose / Standalone
-->

**Architecture pattern:** <!-- Clean Architecture + CQRS / MVC / Vertical Slices / N-Tier / Modular Monolith -->

**Solution file:** <!-- ex : MonProjet.slnx -->

---

## 2. Project Structure

<!-- L'agent memory-bootstrap remplira cette section automatiquement.
     Arbre des projets avec description d'une ligne chacun. -->

```
src/
├── ...
```

---

## 3. Backend — Business Model

<!-- Adapter cette section à l'architecture réelle du projet.
     - Si DDD : Aggregates, Entities, Value Objects
     - Si CRUD : Models / Entities avec leurs propriétés clés
     - Si Vertical Slices : Models par feature -->

### 3.1 Entities / Models

| Entity | Key Properties | Relations | Notes |
|--------|---------------|-----------|-------|
| <!-- à remplir --> | | | |

### 3.2 Business Rules / Invariants

<!-- Règles métier clés, validation domaine, contraintes -->

### 3.3 Error Handling

<!-- Pattern d'erreur : ErrorOr, Result, exceptions, etc. + codes d'erreur existants -->

---

## 4. Backend — Operations

<!-- Adapter selon l'architecture :
     - CQRS : Commands/Queries/Handlers par feature
     - MVC : Controllers et actions par feature
     - Vertical Slices : Endpoints/Features
     - Services : Service classes et méthodes -->

### 4.1 Operations par feature

<!-- Lister les opérations existantes groupées par feature/domaine -->

### 4.2 Middleware / Behaviors / Filters

<!-- Pipeline behaviors, action filters, middleware custom -->

### 4.3 Key Interfaces / Services

<!-- Interfaces clé : repositories, services métier, etc. -->

---

## 5. Contracts / DTOs

### 5.1 Requests par feature

### 5.2 Responses par feature

### 5.3 Conventions

<!-- ex : string pour Guid en JSON, validation attributes, naming, etc. -->

---

## 6. API Endpoints

| Route pattern | Method | Handler/Action | Auth |
|---------------|--------|----------------|------|
| <!-- à remplir --> | | | |

### Conventions de routing

<!-- ex : REST, prefix /api, versioning, etc. -->

---

## 7. Persistence

### 7.1 ORM / Data access

<!-- EF Core + provider / Dapper / autre — avec version -->

### 7.2 DbContext(s) / Configurations

### 7.3 Repositories / Data services

### 7.4 External Services

<!-- Blob storage, message bus, external APIs, etc. -->

### 7.5 Migrations (historique)

### 7.6 Known Persistence Pitfalls

<!-- Pièges spécifiques à l'ORM utilisé -->

---

## 8. Authentication & Authorization

<!-- Provider, policies, current user service, access check pattern -->

---

## 9. Frontend

<!-- Remplir uniquement si un frontend existe. memory-bootstrap adaptera cette section au framework détecté. -->

### 9.1 Framework & version

### 9.2 Structure des dossiers

### 9.3 Pages / Routes

### 9.4 Services API

### 9.5 State management

### 9.6 Design system / CSS framework

### 9.7 Conventions spécifiques

---

## 10. Orchestration / Infrastructure

<!-- Aspire / Docker Compose / Standalone — ressources, variables d'env, proxies -->

---

## 11. Build & Run Commands

<!-- 
- Build backend : dotnet build
- Build frontend : npm run build (dans src/Front)
- Run complet : aspire run / docker-compose up / dotnet run
- Tests : dotnet test
- Migrations : dotnet ef migrations add ...
-->

---

## 12. Conventions & Patterns — Où implémenter quoi

<!-- memory-bootstrap remplira ce tableau avec les chemins RÉELS du projet.
     Adapter les lignes à l'architecture détectée. -->

| Ce que tu veux faire | Où chercher / créer |
|---------------------|---------------------|
| <!-- à remplir par memory-bootstrap --> | |

---

## 13. Available Agents & Skills

<!-- memory-bootstrap remplira cette section avec les agents et skills qu'il a générés. -->

| Agent/Skill | Description | File |
|-------------|-------------|------|
| dev | Orchestrateur principal | .github/agents/dev.agent.md |
| dotnet-dev | Expert C# .NET | .github/agents/dotnet-dev.agent.md |
| memory-bootstrap | Exploration & setup | .github/agents/memory-bootstrap.agent.md |
| merge-main | Synchronisation main | .github/agents/merge-main.agent.md |
| pr-manager | Conventions PR | .github/agents/pr-manager.agent.md |

---

## 14. Known Pitfalls & Lessons Learned

<!-- Liste des pièges rencontrés avec date et solution -->

---

## 15. Changelog

| Date | Description |
|------|-------------|
| <!-- date --> | Initial MEMORY.md created — run `@memory-bootstrap` to populate |
