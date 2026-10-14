# Instructions Copilot — Mariage Edwige & Henri

> Première utilisation ? Lancer `@memory-bootstrap` pour explorer le projet et synchroniser sa mémoire, ses agents et ses skills.

## Avant de travailler

- Lire `MEMORY.md` pour les décisions, conventions et pièges du dépôt.
- Router les changements backend vers `dotnet-dev`, frontend vers `angular-front`, l'analyse d'architecture vers `architect`, et les incidents Aspire vers `aspire-debug`.
- Lire le skill applicable avant de produire du code ou de la documentation spécialisée.
- Ne jamais recopier des secrets depuis les fichiers d'environnement, les GitHub Secrets ou Azure vers la documentation.

## Agents disponibles

| Agent | Rôle | Fichier |
|---|---|---|
| `dev` | Orchestration et routage | `.github/agents/dev.agent.md` |
| `dotnet-dev` | Backend C#/.NET 10 | `.github/agents/dotnet-dev.agent.md` |
| `angular-front` | Frontend Angular 21.2 | `.github/agents/angular-front.agent.md` |
| `architect` | Analyse et plan d'architecture | `.github/agents/architect.agent.md` |
| `aspire-debug` | Diagnostic .NET Aspire | `.github/agents/aspire-debug.agent.md` |
| `memory-bootstrap` | Exploration et mise à jour de la mémoire projet | `.github/agents/memory-bootstrap.agent.md` |
| `merge-main` | Synchronisation de la branche principale | `.github/agents/merge-main.agent.md` |
| `pr-manager` | Conventions de Pull Request | `.github/agents/pr-manager.agent.md` |

## Skills disponibles

| Skill | Quand le charger | Fichier |
|---|---|---|
| `cqrs-feature` | Créer/modifier une feature CQRS | `.github/skills/cqrs-feature/SKILL.md` |
| `ci-cd` | Modifier ou diagnostiquer les déploiements GitHub Actions, ACA ou ACR | `.github/skills/ci-cd/SKILL.md` |
| `documentation` | Créer ou mettre à jour la documentation du dépôt | `.github/skills/documentation/SKILL.md` |

## Commandes courantes (PowerShell depuis la racine)

```powershell
dotnet build .\src\back\Mariage.slnx
dotnet run --project .\src\back\Mariage.AppHost
dotnet run --project .\src\back\Mariage.Api

Push-Location .\src\front
npm ci --legacy-peer-deps
npm run dev
npm run build
Pop-Location
```

Le backend enregistre les migrations EF Core au démarrage. Les commandes manuelles de migration sont décrites dans `MEMORY.md`.

## Charger les skills

Les skills sont des fichiers Markdown sans outils, chargés à la demande. Lorsqu'un skill s'applique, lire son `SKILL.md` avant de produire du code ou un document spécialisé. Les skills disponibles sont listés ci-dessus et dans `MEMORY.md`, section 13.

## Conventions Pull Request

Tout PR préparé par un agent suit `.github/agents/pr-manager.agent.md` et le modèle `.github/PULL_REQUEST_TEMPLATE.md`.

Format du titre : `type(scope): description courte du but principal`.

- Types autorisés : `feat`, `fix`, `refactor`, `perf`, `docs`, `test`, `chore`, `ci`, `style`, `revert`.
- Décrire le but principal au présent, sans majuscule initiale ni point final; le scope kebab-case est recommandé.
- Inclure chaque fichier créé ou modifié dans la description du PR.
