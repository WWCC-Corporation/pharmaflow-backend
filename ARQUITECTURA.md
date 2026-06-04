# Arquitectura PharmaFlow

Base recomendada: Clean Architecture + Hexagonal + SOLID.

## Capas

- `PharmaFlow.Domain`: reglas de negocio puras. Aqui viven las entidades y enums generados/adaptados desde la base de datos.
- `PharmaFlow.Application`: casos de uso. CQRS por modulo con commands, queries, handlers y DTOs.
- `PharmaFlow.Infrastructure`: adaptadores tecnicos. Aqui viven Entity Framework, el `DbContext` y los repositorios cuando se implementen.
- `PharmaFlow.Persistence`: API/Web. Aqui viven `Program.cs`, controllers, Swagger, CORS y configuracion HTTP.

## Regla de dependencias

El flujo debe ir hacia adentro:

`Persistence -> Infrastructure -> Application -> Domain`

`Domain` no debe depender de ninguna otra capa. `Application` no debe depender de `Infrastructure` ni de `Persistence`.

## Estructura por modulo en Application

Cada modulo debe seguir este formato:

```text
Features/
  NombreModulo/
    Commands/
    Queries/
    Handlers/
    DTOs/
```

Ejemplo:

```text
Features/
  Productos/
    Commands/
      CrearProductoCommand.cs
      ActualizarProductoCommand.cs
    Queries/
      ObtenerProductoPorIdQuery.cs
      ListarProductosQuery.cs
    Handlers/
      CrearProductoHandler.cs
      ObtenerProductoPorIdHandler.cs
    DTOs/
      ProductoDto.cs
      CrearProductoRequestDto.cs
```

## Convenciones

- Controllers solo reciben HTTP y llaman a Application.
- Handlers contienen el caso de uso.
- Commands cambian estado.
- Queries solo consultan.
- DTOs no deben vivir en Domain.
- Repositorios concretos viven en Infrastructure.
- Si se agregan repositorios o UnitOfWork, deben respetar la separacion de capas.
- No colocar logica de negocio en controllers ni en repositorios.
