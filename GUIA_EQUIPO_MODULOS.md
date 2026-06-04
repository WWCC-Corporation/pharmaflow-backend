# Guia Simple de Trabajo - PharmaFlow Backend

Equipo: 6 integrantes  
Arquitectura: Clean Architecture + Hexagonal  
Rama de integracion: `dev`  
Responsable de arquitectura y base de datos: Diego Armando Alvarez Ccompi

## 1. Como esta dividido el proyecto

El backend tiene 4 proyectos:

```text
PharmaFlow.Domain
PharmaFlow.Application
PharmaFlow.Infrastructure
PharmaFlow.Persistence
```

En nuestro proyecto se entiende asi:

| Proyecto | Para que sirve |
| --- | --- |
| `PharmaFlow.Domain` | Aqui van las entidades y enums que saldran del scaffold. Representa el negocio. |
| `PharmaFlow.Application` | Aqui va la logica de cada modulo: Commands, Queries, Handlers y DTOs. |
| `PharmaFlow.Infrastructure` | Aqui va el `DbContext` y, cuando se implemente, repositorios y UnitOfWork. |
| `PharmaFlow.Persistence` | Esta es la API: `Program.cs`, Controllers, Swagger, CORS y configuracion web. |

## 2. Paso actual del proyecto

Ahora mismo el codigo esta limpio para que el equipo empiece a trabajar.

Ya se hizo el scaffold desde la base Neon y se generaron:

```text
PharmaFlow.Domain/Entities
PharmaFlow.Infrastructure/Context/PharmaFlowDbContext.cs
```

Las columnas especiales de PostgreSQL se corrigieron manualmente:

- `citext` se usa como `string` en C#.
- Los enums PostgreSQL estan en `PharmaFlow.Domain/Enums`.
- El `DbContext` ya tiene los mapeos de esos tipos.

## 3. Que significa cada carpeta importante

### Domain

```text
PharmaFlow.Domain/Entities
```

Aqui iran las clases que representan tablas de la base de datos, por ejemplo:

```text
Producto
Venta
Compra
Sucursal
Usuario
```

Nota: algunos valores como estado de venta, moneda o metodo de pago vienen de enums PostgreSQL. En C# estan en `PharmaFlow.Domain/Enums`.

### Infrastructure

```text
PharmaFlow.Infrastructure/Context
```

Aqui ira el `DbContext`. El `DbContext` es la clase que conecta C# con PostgreSQL/Neon.

```text
PharmaFlow.Infrastructure/Repositories
```

Aqui iran los repositorios. Un repositorio es una clase que consulta o guarda datos usando el `DbContext`.

### Application

Cada modulo tiene estas carpetas:

```text
Commands
Queries
Handlers
DTOs
```

Significado:

| Carpeta | Significado |
| --- | --- |
| `Commands` | Acciones que escriben o cambian datos. Ejemplo: crear venta, abrir caja, registrar compra. |
| `Queries` | Consultas que solo leen datos. Ejemplo: listar productos, obtener ventas por fecha. |
| `Handlers` | Clases que procesan un Command o Query. Aqui va la logica del caso de uso. |
| `DTOs` | Objetos simples para recibir o devolver datos por la API. |

### Persistence

```text
PharmaFlow.Persistence/Controllers
```

Aqui van los controllers de la API. Los controllers reciben peticiones HTTP y llaman a Application.

Ejemplo de flujo:

```text
Controller -> Command/Query -> Handler -> Repository -> DbContext -> NeonDB
```

## 4. Modulos por integrante

| Integrante | Modulo / Flujo | Carpetas principales |
| --- | --- | --- |
| Diego Armando Alvarez Ccompi | Base de datos, scaffold, arquitectura, dashboard, reportes | `Dashboard`, `Reportes`, `db/neon` |
| Alexandro Cano Narvaez | Compras y proveedores | `Compras`, `Proveedores` |
| Edson Pinto Martinez | Ventas y clientes | `Ventas`, `Clientes` |
| Kevin Lizando Usca Uscca | Caja | `Caja` |
| Mariel Valdez Lima | Inventario, productos y alertas | `Inventario`, `Productos`, `Alertas` |
| Fernando Guillen | Auth, usuarios y roles | `Auth`, `Usuarios` |

## 5. Reglas de base de datos

Solo Diego toca la base de datos.

Nadie mas debe modificar:

- `schema.sql`
- tablas
- columnas
- enums
- vistas
- indices
- relaciones
- scripts SQL

Si un integrante necesita algo de base de datos, lo comunica a Diego.

## 6. Reglas de codigo

Cada integrante debe trabajar solo su modulo.

Ejemplos:

- Kevin trabaja `Caja`.
- Edson trabaja `Ventas` y `Clientes`.
- Mariel trabaja `Inventario`, `Productos` y `Alertas`.
- Fernando trabaja `Auth` y `Usuarios`.
- Alexandro trabaja `Compras` y `Proveedores`.

Si aparece un error en otro modulo, no se toca sin avisar.

Ejemplo:

```text
Kevin trabaja Caja y aparece error en Ventas.
Kevin no modifica Ventas.
Kevin avisa a Diego y a Edson.
```

## 7. Flujo de Git

### 7.1 Clonar desde dev

```text
git clone -b dev https://github.com/WWCC-Corporation/pharmaflow-backend.git
cd pharmaflow-backend
```

Si ya tiene el repo:

```text
git checkout dev
git pull origin dev
```

### 7.2 Crear su rama

```text
git checkout -b feature/modulo-nombre
```

Ejemplos:

```text
feature/ventas-registrar-venta
feature/compras-registrar-compra
feature/caja-apertura
feature/inventario-productos
feature/auth-login
```

### 7.3 Ver cambios

Despues de programar:

```text
git status
```

Para ver detalle:

```text
git diff
```

### 7.4 Guardar en commit

```text
git add .
git commit -m "feat(modulo): descripcion clara"
```

### 7.5 Actualizar con dev antes de subir

```text
git checkout dev
git pull origin dev
git checkout feature/modulo-nombre
git merge dev
```

Si hay conflictos, resolverlos. Si el conflicto es de otro modulo, avisar a Diego.

### 7.6 Compilar

```text
dotnet build
```

Si falla por codigo propio, corregir.

Si falla por codigo de otro modulo, no tocar sin coordinar.

### 7.7 Verificar rama

Antes de subir:

```text
git branch --show-current
```

Debe mostrar:

```text
feature/modulo-nombre
```

### 7.8 Subir la rama

```text
git push origin feature/modulo-nombre
```

### 7.9 Crear Pull Request

En GitHub:

```text
base: dev
compare: feature/modulo-nombre
reviewer: Diego
```

Nunca hacer PR a `main`.

## 8. Orden correcto para empezar

1. Diego ya creo la base Neon.
2. Diego limpio el codigo.
3. Diego hizo scaffold.
4. Ya se generaron entidades y DbContext.
5. Diego sube la base limpia a `dev`.
6. Cada integrante clona `dev`.
7. Cada integrante crea su rama.
8. Cada integrante trabaja su modulo.
9. Cada integrante abre PR hacia `dev`.
