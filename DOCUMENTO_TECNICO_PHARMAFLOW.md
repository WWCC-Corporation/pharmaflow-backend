# Documento Tecnico Formal - PharmaFlow Backend

Version: 2.0  
Fecha: Junio 2026  
Repositorio: `WWCC-Corporation/pharmaflow-backend`  
Rama de integracion: `dev`  
Tech Lead / DB Architect: Diego Armando Alvarez Ccompi

## 1. Objetivo del Documento

Este documento deja definida la base tecnica formal del backend de PharmaFlow para que el equipo trabaje con una misma arquitectura, una misma estructura de carpetas, una misma base de datos y una misma forma de integracion por Pull Request.

El objetivo principal es cerrar la base del proyecto antes de que cada integrante continue desarrollando su modulo. La idea es que el equipo clone el repositorio, cree su rama desde `dev`, encuentre sus carpetas ya preparadas y programe siguiendo una convencion unica.

Este documento toma como base el estado actual del proyecto:

- Las entidades se encuentran en `PharmaFlow.Domain.Entities`.
- El `DbContext` se encuentra en `PharmaFlow.Infrastructure.Context`.
- La API se expone desde `PharmaFlow.Persistence`.
- La base de datos objetivo es PostgreSQL en Neon.
- La base debe soportar varias boticas/sucursales desde el inicio.
- La logica operativa debe vivir en backend, no duplicada en triggers.

## 2. Descripcion General del Sistema

PharmaFlow es un sistema backend para la gestion de una farmacia. Centraliza procesos de inventario, compras, ventas, caja, usuarios, roles, autenticacion, dashboard y reportes.

El sistema debe soportar operacion multi-sucursal. La clienta puede iniciar con 2 o 3 boticas y crecer a 5 o mas sin redisenar la base.

El sistema permite administrar:

- Productos y precios.
- Lotes y stock.
- Alertas de stock bajo y vencimiento.
- Proveedores.
- Compras y detalle de compras.
- Clientes.
- Ventas y detalle de ventas.
- Turnos y movimientos de caja.
- Usuarios, roles, login y refresh tokens.
- Reportes y dashboard.

La aplicacion esta desarrollada en .NET 8 y usa Entity Framework Core para mapear y consultar una base PostgreSQL alojada en Neon.

## 3. Estado Arquitectonico Aceptado

La separacion base aceptada es:

```text
PharmaFlow.Domain
PharmaFlow.Application
PharmaFlow.Infrastructure
PharmaFlow.Persistence
```

Estado correcto esperado:

- `Entities` en `PharmaFlow.Domain/Entities`.
- `Enums` en `PharmaFlow.Domain/Enums`.
- `PharmaFlowDbContext` en `PharmaFlow.Infrastructure/Context`.
- Repositorios concretos en `PharmaFlow.Infrastructure/Repositories`.
- Controllers en `PharmaFlow.Persistence/Controllers`.
- Casos de uso en `PharmaFlow.Application/Features`.

Esta base ya es coherente con Clean Architecture, siempre que se respete la regla de dependencias.

## 4. Regla Principal de Dependencias

La direccion correcta es:

```text
Persistence -> Infrastructure -> Application -> Domain
```

Reglas obligatorias:

- `Domain` no debe depender de ninguna otra capa.
- `Application` puede depender de `Domain`.
- `Application` no debe depender de `Infrastructure`.
- `Application` no debe depender de `Persistence`.
- `Infrastructure` puede depender de `Application` y `Domain`.
- `Persistence` puede depender de `Application` e `Infrastructure`.
- Controllers solo reciben HTTP, validan entrada basica y llaman casos de uso.
- Repositorios solo acceden a datos; no concentran reglas de negocio.

## 5. Decision de Arquitectura para el Equipo

Se adopta Clean Architecture + Hexagonal + SOLID con organizacion CQRS por modulo.

Problema que se debe evitar:

- Un modulo con `Commands`, `Queries`, `Handlers`.
- Otro modulo con `Services`.
- Otro modulo con logica en controllers.
- Otro modulo con repositorios haciendo reglas de negocio.

Decision final:

Los casos de uso del negocio deben implementarse como `Commands`, `Queries` y `Handlers` dentro de `Application`.

Los `Services` quedan reservados para servicios transversales o tecnicos, por ejemplo:

- JWT.
- Hash de passwords.
- Email.
- Storage.
- Servicios externos.
- Utilidades compartidas.

## 6. Estructura Oficial de Carpetas por Modulo

Cada modulo dentro de `Application/Features` debe tener esta estructura:

```text
Features/
  NombreModulo/
    Commands/
    Queries/
    Handlers/
    DTOs/
```

