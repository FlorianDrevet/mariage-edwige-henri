# Copilot Instructions

## Getting started

> **Première utilisation ?** Lancez l'agent `@memory-bootstrap` pour qu'il explore le projet,
> rédige `MEMORY.md`, et génère les agents et skills adaptés à la stack réelle.

Ce fichier est le point d'entrée pour **tous** les agents Copilot du projet.
Il est volontairement léger — la connaissance détaillée vit dans `MEMORY.md` et dans les
agents/skills qui sont **générés automatiquement** par `memory-bootstrap` en fonction du projet.

---

## Agents de base (toujours présents)

| Agent | Rôle | Fichier |
|-------|------|---------|
| `dev` | Orchestrateur principal — lit MEMORY.md, route vers les spécialistes, charge les skills | `.github/agents/dev.agent.md` |
| `memory-bootstrap` | Explore le projet, génère MEMORY.md, crée et adapte les agents/skills à la stack | `.github/agents/memory-bootstrap.agent.md` |
| `dotnet-dev` | Expert C# .NET pour tout le code backend | `.github/agents/dotnet-dev.agent.md` |
| `merge-main` | Merge la branche main avec résolution de conflits guidée par MEMORY.md | `.github/agents/merge-main.agent.md` |
| `pr-manager` | Conventions de Pull Request (titre, description, checklist) | `.github/agents/pr-manager.agent.md` |

## Agents générés (stack détectée : .NET 10 + Angular 17 + Aspire)

| Agent | Rôle | Fichier |
|-------|------|---------|
| `front-dev` | Expert Angular 17 frontend — modules, Axios, Tailwind, CoreUI | `.github/agents/front-dev.agent.md` |
| `aspire-debug` | Diagnostic runtime .NET Aspire (logs, traces, resources) | `.github/agents/aspire-debug.agent.md` |

## Skills générés

| Skill | Description | Fichier |
|-------|-------------|---------|
| `cqrs-feature` | Génération de features CQRS (command, query, handler, validator, endpoint) — patterns extraits du code réel | `.github/skills/cqrs-feature/SKILL.md` |

---

## Skills — Concept

Les Skills sont des fichiers Markdown (`SKILL.md`) contenant une connaissance spécialisée, chargés à la demande par un agent via `read_file`.

- Pas d'outils — connaissance pure
- Lazy-loaded — chargé uniquement quand pertinent
- Composable — plusieurs agents peuvent charger le même skill
- Mis à jour indépendamment des agents

> **BLOCKING REQUIREMENT:** Quand un skill s'applique à la demande, le lire avec `read_file` AVANT de générer du code.

Les skills disponibles sont listés dans `MEMORY.md` section "Available Skills" après le bootstrap.

---

## Conventions Pull Request

Tout PR fait par un agent Copilot **doit** suivre `.github/agents/pr-manager.agent.md`.

### Format du titre

```
type(scope): description courte du but principal
```

- **type** : `feat` | `fix` | `refactor` | `perf` | `docs` | `test` | `chore` | `ci` | `style` | `revert`
- **scope** : composant en kebab-case (recommandé)
- **description** : phrase courte au présent, sans majuscule initiale, sans point final

Le titre décrit le **but principal**, jamais la dernière tâche effectuée.

### Template PR

Utiliser `.github/PULL_REQUEST_TEMPLATE.md`. Chaque fichier créé/modifié doit apparaître dans la description.

---

## Conventions spécifiques au projet

> **Voir `MEMORY.md`** pour toutes les conventions détectées.

### Build & Run

```bash
# Backend
cd src/back && dotnet build Mariage.slnx
cd src/back/Mariage.Api && dotnet run

# Frontend
cd src/front && npm install && npm run dev

# Aspire (full stack)
cd src/back/Mariage.AppHost && dotnet run

# Migrations
cd src/back && dotnet ef migrations add <Name> --project Mariage.Infrastructure --startup-project Mariage.Api
```
