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

## Agents et skills générés par `memory-bootstrap`

Après exécution de `@memory-bootstrap`, des agents et skills supplémentaires peuvent apparaître dans `.github/agents/` et `.github/skills/` en fonction de la stack détectée. Par exemple :

- **Frontend React/Vue/Angular/Blazor** → agent frontend spécialisé
- **Aspire** → agent de debug Aspire
- **CQRS/MediatR** → skill de génération de features CQRS
- **UI/UX SaaS** → skill de design system et règles UI/UX

Ces fichiers sont documentés automatiquement dans `MEMORY.md` après le bootstrap.

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

> **Voir `MEMORY.md`** pour toutes les conventions détectées par `memory-bootstrap`.
> Ce fichier ne contient pas de conventions spécifiques — elles sont générées dynamiquement.