Ejemplo para Caja:

```text
Features/
  Caja/
    Commands/
      AbrirCajaCommand.cs
      CerrarCajaCommand.cs
      RegistrarMovimientoCajaCommand.cs
    Queries/
      ObtenerEstadoCajaQuery.cs
    Handlers/
      AbrirCajaHandler.cs
      CerrarCajaHandler.cs
      RegistrarMovimientoCajaHandler.cs
      ObtenerEstadoCajaHandler.cs
    DTOs/
      AperturaCajaDto.cs
      CierreCajaDto.cs
      MovimientoCajaDto.cs
      TurnoCajaResponseDto.cs
```

## 7. Estructura General Recomendada

### 7.1 Domain

```text
PharmaFlow.Domain/
  Entities/
  Enums/
```

### 7.2 Application

```text
PharmaFlow.Application/
  Features/
    Auth/
    Usuarios/
    Dashboard/
    Reportes/
    Ventas/
    Clientes/
    Productos/
    Alertas/
    Inventario/
    Compras/
    Proveedores/
    Caja/
```

### 7.3 Infrastructure

```text
PharmaFlow.Infrastructure/
  Context/
  Repositories/
```

### 7.4 Persistence

```text
PharmaFlow.Persistence/
  Controllers/
    Auth/
    Usuarios/
    Dashboard/
    Reportes/
    Ventas/
    Clientes/
    Productos/
    Alertas/
    Inventario/
    Compras/
    Proveedores/
    Caja/
```

## 8. Asignacion Formal de Roles y Dominios

| Desarrollador | Rol | Dominios | Responsabilidades |
| --- | --- | --- | --- |
| Diego Armando Alvarez Ccompi | Tech Lead / DB Architect | Dashboard / Reportes | Scaffolding, administracion de Entity Framework, coordinacion de integracion, aprobacion de Pull Requests en `dev`, resolucion de bloqueos tecnicos, definicion de arquitectura y control de base de datos. |
| Edson Pinto Martinez | Core Business | Ventas / Clientes | Casos de uso para ventas y clientes, registro transaccional de ventas, validacion de cliente, detalle de venta y persistencia relacionada. |
| Mariel Valdez Lima | Inventario | Productos / Alertas | CRUD de productos, manejo de lotes, stock, alertas de stock minimo, alertas de vencimiento y reglas de existencia. |
| Alexandro Cano Narvaez | Abastecimiento | Compras / Proveedores | Registro de proveedores, compras, detalle de compra, recepcion de mercaderia y actualizacion de inventario desde compras. |
| Kevin Lizando Usca Uscca | Operaciones Financieras | Caja | Apertura de caja, cierre de caja, ingresos, egresos, movimientos, cuadre y registro transaccional de caja. |
| Fernando Guillen | Seguridad | Auth / Usuarios | Identity, login, JWT, refresh tokens, roles, permisos, gestion de usuarios y configuracion de seguridad. |

## 9. Base de Datos Oficial

La base de datos oficial del backend es PostgreSQL en Neon.

Responsable unico de base de datos:

```text
Diego Armando Alvarez Ccompi
```

Regla formal:

```text
Ningun integrante del equipo modifica tablas, enums, vistas, indices,
constraints, triggers, migraciones ni scripts SQL sin aprobacion de Diego.
```

El schema limpio debe representar los objetos que Entity Framework usa actualmente:

```text
sucursales
roles
usuarios
usuario_sucursales
login_log
audit_log
refresh_token
productos
precios
proveedores
clientes
compras
detalle_compras
lotes
stock_lotes
turnos_caja
ventas
detalle_ventas
movimiento_inventario
movimientos_caja
alertas
v_stock_por_producto
v_stock_fefo
```

Modelo multi-sucursal:

- `productos` es catalogo global.
- `sucursales` representa cada botica.
- `usuario_sucursales` asigna usuarios a una o varias boticas.
- `precios`, `compras`, `lotes`, `stock_lotes`, `ventas`, `turnos_caja`, `movimientos_caja`, `movimiento_inventario` y `alertas` tienen `id_sucursal`.
- Los reportes y dashboard deben filtrar por `id_sucursal`, salvo que el usuario tenga permisos para ver consolidado.

La base debe incluir:

- Primary keys.
- Foreign keys.
- Checks.
- Defaults.
- Enums PostgreSQL.
- Indices.
- Vistas.
- Extensiones necesarias.

Scripts oficiales del repositorio:

```text
db/neon/00_reset_public_schema.sql
db/neon/01_schema_neon_clean.sql
db/neon/02_verify_database.sql
```

