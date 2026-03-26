# Copilot instructions

> **Première utilisation ?** Lancez l'agent `@memory-bootstrap` pour qu'il explore le projet,
> génère `MEMORY.md`, et crée les agents/skills adaptés à votre stack.

Ce dépôt utilise un système d'agents Copilot spécialisés. Le tableau ci-dessous liste les agents **de base**.
Des agents et skills supplémentaires sont **générés automatiquement** par `memory-bootstrap` en fonction du projet.

## Agents de base (toujours disponibles)

| Agent | Purpose | File |
|-------|---------|------|
| `dev` | Main entry point — reads MEMORY.md, routes to specialists, loads Skills | `.github/agents/dev.agent.md` |
| `dotnet-dev` | Expert C# .NET developer for ALL backend tasks | `.github/agents/dotnet-dev.agent.md` |
| `memory-bootstrap` | Explores the project, generates MEMORY.md, creates adapted agents/skills | `.github/agents/memory-bootstrap.agent.md` |
| `merge-main` | Merge main branch with conflict resolution using MEMORY.md | `.github/agents/merge-main.agent.md` |
| `pr-manager` | PR title/description conventions | `.github/agents/pr-manager.agent.md` |

## Agents et skills générés par `memory-bootstrap`

Après exécution de `@memory-bootstrap`, des agents et skills supplémentaires peuvent apparaître :

| Condition détectée | Agent/Skill généré | Fichier |
|-------------------|-------------------|---------|
| Frontend Angular/React/Vue/Svelte/Blazor | `front-dev` agent | `.github/agents/front-dev.agent.md` |
| .NET Aspire (AppHost) | `aspire-debug` agent | `.github/agents/aspire-debug.agent.md` |
| CQRS + MediatR | `cqrs-feature` skill | `.github/skills/cqrs-feature/SKILL.md` |
| Frontend SaaS B2B | `ui-ux-front-saas` skill | `.github/skills/ui-ux-front-saas/SKILL.md` |
| Tests structurés | `testing` skill | `.github/skills/testing/SKILL.md` |
| CI/CD pipeline | `ci-cd` skill | `.github/skills/ci-cd/SKILL.md` |

> **Voir `MEMORY.md` section 13** pour la liste complète des agents et skills disponibles.

## Skills

Les skills sont des fichiers `SKILL.md` de connaissance spécialisée, chargés à la demande par les agents.
Ils sont générés par `memory-bootstrap` et listés dans `MEMORY.md`.

> **BLOCKING REQUIREMENT:** Quand un skill s'applique, l'agent DOIT le lire avec `read_file` AVANT de générer du code.

## Build & Run

> **Voir `MEMORY.md` section 11** pour les commandes de build et d'exécution spécifiques au projet.

## Conventions

> **Voir `MEMORY.md`** pour toutes les conventions détectées par `memory-bootstrap`.
