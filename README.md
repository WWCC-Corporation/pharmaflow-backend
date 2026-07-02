# PharmaFlow Backend

Backend de PharmaFlow organizado con una arquitectura hexagonal adaptada al proyecto.

## Estructura General

```text
PharmaFlow.Domain
  Entities
  Enums
  Ports
    Repositories
    Services

PharmaFlow.Application
  Alertas
  Auth
  Caja
  Clientes
  Compras
  Dashboard
  Inventario
  Productos
  Proveedores
  Reportes
  Usuarios
  Ventas

PharmaFlow.Infrastructure
  Data
  Adapters
    Repositories
    Services

PharmaFlow.Persistence
  Program.cs
  Controllers
  Middleware
  appsettings.json
```

## Documentacion Del Equipo

- [Responsabilidades de modulos](docs/RESPONSABILIDADES_MODULOS.md): explica que debe hacer cada modulo, donde va la logica y que le corresponde a cada integrante.

## Responsabilidad Por Proyecto

### PharmaFlow.Domain

Contiene el nucleo del negocio.

- `Entities`: entidades del dominio.
- `Enums`: enumeraciones del dominio.
- `Ports/Repositories`: contratos para repositorios.
- `Ports/Services`: contratos para servicios externos o capacidades tecnicas requeridas por el dominio.

Este proyecto no debe depender de `Application`, `Infrastructure` ni `Persistence`.

### PharmaFlow.Application

Contiene los casos de uso de la aplicacion, separados por modulo.

Cada modulo puede tener:

```text
Commands
Queries
Handlers
DTOs
Validators
Mappings
```

Ejemplo:

```text
PharmaFlow.Application
  Compras
    Commands
    Queries
    Handlers
    DTOs
    Validators
    Mappings
```

No se usa carpeta `Features`. Los modulos van directamente dentro de `PharmaFlow.Application`.

### PharmaFlow.Infrastructure

Contiene implementaciones tecnicas.

- `Data`: contexto de base de datos y configuracion relacionada a persistencia.
- `Adapters/Repositories`: implementaciones de repositorios definidos como puertos.
- `Adapters/Services`: implementaciones de servicios externos como JWT, hashing, integraciones, etc.

Ejemplo actual:

```text
PharmaFlow.Infrastructure
  Data
    PharmaFlowDbContext.cs
  Adapters
    Repositories
      Reportes
```

### PharmaFlow.Persistence

Es el proyecto principal/API.

Contiene:

- `Program.cs`
- `Controllers`
- `Middleware`
- `appsettings.json`

Aunque el proyecto se llame `Persistence`, dentro de esta solucion cumple el rol de proyecto principal, el que se ejecuta y expone la API.

## Flujo De Una Peticion

```text
Controller
  -> Command o Query
  -> Handler
  -> Port / Repository
  -> Adapter en Infrastructure
  -> DbContext o servicio externo
```

Ejemplo:

```text
ReportesController
  -> ObtenerResumenVentasQuery
  -> ObtenerResumenVentasHandler
  -> IReporteVentasReader
  -> ReporteVentasReader
  -> PharmaFlowDbContext
```

## Commands, Queries Y Handlers

### Command

Representa una accion que modifica el estado del sistema.

Ejemplos:

```text
CrearCompraCommand
ActualizarProveedorCommand
AnularVentaCommand
```

### Query

Representa una consulta de datos.

Ejemplos:

```text
ObtenerCompraPorIdQuery
ListarProductosQuery
ObtenerResumenVentasQuery
```

### Handler

Ejecuta el caso de uso asociado a un command o query.

El handler puede:

- validar reglas del caso de uso,
- consultar puertos o repositorios,
- crear o actualizar entidades,
- coordinar operaciones,
- devolver DTOs de respuesta.

Los handlers van en `PharmaFlow.Application`, dentro del modulo correspondiente.

## Reglas De Dependencia

Las dependencias deben ir en esta direccion:

```text
Persistence -> Application -> Domain
Persistence -> Infrastructure -> Application -> Domain
Infrastructure -> Application -> Domain
```

Reglas importantes:

- `Domain` no depende de ningun otro proyecto.
- `Application` puede depender de `Domain`.
- `Infrastructure` puede depender de `Application` y `Domain`.
- `Persistence` puede depender de `Application` e `Infrastructure`.
- Los controllers no deben contener logica de negocio.
- La logica de casos de uso debe estar en handlers.
- El acceso a base de datos debe estar en Infrastructure.

## Comandos Utiles

Restaurar dependencias:

```bash
dotnet restore
```

Compilar solucion:

```bash
dotnet build PharmaFlow.sln
```

Ejecutar API:

```bash
dotnet run --project PharmaFlow.Persistence
```

Actualizar la rama local con la arquitectura de `dev`:

```bash
git checkout dev
git fetch origin
git reset --hard origin/dev
git clean -fd
dotnet build PharmaFlow.sln
```

> Importante: `git reset --hard` y `git clean -fd` eliminan cambios locales no guardados.