Orden oficial para reconstruir la base:

1. Ejecutar `00_reset_public_schema.sql`.
2. Ejecutar `01_schema_neon_clean.sql`.
3. Ejecutar `02_verify_database.sql`.
4. Revisar que la consulta de triggers no devuelva triggers operativos.

## 10. Enums Oficiales de Base de Datos

### 10.1 `estado_compra`

```text
pendiente
recepcionada
anulada
```

### 10.2 `estado_venta`

```text
completada
anulada
```

### 10.3 `tipo_movimiento`

```text
ENTRADA
SALIDA
AJUSTE
DEVOLUCION
```

### 10.4 `tipo_alerta`

```text
stock_minimo
por_vencer
vencido
otro
```

### 10.5 `metodo_pago`

```text
efectivo
tarjeta
yape
plin
```

### 10.6 `moneda`

```text
PEN
USD
```

### 10.7 `tipo_movimiento_caja`

```text
APERTURA
VENTA_EFECTIVO_INGRESO
VUELTO_SALIDA
INGRESO_MANUAL
EGRESO_MANUAL
ANULACION_INGRESO
ANULACION_EGRESO
CIERRE
```

Regla importante: el codigo no debe usar nombres de enum que no existan en base de datos. Por ejemplo, no usar `Ingreso` o `Egreso` si el enum oficial usa `INGRESO_MANUAL` y `EGRESO_MANUAL`.

## 11. Vistas Oficiales

### 11.1 `v_stock_por_producto`

Vista agregada para conocer stock total por producto.

Debe devolver:

- `id`
- `nombre`
- `codigo_barra`
- `stock_minimo`
- `stock_total`
- `estado`

Estados esperados:

```text
sin_stock
stock_bajo
ok
```

### 11.2 `v_stock_fefo`

Vista para consultar lotes disponibles bajo criterio FEFO.

Debe devolver:

- `id`
- `nombre`
- `numero_lote`
- `fecha_vencimiento`
- `stock_actual`
- `estado`

Estados esperados:

```text
vencido
por_vencer
vigente
```

## 12. Decision sobre Triggers

La decision tecnica para cerrar la base de datos es:

```text
No usar triggers operativos para compras, ventas, stock, caja, FEFO ni alertas.
```

La logica operativa debe vivir en el backend, especificamente en `Application` mediante Commands, Queries y Handlers.

Motivos:

- Evita duplicar reglas entre base de datos y backend.
- Facilita depuracion.
- Permite revisar la logica desde Pull Requests.
- Evita efectos secundarios invisibles al equipo.
- Reduce errores cuando varios modulos actualizan inventario, compras, ventas o caja.

Si existe una base antigua con triggers, se debe auditar antes de conectarla al backend final.

Para una reconstruccion limpia de Neon, los triggers operativos deben eliminarse junto con el schema anterior mediante el script:

```text
db/neon/00_reset_public_schema.sql
```

Luego el schema se recrea sin triggers operativos usando:

```text
db/neon/01_schema_neon_clean.sql
```

Triggers no recomendados:

- Trigger que actualiza `stock_lotes` despues de una compra.
- Trigger que descuenta stock despues de una venta.
- Trigger que crea alertas automaticamente.
- Trigger que crea movimientos de inventario automaticamente.
- Trigger que crea movimientos de caja automaticamente.

Elementos permitidos en base:

- Foreign keys.
- Checks.
- Defaults.
- Indices.
- Vistas.
- Enums.
- Extensiones.

## 13. Contrato entre Backend y Base de Datos

La base de datos debe ser estable y predecible. El backend debe ser el dueño de las reglas.

Ejemplo de responsabilidades:

| Proceso | Responsable de la regla |
| --- | --- |
| Crear producto | Backend |
| Validar stock minimo | Backend |
| Consultar stock total | Vista SQL |
| Registrar compra | Backend |
| Crear lote por compra | Backend |
| Aumentar stock por compra | Backend |
| Registrar venta | Backend |
| Descontar stock por venta | Backend |
| Aplicar FEFO | Backend usando datos/vista |
| Crear alerta | Backend |
| Abrir caja | Backend |
| Registrar movimiento de caja | Backend |
| Cerrar caja | Backend |
| Consultar dashboard | Backend + vistas/queries |

## 14. Conexion a Neon

La conexion debe manejarse por configuracion.

Fuentes recomendadas:

```text
PharmaFlow.Persistence/appsettings.json
PharmaFlow.Persistence/appsettings.Development.json
.env
variables de entorno
```

Nombre recomendado:

```text
ConnectionStrings__DefaultConnection
```

