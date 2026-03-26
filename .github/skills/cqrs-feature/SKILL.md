# Skill : CQRS Feature Generation

> Load this skill before generating any new CQRS feature (command, query, handler, validator, endpoint).
> Patterns extracted from the actual codebase of **Mariage**.

---

## Architecture Overview

- **MediatR 14** for CQRS dispatch
- **ErrorOr 2.0** for result pattern (`IRequest<ErrorOr<T>>`)
- **FluentValidation 12** for input validation
- **Mapster 7.4** for mapping (contracts ↔ commands/queries, domain ↔ responses)
- **Minimal API** endpoints in static controller classes

---

## Command Pattern (extracted from `CreateGiftCommand`)

### 1. Command class — `{Name}Command.cs`

```csharp
using ErrorOr;
using MediatR;

namespace Mariage.Application.{Feature}.Commands.{Name};

public record {Name}Command(
    // properties matching business need
) : IRequest<ErrorOr<{ResultType}>>;
```

### 2. Handler — `{Name}CommandHandler.cs`

```csharp
using ErrorOr;
using Mariage.Application.Common.Interfaces.Persistence;
using Mariage.Domain.Common.Errors;
using MediatR;

namespace Mariage.Application.{Feature}.Commands.{Name};

public class {Name}CommandHandler(
    I{Aggregate}Repository {aggregate}Repository
) : IRequestHandler<{Name}Command, ErrorOr<{ResultType}>>
{
    public async Task<ErrorOr<{ResultType}>> Handle(
        {Name}Command request,
        CancellationToken cancellationToken)
    {
        // 1. Validate business rules
        // 2. Create/update domain objects
        // 3. Persist
        // 4. Return result or error
    }
}
```

### 3. Validator — `{Name}CommandValidator.cs`

```csharp
using FluentValidation;

namespace Mariage.Application.{Feature}.Commands.{Name};

public class {Name}CommandValidator : AbstractValidator<{Name}Command>
{
    public {Name}CommandValidator()
    {
        RuleFor(x => x.PropertyName)
            .NotEmpty()
            .WithMessage("Property is required.");
    }
}
```

---

## Query Pattern (extracted from `GetGiftByIdQuery`)

### 1. Query class — `{Name}Query.cs`

```csharp
using ErrorOr;
using MediatR;

namespace Mariage.Application.{Feature}.Queries.{Name};

public record {Name}Query(
    // filter/id parameters
) : IRequest<ErrorOr<{ResultType}>>;
```

### 2. Handler — `{Name}QueryHandler.cs`

```csharp
using ErrorOr;
using Mariage.Application.Common.Interfaces.Persistence;
using Mariage.Domain.Common.Errors;
using MediatR;

namespace Mariage.Application.{Feature}.Queries.{Name};

public class {Name}QueryHandler(
    I{Aggregate}Repository {aggregate}Repository
) : IRequestHandler<{Name}Query, ErrorOr<{ResultType}>>
{
    public async Task<ErrorOr<{ResultType}>> Handle(
        {Name}Query request,
        CancellationToken cancellationToken)
    {
        // 1. Fetch from repository
        // 2. Return result or Errors.{Feature}.NotFound()
    }
}
```

---

## Contract Pattern (extracted from existing DTOs)

### Request — `Mariage.Contracts/{Feature}/{Name}Request.cs`

```csharp
namespace Mariage.Contracts.{Feature};

public record {Name}Request(
    // properties matching API input
);
```

### Response — `Mariage.Contracts/{Feature}/{Name}Response.cs`

```csharp
namespace Mariage.Contracts.{Feature};

public record {Name}Response(
    // properties matching API output
);
```

---

## Endpoint Pattern (extracted from existing controllers)

### Endpoint — `Mariage.Api/Controllers/{Feature}Controller.cs`

```csharp
using MapsterMapper;
using Mariage.Api.Errors;
using MediatR;

namespace Mariage.Api.Controllers;

public static class {Feature}Controller
{
    public static IApplicationBuilder Use{Feature}Controller(this IApplicationBuilder builder)
    {
        return builder.UseEndpoints(endpoints =>
        {
            endpoints.MapPost("/{route}",
                    async (IMediator mediator, IMapper mapper, {Request}Request request) =>
                    {
                        var command = mapper.Map<{Name}Command>(request);
                        var result = await mediator.Send(command);

                        return result.Match(
                            value =>
                            {
                                var response = mapper.Map<{Name}Response>(value);
                                return Results.Ok(response);
                            },
                            error => error.Result());
                    })
                .WithName("{EndpointName}")
                .WithOpenApi();
        });
    }
}
```

Then register in `Program.cs`:
```csharp
app.Use{Feature}Controller();
```

---

## Mapping Config Pattern (Mapster)

### `Mariage.Api/Common/Mapping/{Feature}MappingConfig.cs`

```csharp
using Mapster;

namespace Mariage.Api.Common.Mapping;

public class {Feature}MappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<{Request}Request, {Name}Command>();
        config.NewConfig<{DomainModel}, {Name}Response>();
    }
}
```

---

## Error Pattern

### `Mariage.Domain/Common/Errors/Errors.{Feature}.cs`

```csharp
using ErrorOr;

namespace Mariage.Domain.Common.Errors;

public static partial class Errors
{
    public static class {Feature}
    {
        public static Error {Name}NotFound() => Error.NotFound(
            code: "{Feature}.NotFound",
            description: "The {feature} was not found."
        );
    }
}
```

---

## Checklist for new CQRS feature

1. [ ] Domain aggregate/entity exists (or create in `Mariage.Domain/{Name}Aggregate/`)
2. [ ] Error codes defined in `Mariage.Domain/Common/Errors/Errors.{Feature}.cs`
3. [ ] Repository interface in `Mariage.Application/Common/Interfaces/Persistence/I{Name}Repository.cs`
4. [ ] Repository implementation in `Mariage.Infrastructure/Persistence/Repositories/{Name}Repository.cs`
5. [ ] Command/Query + Handler + Validator in `Mariage.Application/{Feature}/Commands/{Name}/` or `Queries/{Name}/`
6. [ ] Contract (Request/Response) in `Mariage.Contracts/{Feature}/`
7. [ ] Mapster config in `Mariage.Api/Common/Mapping/{Feature}MappingConfig.cs`
8. [ ] Endpoint in `Mariage.Api/Controllers/{Feature}Controller.cs`
9. [ ] Register endpoint in `Program.cs` with `app.Use{Feature}Controller()`
10. [ ] Register repository DI in `Mariage.Infrastructure/DependencyInjection.cs`
11. [ ] EF Core config if new entity in `Mariage.Infrastructure/Persistence/Configurations/`
