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

Agents et skills actuellement disponibles pour ce projet :

| Agent/Skill | Description | Fichier |
|-------------|-------------|---------|
| `angular-front` agent | Expert Angular 17 frontend — NgModules, Axios, Tailwind, CoreUI | `.github/agents/angular-front.agent.md` |
| `architect` agent | Architecte senior — analyse, challenge, plan d'implémentation | `.github/agents/architect.agent.md` |
| `aspire-debug` agent | Diagnostic runtime .NET Aspire | `.github/agents/aspire-debug.agent.md` |
| `cqrs-feature` skill | Génération features CQRS (patterns extraits du code réel) | `.github/skills/cqrs-feature/SKILL.md` |

> **Voir `MEMORY.md` section 13** pour la liste complète des agents et skills disponibles.

## Skills

Les skills sont des fichiers `SKILL.md` de connaissance spécialisée, chargés à la demande par les agents.
Ils sont générés par `memory-bootstrap` et listés dans `MEMORY.md`.

> **BLOCKING REQUIREMENT:** Quand un skill s'applique, l'agent DOIT le lire avec `read_file` AVANT de générer du code.

## Build & Run

> **Voir `MEMORY.md` section 11** pour les commandes de build et d'exécution spécifiques au projet.

## Conventions

> **Voir `MEMORY.md`** pour toutes les conventions détectées par `memory-bootstrap`.