En desarrollo local puede existir `.env`; en despliegue deben usarse variables de entorno del servidor.

No se recomienda dejar credenciales sensibles quemadas directamente en codigo fuente.

## 15. Checklist para Cerrar la Base de Datos

Antes de dar la base como cerrada, validar:

- La base es PostgreSQL en Neon.
- El schema fue creado desde script limpio.
- La extension `pgcrypto` esta disponible.
- La extension `citext` esta disponible.
- Todos los enums existen.
- Todas las tablas oficiales existen.
- Las vistas `v_stock_por_producto` y `v_stock_fefo` existen.
- Los indices principales existen.
- No hay triggers operativos heredados de una base anterior.
- Los nombres de columnas coinciden con el `DbContext`.
- Los nombres de enums coinciden con `Domain.Enums`.
- La cadena de conexion se lee por configuracion.
- `dotnet build` compila antes de integrar cambios.

## 16. Neon vs Supabase

Para PharmaFlow se recomienda Neon como base principal si el backend .NET sera responsable de autenticacion, roles, reglas de negocio y operaciones transaccionales.

Neon encaja bien porque:

- Es PostgreSQL administrado.
- Trabaja bien con Entity Framework Core.
- Permite mantener un schema limpio.
- No obliga a usar features adicionales.
- Evita mezclar Auth/RLS externos con la logica propia del backend.

Supabase tambien es PostgreSQL, pero puede agregar complejidad si se usan Auth, RLS o triggers generados desde su plataforma. Para este proyecto, la recomendacion es mantener Neon como base limpia y al backend como dueño de la logica.

## 17. Forma de Trabajo

Regla principal:

```text
Nadie hace push directo a dev ni main.
Todo entra por Pull Request hacia dev.
```

Flujo:

```text
git clone https://github.com/WWCC-Corporation/pharmaflow-backend.git
git checkout dev
git pull origin dev
git checkout -b feature/modulo-nombre
dotnet build
git add .
git commit -m "feat(modulo): descripcion corta"
git push origin feature/modulo-nombre
```

Pull Request:

```text
base: dev
compare: feature/modulo-nombre
reviewer: Diego Armando Alvarez Ccompi
```

## 18. Convencion de Ramas

| Desarrollador | Dominio | Rama |
| --- | --- | --- |
| Diego | Dashboard / Reportes | `feature/reportes-nombre` |
| Edson | Ventas / Clientes | `feature/ventas-nombre` |
| Mariel | Inventario / Productos / Alertas | `feature/inventario-nombre` |
| Alexandro | Compras / Proveedores | `feature/compras-nombre` |
| Kevin | Caja | `feature/caja-nombre` |
| Fernando | Auth / Usuarios | `feature/auth-nombre` |

## 19. Criterios de Aprobacion de PR

Un PR puede aprobarse si:

- Compila.
- Respeta la estructura de carpetas.
- Usa CQRS para casos de uso.
- No introduce dependencias cruzadas.
- No modifica modulos ajenos sin coordinacion.
- No agrega triggers operativos.
- No hardcodea credenciales nuevas.
- Usa enums validos.
- Mantiene controllers delgados.
- Mantiene repositorios enfocados en persistencia.

Un PR debe rechazarse si:

- No compila.
- Mezcla arquitectura sin razon.
- Usa `Services` para casos de uso de negocio.
- Hace que `Application` dependa de `Infrastructure` o `Persistence`.
- Usa valores de enum inexistentes.
- Duplica reglas entre BD y backend.
- Cambia schema sin informar al Tech Lead.

## 20. Orden Recomendado para Continuar

1. Mantener `Entities` en `Domain`.
2. Mantener `DbContext` en `Infrastructure`.
3. Cerrar schema limpio de Neon.
4. Validar que no existan triggers operativos heredados.
5. Conectar backend por `DefaultConnection`.
6. Dejar carpetas base por modulo.
7. Cada integrante crea su rama desde `dev`.
8. Cada integrante trabaja solo su dominio.
9. Cada PR debe compilar.
10. Diego revisa y aprueba hacia `dev`.
11. Cuando `dev` este estable, se integra a `main`.

## 21. Conclusion

La base correcta para PharmaFlow es:

- Backend .NET 8.
- PostgreSQL en Neon.
- Entidades en `Domain`.
- `DbContext` en `Infrastructure`.
- Controllers en `Persistence`.
- Casos de uso en `Application`.
- CQRS por modulo.
- Sin triggers operativos.
- Pull Requests obligatorios hacia `dev`.

Con esta definicion, el equipo puede trabajar de forma ordenada, con responsabilidades claras y con una base de datos cerrada para desarrollo e integracion.
